import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { NotificationEmailDetails, NotificationEmailSummary, PagedResult } from '../models/models';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = `${environment.apiUrl}/Notification`;

  getPage(pageNumber = 1, pageSize = 10, searchTerm = ''): Observable<PagedResult<NotificationEmailSummary>> {
    let params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);
    if (searchTerm.trim()) {
      params = params.set('searchTerm', searchTerm.trim());
    }

    return this.http.get<PagedResult<NotificationEmailSummary>>(`${this.endpoint}/page`, { params });
  }

  getById(emailAuditId: number): Observable<NotificationEmailDetails> {
    return this.http.get<NotificationEmailDetails>(`${this.endpoint}/${emailAuditId}`);
  }

  resend(emailAuditId: number): Observable<void> {
    return this.http.post<void>(`${this.endpoint}/${emailAuditId}/resend`, {});
  }
}
