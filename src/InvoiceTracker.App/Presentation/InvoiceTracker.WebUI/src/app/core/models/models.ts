export interface LoginRequest {
  email: string;
  password: string;
  authType: 'Password';
}

export interface NotificationEmailSummary {
  emailAuditId: number;
  to: string;
  cc?: string | null;
  subject: string;
  itemType: string;
  itemTypeRef: number;
  hasAttachments: boolean;
  isSent: boolean;
  sentDate: string;
}

export interface NotificationEmailDetails extends NotificationEmailSummary {
  body: string;
}

export interface LoginResponse {
  loginSuccess: boolean;
  accessToken: string;
  role: string;
  message: string;
  expiresAt: string;
}

export interface Invoice {
  invoiceId: number;
  personId: number;
  reference: string;
  invoiceDate: string;
  invoiceDueDate: string;
  invoiceStatusId: number;
  invoiceAmount: number;
  createdBy: string;
  personFullName?: string;
  lineItems?: InvoiceLineItem[];
}

export enum InvoiceStatusEnum {
  Draft = 1,
  Pending = 2,
  Sent = 3,
  Viewed = 4,
  PartiallyPaid = 5,
  Paid = 6,
  Overdue = 7,
  Cancelled = 8,
  Refunded = 9,
  WrittenOff = 10
}

export interface InvoiceLineItem { reference: string; amount: number; }

export interface Person {
  personId: number;
  title?: string | null;
  intials?: string | null;
  fullNames: string;
  surname: string;
  idNumber?: string | null;
  passportNumber?: string | null;
  isActive: boolean;
  createdBy: string;
  createdDate: string;
  modifiedBy?: string | null;
  modifiedDate?: string | null;
}

export interface CreatePersonRequest {
  title?: string | null;
  intials?: string | null;
  fullNames: string;
  surname: string;
  idNumber?: string | null;
  passportNumber?: string | null;
  createdBy: string;
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface CreateInvoiceRequest {
  personId: number;
  reference: string;
  invoiceDate: string;
  invoiceDueDate: string;
  invoiceStatusEnum: number;
  invoiceAmount: number;
  createdBy: string;
  lineItems?: InvoiceLineItem[];
}
