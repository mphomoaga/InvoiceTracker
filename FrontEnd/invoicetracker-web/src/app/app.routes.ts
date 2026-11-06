import { Routes } from '@angular/router';
import { authGuard } from './core/auth.guard';
import { LoginComponent } from './features/auth/login.component';
import { InvoiceManagementComponent } from './features/invoices/invoice-management.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'invoices', component: InvoiceManagementComponent, canActivate: [authGuard] },
  { path: '', pathMatch: 'full', redirectTo: 'invoices' },
  { path: '**', redirectTo: 'invoices' }
];
