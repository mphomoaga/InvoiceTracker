import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/auth.service';
import { CreateInvoiceRequest, Invoice, InvoiceService } from '../../core/invoice.service';

@Component({
  selector: 'app-invoice-management',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterLink, CurrencyPipe, DatePipe],
  templateUrl: './invoice-management.component.html'
})
export class InvoiceManagementComponent implements OnInit {
  private readonly invoiceService = inject(InvoiceService);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly formBuilder = inject(FormBuilder);

  invoices: Invoice[] = [];
  loading = false;
  saving = false;
  errorMessage = '';
  showCreateForm = false;
  searchTerm = '';
  pageNumber = 1;
  readonly pageSize = 10;
  totalCount = 0;
  totalPages = 1;

  readonly form = this.formBuilder.nonNullable.group({
    personId: [0, [Validators.required, Validators.min(1)]],
    reference: ['', [Validators.required, Validators.maxLength(250)]],
    invoiceDate: [new Date().toISOString().slice(0, 10), Validators.required],
    invoiceDueDate: ['', Validators.required],
    invoiceStatusId: [1, [Validators.required, Validators.min(1)]],
    invoiceAmount: [0, [Validators.required, Validators.min(0.01)]]
  });

  get totalAmount(): number {
    return this.invoices.reduce((total, invoice) => total + invoice.invoiceAmount, 0);
  }

  ngOnInit(): void {
    this.loadInvoices();
  }

  loadInvoices(): void {
    this.loading = true;
    this.errorMessage = '';
    this.invoiceService.getPaged(this.pageNumber, this.pageSize, this.searchTerm).subscribe({
      next: result => {
        this.invoices = result.items;
        this.totalCount = result.totalCount;
        this.totalPages = result.totalPages || 1;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Invoices could not be loaded.';
      }
    });
  }

  search(): void {
    this.pageNumber = 1;
    this.loadInvoices();
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

  toggleCreate(): void {
    this.showCreateForm = !this.showCreateForm;
    this.errorMessage = '';
    if (!this.showCreateForm) {
      this.form.reset({
        personId: 0,
        reference: '',
        invoiceDate: new Date().toISOString().slice(0, 10),
        invoiceDueDate: '',
        invoiceStatusId: 1,
        invoiceAmount: 0
      });
    }
  }

  createInvoice(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;
    const value = this.form.getRawValue();
    const request: CreateInvoiceRequest = {
      ...value,
      invoiceDate: new Date(value.invoiceDate).toISOString(),
      invoiceDueDate: new Date(value.invoiceDueDate).toISOString(),
      createdBy: 'web-user',
      lineItems: []
    };

    this.invoiceService.create(request).subscribe({
      next: () => {
        this.saving = false;
        this.showCreateForm = false;
        this.pageNumber = 1;
        this.loadInvoices();
      },
      error: () => {
        this.saving = false;
        this.errorMessage = 'Invoice could not be created.';
      }
    });
  }

  remove(invoice: Invoice): void {
    if (!confirm(`Delete ${invoice.reference}?`)) {
      return;
    }

    this.invoiceService.delete(invoice.invoiceId, 'web-user').subscribe({
      next: () => this.loadInvoices(),
      error: () => this.errorMessage = 'Invoice could not be deleted.'
    });
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
