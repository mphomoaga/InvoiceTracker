import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreatePersonRequest, PagedResult, Person } from '../models/models';

@Injectable({ providedIn: 'root' })
export class PersonService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = `${environment.apiUrl}/Client`;

  getPage(pageNumber = 1, pageSize = 10, searchTerm = ''): Observable<PagedResult<Person>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    const query = searchTerm.trim();
    if (query) {
      params = params.set('searchTerm', query);
    }

    return this.http.get<PagedResult<Person>>(`${this.endpoint}/page`, { params });
  }

  create(request: CreatePersonRequest): Observable<Person> {
    return this.http.post<Person>(this.endpoint, request);
  }

  deactivate(personId: number): Observable<void> {
    return this.http.delete<void>(`${this.endpoint}/${personId}`);
  }
}
