import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { CreateInvoiceRequest, Invoice, InvoiceStatusEnum, Person } from '../../core/models/models';
import { InvoiceService } from '../../core/services/invoice.service';
import { CustomerLookupComponent } from './customer-lookup.component';

@Component({
  selector: 'app-invoice',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterLink, RouterLinkActive, CustomerLookupComponent],
  templateUrl: './invoice.component.html',
  styleUrl: './invoice.component.css'
})
export class InvoiceComponent implements OnInit {
  private readonly invoicesApi = inject(InvoiceService);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly formBuilder = inject(FormBuilder);

  readonly user = this.auth.user();
  readonly invoiceForm = this.formBuilder.nonNullable.group({
    personId: [0, [Validators.required, Validators.min(1)]],
    reference: [`INV-${Date.now()}`, Validators.required],
    invoiceDate: [new Date().toISOString().slice(0, 10), Validators.required],
    invoiceDueDate: [new Date(Date.now() + 30 * 86400000).toISOString().slice(0, 10), Validators.required]
  });
  readonly statuses = [
    { value: InvoiceStatusEnum.Draft, label: 'Draft' },
    { value: InvoiceStatusEnum.Pending, label: 'Pending' },
    { value: InvoiceStatusEnum.Sent, label: 'Sent' },
    { value: InvoiceStatusEnum.Viewed, label: 'Viewed' },
    { value: InvoiceStatusEnum.PartiallyPaid, label: 'Partially Paid' },
    { value: InvoiceStatusEnum.Paid, label: 'Paid' },
    { value: InvoiceStatusEnum.Overdue, label: 'Overdue' },
    { value: InvoiceStatusEnum.Cancelled, label: 'Cancelled' },
    { value: InvoiceStatusEnum.Refunded, label: 'Refunded' },
    { value: InvoiceStatusEnum.WrittenOff, label: 'Written Off' }
  ];

  invoices: Invoice[] = [];
  searchTerm = '';
  pageNumber = 1;
  readonly pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  loading = false;
  updatingStatusId: number | null = null;
  deactivatingId: number | null = null;
  emailingInvoiceId: number | null = null;
  recipientEmail = '';
  sendingEmail = false;
  showCreate = false;
  saving = false;
  selectedCustomer: Person | null = null;
  lineItems = [this.newLineItem()];
  lineItemsError = '';
  message = '';
  error = '';

  ngOnInit(): void {
    this.loadInvoices();
  }

  openCreate(): void {
    this.showCreate = !this.showCreate;
    this.message = '';
    if (this.showCreate) {
      this.resetInvoiceForm();
    }
  }

  selectCustomer(customer: Person): void {
    this.selectedCustomer = customer;
    this.invoiceForm.controls.personId.setValue(customer.personId);
  }

  changeCustomer(): void {
    this.selectedCustomer = null;
    this.invoiceForm.controls.personId.setValue(0);
  }

  addLineItem(): void {
    this.lineItems.push(this.newLineItem());
  }

  removeLineItem(index: number): void {
    if (this.lineItems.length > 1) {
      this.lineItems.splice(index, 1);
    }
  }

  get invoiceTotal(): number {
    return this.lineItems.reduce((total, item) => total + (Number(item.amount) || 0), 0);
  }

  createInvoice(): void {
    if (!this.selectedCustomer || this.invoiceForm.invalid) {
      this.invoiceForm.markAllAsTouched();
      return;
    }

    if (this.lineItems.some((item) => !item.reference.trim() || !Number.isFinite(Number(item.amount)) || Number(item.amount) <= 0)) {
      this.lineItemsError = 'Add a description and positive amount for each line item.';
      return;
    }

    this.saving = true;
    this.error = '';
    this.lineItemsError = '';
    const request: CreateInvoiceRequest = {
      ...this.invoiceForm.getRawValue(),
      invoiceStatusEnum: InvoiceStatusEnum.Draft,
      invoiceAmount: this.invoiceTotal,
      createdBy: this.user?.email || 'web-ui',
      lineItems: this.lineItems.map((item) => ({ reference: item.reference.trim(), amount: Number(item.amount) }))
    };

    this.invoicesApi.create(request).subscribe({
      next: () => {
        this.saving = false;
        this.showCreate = false;
        this.message = 'Invoice created successfully.';
        this.loadInvoices();
      },
      error: (error: unknown) => {
        this.saving = false;
        this.error = this.getErrorMessage(error, 'Could not create invoice.');
      }
    });
  }

  startEmail(invoice: Invoice): void {
    this.emailingInvoiceId = invoice.invoiceId;
    this.recipientEmail = '';
    this.error = '';
  }

  sendInvoice(): void {
    const email = this.recipientEmail.trim();
    if (!this.emailingInvoiceId || !email) {
      return;
    }

    this.sendingEmail = true;
    this.error = '';
    this.invoicesApi.email(this.emailingInvoiceId, email).subscribe({
      next: () => {
        const invoice = this.invoices.find((item) => item.invoiceId === this.emailingInvoiceId);
        this.sendingEmail = false;
        this.emailingInvoiceId = null;
        this.message = `Invoice ${invoice?.reference ?? ''} was emailed.`;
      },
      error: (error: unknown) => {
        this.sendingEmail = false;
        this.error = this.getErrorMessage(error, 'Could not email invoice.');
      }
    });
  }

  cancelEmail(): void {
    this.emailingInvoiceId = null;
    this.recipientEmail = '';
  }

  loadInvoices(): void {
    this.loading = true;
    this.error = '';
    this.invoicesApi.getInvoices(this.pageNumber, this.pageSize, this.searchTerm).subscribe({
      next: (result) => {
        this.invoices = result.items ?? [];
        this.totalCount = result.totalCount;
        this.totalPages = result.totalPages;
        this.loading = false;
      },
      error: (error: unknown) => {
        this.error = this.getErrorMessage(error, 'Could not load invoices.');
        this.loading = false;
      }
    });
  }

  searchInvoices(): void {
    this.pageNumber = 1;
    this.loadInvoices();
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.searchInvoices();
  }

  statusName(status: number): string {
    return this.statuses.find((item) => item.value === status)?.label ?? 'Unknown';
  }

  statusClass(status: number): string {
    return this.statusName(status).toLowerCase().replaceAll(' ', '-');
  }

  updateStatus(invoice: Invoice, status: number): void {
    if (status === invoice.invoiceStatusId) {
      return;
    }

    this.updatingStatusId = invoice.invoiceId;
    this.error = '';
    this.invoicesApi.updateStatus(invoice.invoiceId, status).subscribe({
      next: () => {
        this.updatingStatusId = null;
        this.message = `Invoice ${invoice.reference} status updated.`;
        this.loadInvoices();
      },
      error: (error: unknown) => {
        this.updatingStatusId = null;
        this.error = this.getErrorMessage(error, 'Could not update invoice status.');
      }
    });
  }

  deactivate(invoice: Invoice): void {
    if (!window.confirm(`Deactivate invoice ${invoice.reference}?`)) {
      return;
    }

    this.deactivatingId = invoice.invoiceId;
    this.error = '';
    this.invoicesApi.deactivate(invoice.invoiceId).subscribe({
      next: () => {
        this.deactivatingId = null;
        this.message = `Invoice ${invoice.reference} was deactivated.`;
        this.pageNumber = Math.min(
          this.pageNumber,
          Math.max(1, Math.ceil((this.totalCount - 1) / this.pageSize))
        );
        this.loadInvoices();
      },
      error: (error: unknown) => {
        this.deactivatingId = null;
        this.error = this.getErrorMessage(error, 'Could not deactivate invoice.');
      }
    });
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadInvoices();
    }
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadInvoices();
    }
  }

  logout(): void {
    this.auth.logout();
    void this.router.navigate(['/login']);
  }

  private getErrorMessage(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message || error.error?.title || fallback;
    }

    return fallback;
  }

  private newLineItem(): { reference: string; amount: number } {
    return { reference: '', amount: 0 };
  }

  private resetInvoiceForm(): void {
    this.selectedCustomer = null;
    this.invoiceForm.reset({
      personId: 0,
      reference: `INV-${Date.now()}`,
      invoiceDate: new Date().toISOString().slice(0, 10),
      invoiceDueDate: new Date(Date.now() + 30 * 86400000).toISOString().slice(0, 10)
    });
    this.lineItems = [this.newLineItem()];
    this.lineItemsError = '';
  }
}
