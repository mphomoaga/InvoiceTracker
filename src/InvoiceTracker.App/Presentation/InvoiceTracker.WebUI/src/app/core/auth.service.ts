import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { LoginRequest, LoginResponse } from './models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly tokenKey = 'invoiceTracker.accessToken';
  private readonly userKey = 'invoiceTracker.user';

  login(email: string, password: string): Observable<LoginResponse> {
    const request: LoginRequest = { email, password, authType: 'Password' };
    return this.http.post<LoginResponse>(`${environment.apiUrl}/Auth/login`, request).pipe(
      tap((response) => {
        if (response.loginSuccess && response.accessToken) {
          localStorage.setItem(this.tokenKey, response.accessToken);
          localStorage.setItem(this.userKey, JSON.stringify({ email, role: response.role }));
        }
      })
    );
  }

  token(): string | null { return localStorage.getItem(this.tokenKey); }
  isAuthenticated(): boolean { return !!this.token(); }
  user(): { email: string; role: string } | null {
    const value = localStorage.getItem(this.userKey);
    return value ? JSON.parse(value) as { email: string; role: string } : null;
  }
  logout(): void { localStorage.removeItem(this.tokenKey); localStorage.removeItem(this.userKey); }
}
