import { Component } from '@angular/core';
import { NotificationService } from '../../core/services/notification.service';

interface InvoiceStat {
  label: string;
  value: string;
  icon: string;
}

interface InvoiceFeature {
  name: string;
  title: string;
  description: string;
  tag?: string;
  icon: string;
  actionLabel: string;
}

interface RecentInvoice {
  invoiceNumber: string;
  customer: string;
  amount: string;
  status: 'paid' | 'pending' | 'cancelled';
}

interface QuickAction {
  label: string;
  icon: string;
  action: string;
}

@Component({
  selector: 'app-invoice-management',
  standalone: false,
  templateUrl: './invoice-management.component.html',
  styleUrl: './invoice-management.component.scss',
})
export class InvoiceManagementComponent {
  searchTerm = '';

  stats: InvoiceStat[] = [
    { label: 'Total invoices', value: '1,248', icon: 'document' },
    { label: 'Paid', value: 'R 428k', icon: 'check' },
    { label: 'Outstanding', value: 'R 79k', icon: 'clock' },
    { label: 'Overdue', value: '23', icon: 'warning' },
  ];

  features: InvoiceFeature[] = [
    {
      name: 'create invoice',
      title: 'Create Invoice',
      description: 'Build a new customer invoice with line items, tax, discounts, and payment terms.',
      tag: 'Primary',
      icon: '＋',
      actionLabel: 'Start creating',
    },
    {
      name: 'edit invoice',
      title: 'Edit Invoice',
      description: 'Update customer details, invoice items, due dates, and payment information.',
      icon: '✎',
      actionLabel: 'Open editor',
    },
    {
      name: 'cancel invoice',
      title: 'Cancel Invoice',
      description: 'Cancel an invoice safely while retaining an auditable record for reporting.',
      icon: '×',
      actionLabel: 'Review invoices',
    },
    {
      name: 'duplicate invoice copy',
      title: 'Duplicate Invoice',
      description: 'Copy an existing invoice to speed up recurring or similar billing.',
      icon: '⧉',
      actionLabel: 'Select invoice',
    },
    {
      name: 'generate pdf download',
      title: 'Generate PDF',
      description: 'Create a polished PDF version for download, printing, or secure storage.',
      icon: '⇩',
      actionLabel: 'Generate document',
    },
    {
      name: 'email invoice send',
      title: 'Email Invoice',
      description: 'Send an invoice to a customer with a clear message and PDF attachment.',
      icon: '✉',
      actionLabel: 'Compose email',
    },
  ];

  recentInvoices: RecentInvoice[] = [
    { invoiceNumber: '#INV-1048', customer: 'Ubuntu Retail', amount: 'R 18,450.00', status: 'paid' },
    { invoiceNumber: '#INV-1047', customer: 'Northstar Studio', amount: 'R 9,780.00', status: 'pending' },
    { invoiceNumber: '#INV-1046', customer: 'Clearview Consulting', amount: 'R 27,200.00', status: 'cancelled' },
  ];

  quickActions: QuickAction[] = [
    { label: 'Add customer', action: 'Add customer', icon: '＋' },
    { label: 'Record payment', action: 'Record payment', icon: 'R' },
    { label: 'Export report', action: 'Export report', icon: '⇩' },
  ];

  constructor(private notificationService: NotificationService) {}

  get filteredFeatures(): InvoiceFeature[] {
    const query = this.searchTerm.trim().toLowerCase();

    if (!query) {
      return this.features;
    }

    return this.features.filter((feature) => {
      return (
        feature.name.toLowerCase().includes(query) ||
        feature.title.toLowerCase().includes(query) ||
        feature.description.toLowerCase().includes(query)
      );
    });
  }

  trackFeature(index: number, feature: InvoiceFeature): string {
    return feature.name;
  }

  handleAction(action: string): void {
    this.notificationService.notify(action);
  }
}
