import { Routes } from '@angular/router';
import { authGuard } from './core/auth.guard';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./pages/login/login.component').then((m) => m.LoginComponent) },
  { path: '', canActivate: [authGuard], loadComponent: () => import('./pages/dashboard/dashboard.component').then((m) => m.DashboardComponent) },
  { path: 'invoice', canActivate: [authGuard], loadComponent: () => import('./pages/Invoice/invoice.component').then((m) => m.InvoiceComponent) },
  { path: 'customers', canActivate: [authGuard], loadComponent: () => import('./pages/customers/customers.component').then((m) => m.CustomersComponent) },
  { path: 'notifications', canActivate: [authGuard], loadComponent: () => import('./pages/notification/notification.component').then((m) => m.NotificationComponent) },
  { path: '**', redirectTo: '' }
];
