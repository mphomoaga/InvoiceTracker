import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth.service';
import { CreateInvoiceRequest, Invoice } from '../../core/models';
import { InvoiceService } from '../../core/invoice.service';

@Component({
  selector: 'app-dashboard', standalone: true, imports: [CommonModule, FormsModule, ReactiveFormsModule], templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  private readonly invoicesApi = inject(InvoiceService);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  readonly user = this.auth.user();
  readonly invoiceForm = this.fb.nonNullable.group({
    personId: [0, [Validators.required, Validators.min(1)]], reference: ['', Validators.required],
    invoiceDate: [new Date().toISOString().slice(0, 10), Validators.required],
    invoiceDueDate: [new Date(Date.now() + 30 * 86400000).toISOString().slice(0, 10), Validators.required],
    invoiceAmount: [0, [Validators.required, Validators.min(0.01)]]
  });
  invoices: Invoice[] = [];
  filteredInvoices: Invoice[] = [];
  search = '';
  loading = true;
  saving = false;
  message = '';
  error = '';
  showCreate = false;

  ngOnInit(): void { this.loadInvoices(); }
  loadInvoices(): void {
    this.loading = true;
    this.invoicesApi.getInvoices().subscribe({
      next: (result) => { this.invoices = result.items ?? []; this.applySearch(); this.loading = false; },
      error: (error: { error?: { message?: string } }) => { this.error = error.error?.message || 'Could not load invoices.'; this.loading = false; }
    });
  }
  applySearch(): void {
    const query = this.search.toLowerCase().trim();
    this.filteredInvoices = this.invoices.filter((invoice) => `${invoice.reference} ${invoice.personName ?? ''}`.toLowerCase().includes(query));
  }
  statusName(status: number): string {
    return ({ 1: 'Draft', 2: 'Pending', 3: 'Sent', 4: 'Viewed', 5: 'Partially paid', 6: 'Paid', 7: 'Overdue', 8: 'Cancelled' } as Record<number, string>)[status] || 'Unknown';
  }
  statusClass(status: number): string { return this.statusName(status).toLowerCase().replace(' ', '-'); }
  get totalAmount(): number { return this.invoices.reduce((total, invoice) => total + invoice.invoiceAmount, 0); }
  get paidAmount(): number { return this.invoices.filter((invoice) => invoice.invoiceStatusId === 6).reduce((total, invoice) => total + invoice.invoiceAmount, 0); }
  get outstandingAmount(): number { return this.invoices.filter((invoice) => ![6, 8].includes(invoice.invoiceStatusId)).reduce((total, invoice) => total + invoice.invoiceAmount, 0); }
  get overdueCount(): number { return this.invoices.filter((invoice) => invoice.invoiceStatusId === 7).length; }
  createInvoice(): void {
    if (this.invoiceForm.invalid) { this.invoiceForm.markAllAsTouched(); return; }
    this.saving = true; this.error = '';
    const value = this.invoiceForm.getRawValue();
    const request: CreateInvoiceRequest = { ...value, invoiceStatusEnum: 1, createdBy: this.user?.email || 'web-ui' };
    this.invoicesApi.create(request).subscribe({
      next: () => { this.saving = false; this.showCreate = false; this.message = 'Invoice created successfully.'; this.invoiceForm.reset({ personId: 0, reference: '', invoiceDate: new Date().toISOString().slice(0, 10), invoiceDueDate: new Date(Date.now() + 30 * 86400000).toISOString().slice(0, 10), invoiceAmount: 0 }); this.loadInvoices(); },
      error: (error: { error?: { message?: string } }) => { this.saving = false; this.error = error.error?.message || 'Could not create invoice.'; }
    });
  }
  markPaid(invoice: Invoice): void {
    this.invoicesApi.updateStatus(invoice.invoiceId, 6).subscribe({ next: () => this.loadInvoices(), error: () => this.error = 'Could not update invoice status.' });
  }
  logout(): void { this.auth.logout(); void this.router.navigate(['/login']); }
}
