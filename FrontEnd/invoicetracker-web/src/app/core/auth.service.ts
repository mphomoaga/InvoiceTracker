import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';

export interface LoginRequest {
  username: string;
  email: string;
  password: string;
  authType: string;
}

export interface LoginResponse {
  accessToken: string;
  expiresAt: string;
  roles: Array<{ roleName?: string; RoleName?: string }>;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly tokenKey = 'invoiceTracker.accessToken';
  private readonly expiryKey = 'invoiceTracker.tokenExpiry';

  login(email: string, password: string): Observable<LoginResponse> {
    const request: LoginRequest = {
      username: '',
      email,
      password,
      authType: 'Email'
    };

    return this.http.post<LoginResponse>(`${environment.apiUrl}/Account/login`, request).pipe(
      tap(response => {
        localStorage.setItem(this.tokenKey, response.accessToken);
        localStorage.setItem(this.expiryKey, response.expiresAt);
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.expiryKey);
  }

  token(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    const token = this.token();
    const expiry = localStorage.getItem(this.expiryKey);
    return Boolean(token && (!expiry || new Date(expiry).getTime() > Date.now()));
  }
}
