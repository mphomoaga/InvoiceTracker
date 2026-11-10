import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PagedResult, Person } from '../../core/models/models';
import { PersonService } from '../../core/services/person.service';

@Component({
  selector: 'app-customer-lookup',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './customer-lookup.component.html',
  styleUrl: './customer-lookup.component.css'
})
export class CustomerLookupComponent {
  private readonly peopleApi = inject(PersonService);

  @Output() readonly customerFound = new EventEmitter<Person>();

  searchTerm = '';
  people: Person[] = [];
  pageNumber = 1;
  readonly pageSize = 8;
  totalPages = 0;
  loading = false;
  error = '';

  searchCustomers(): void {
    this.pageNumber = 1;
    this.loadCustomers();
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadCustomers();
    }
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadCustomers();
    }
  }

  selectCustomer(customer: Person): void {
    this.customerFound.emit(customer);
  }

  private loadCustomers(): void {
    this.loading = true;
    this.error = '';
    this.peopleApi.getPage(this.pageNumber, this.pageSize, this.searchTerm).subscribe({
      next: (result: PagedResult<Person>) => {
        this.people = result.items ?? [];
        this.totalPages = result.totalPages;
        this.loading = false;
      },
      error: () => {
        this.error = 'Could not search customers.';
        this.loading = false;
      }
    });
  }
}
