export interface LoginRequest {
  email: string;
  password: string;
  authType: 'Password';
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
  personName?: string;
  lineItems?: InvoiceLineItem[];
}

export interface InvoiceLineItem { reference: string; amount: number; }

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
