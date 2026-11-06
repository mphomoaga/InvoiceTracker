import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface InvoiceLineItem {
  invoiceLineItemId?: number;
  reference: string;
  amount: number;
}

export interface Invoice {
  invoiceId: number;
  personId: number;
  reference: string;
  invoiceDate: string;
  invoiceDueDate: string;
  invoiceStatusId: number;
  invoiceAmount: number;
  status?: string;
  personName?: string;
  lineItems: InvoiceLineItem[];
}

export interface CreateInvoiceRequest {
  personId: number;
  reference: string;
  invoiceDate: string;
  invoiceDueDate: string;
  invoiceStatusId: number;
  invoiceAmount: number;
  createdBy: string;
  lineItems: InvoiceLineItem[];
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

@Injectable({ providedIn: 'root' })
export class InvoiceService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = `${environment.apiUrl}/Invoice`;

  getPaged(pageNumber: number, pageSize: number, search = ''): Observable<PagedResult<Invoice>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    return this.http.get<PagedResult<Invoice>>(this.endpoint, { params });
  }

  create(request: CreateInvoiceRequest): Observable<Invoice> {
    return this.http.post<Invoice>(this.endpoint, request);
  }

  delete(invoiceId: number, modifiedBy: string): Observable<void> {
    return this.http.delete<void>(`${this.endpoint}/${invoiceId}`, {
      params: new HttpParams().set('modifiedBy', modifiedBy)
    });
  }
}
