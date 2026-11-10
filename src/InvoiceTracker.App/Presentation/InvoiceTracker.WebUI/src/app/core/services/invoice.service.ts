import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateInvoiceRequest, Invoice, PagedResult } from '../models/models';

@Injectable({ providedIn: 'root' })
export class InvoiceService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = `${environment.apiUrl}/Invoice`;

  getInvoices(pageNumber = 1, pageSize = 10, searchTerm = ''): Observable<PagedResult<Invoice>> {
    let params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);
    if (searchTerm.trim()) {
      params = params.set('searchTerm', searchTerm.trim());
    }
    return this.http.get<PagedResult<Invoice>>(this.endpoint, { params });
  }
  create(request: CreateInvoiceRequest): Observable<Invoice> { return this.http.post<Invoice>(this.endpoint, request); }
  getById(invoiceId: number): Observable<Invoice> { return this.http.get<Invoice>(`${this.endpoint}/GetInvoice/${invoiceId}`); }
  updateStatus(invoiceId: number, status: number): Observable<boolean> {
    return this.http.put<boolean>(`${this.endpoint}/UpdateInvoiceStatus/${invoiceId}/${status}`, {});
  }
  deactivate(invoiceId: number): Observable<void> {
    return this.http.delete<void>(`${this.endpoint}/${invoiceId}`);
  }
  email(invoiceId: number, recipientEmail: string): Observable<void> {
    return this.http.post<void>(`${this.endpoint}/${invoiceId}/email`, { recipientEmail });
  }
}
