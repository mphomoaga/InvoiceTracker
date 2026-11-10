import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { CreatePersonRequest, Person } from '../../core/models/models';
import { PersonService } from '../../core/services/person.service';

@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterLink, RouterLinkActive],
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.css'
})
export class CustomersComponent implements OnInit {
  private readonly peopleApi = inject(PersonService);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly formBuilder = inject(FormBuilder);

  readonly user = this.auth.user();
  readonly personForm = this.formBuilder.nonNullable.group({
    title: [''],
    intials: [''],
    fullNames: ['', Validators.required],
    surname: ['', Validators.required],
    idNumber: [''],
    passportNumber: ['']
  });

  people: Person[] = [];
  searchTerm = '';
  pageNumber = 1;
  readonly pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  loading = false;
  saving = false;
  deactivatingId: number | null = null;
  showCreate = false;
  message = '';
  error = '';

  ngOnInit(): void {
    this.loadPeople();
  }

  loadPeople(): void {
    this.loading = true;
    this.error = '';
    this.peopleApi.getPage(this.pageNumber, this.pageSize, this.searchTerm).subscribe({
      next: (result) => {
        this.people = result.items ?? [];
        this.totalCount = result.totalCount;
        this.totalPages = result.totalPages;
        this.loading = false;
      },
      error: (error: unknown) => {
        this.error = this.getErrorMessage(error, 'Could not load customers.');
        this.loading = false;
      }
    });
  }

  searchPeople(): void {
    this.pageNumber = 1;
    this.loadPeople();
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.searchPeople();
  }

  addPerson(): void {
    if (this.personForm.invalid) {
      this.personForm.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.error = '';
    const request: CreatePersonRequest = {
      ...this.personForm.getRawValue(),
      createdBy: this.user?.email || 'web-ui'
    };

    this.peopleApi.create(request).subscribe({
      next: () => {
        this.saving = false;
        this.showCreate = false;
        this.message = 'Customer added successfully.';
        this.personForm.reset();
        this.pageNumber = 1;
        this.loadPeople();
      },
      error: (error: unknown) => {
        this.saving = false;
        this.error = this.getErrorMessage(error, 'Could not add customer.');
      }
    });
  }

  deactivate(person: Person): void {
    const name = `${person.fullNames} ${person.surname}`.trim();
    if (!window.confirm(`Deactivate ${name}?`)) {
      return;
    }

    this.deactivatingId = person.personId;
    this.error = '';
    this.peopleApi.deactivate(person.personId).subscribe({
      next: () => {
        this.deactivatingId = null;
        this.message = `${name} was deactivated.`;
        const remainingCount = Math.max(0, this.totalCount - 1);
        this.pageNumber = Math.min(
          this.pageNumber,
          Math.max(1, Math.ceil(remainingCount / this.pageSize))
        );
        this.loadPeople();
      },
      error: (error: unknown) => {
        this.deactivatingId = null;
        this.error = this.getErrorMessage(error, 'Could not deactivate customer.');
      }
    });
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadPeople();
    }
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadPeople();
    }
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
