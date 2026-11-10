import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { NotificationEmailDetails, NotificationEmailSummary } from '../../core/models/models';
import { NotificationService } from '../../core/services/notification.service';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.css'
})
export class NotificationComponent implements OnInit {
  private readonly notificationsApi = inject(NotificationService);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  readonly user = this.auth.user();
  emails: NotificationEmailSummary[] = [];
  searchTerm = '';
  pageNumber = 1;
  readonly pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  loading = false;
  resendingId: number | null = null;
  message = '';
  error = '';

  dialogOpen = false;
  dialogLoading = false;
  dialogSubject = '';
  dialogError = '';
  selectedEmail: NotificationEmailDetails | null = null;

  ngOnInit(): void {
    this.loadEmails();
  }

  loadEmails(): void {
    this.loading = true;
    this.error = '';
    this.notificationsApi.getPage(this.pageNumber, this.pageSize, this.searchTerm).subscribe({
      next: (result) => {
        this.emails = result.items ?? [];
        this.totalCount = result.totalCount;
        this.totalPages = result.totalPages;
        this.loading = false;
      },
      error: (error: unknown) => {
        this.error = this.getErrorMessage(error, 'Could not load notifications.');
        this.loading = false;
      }
    });
  }

  searchEmails(): void {
    this.pageNumber = 1;
    this.loadEmails();
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.searchEmails();
  }

  viewEmail(email: NotificationEmailSummary): void {
    this.dialogOpen = true;
    this.dialogLoading = true;
    this.dialogSubject = email.subject;
    this.dialogError = '';
    this.selectedEmail = null;
    this.notificationsApi.getById(email.emailAuditId).subscribe({
      next: (details) => {
        this.selectedEmail = details;
        this.dialogLoading = false;
      },
      error: (error: unknown) => {
        this.dialogError = this.getErrorMessage(error, 'Could not load email details.');
        this.dialogLoading = false;
      }
    });
  }

  closeDialog(): void {
    this.dialogOpen = false;
    this.selectedEmail = null;
    this.dialogError = '';
  }

  resend(email: NotificationEmailSummary): void {
    const attachmentNotice = email.hasAttachments
      ? ' The invoice attachment will be regenerated from its invoice record.'
      : '';
    if (!window.confirm(`Resend “${email.subject}” to ${email.to}?${attachmentNotice}`)) {
      return;
    }

    this.resendingId = email.emailAuditId;
    this.error = '';
    this.notificationsApi.resend(email.emailAuditId).subscribe({
      next: () => {
        this.resendingId = null;
        this.message = `Resend request processed for ${email.to}.`;
        this.loadEmails();
      },
      error: (error: unknown) => {
        this.resendingId = null;
        this.error = this.getErrorMessage(error, 'Could not resend email.');
      }
    });
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadEmails();
    }
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadEmails();
    }
  }

  sentStatusClass(isSent: boolean): string {
    return isSent ? 'sent-status' : 'failed-status';
  }

  logout(): void {
    this.auth.logout();
    void this.router.navigate(['/login']);
  }

  private getErrorMessage(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message || error.error?.title || fallback;
    }

    return fallback;
  }
}
