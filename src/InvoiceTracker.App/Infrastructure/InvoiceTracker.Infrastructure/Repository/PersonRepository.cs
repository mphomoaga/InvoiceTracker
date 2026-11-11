using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Domain.Models;
using InvoiceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceTracker.Infrastructure.Repository;

public class PersonRepository(InvoiceTrackerDbContext context) : IPersonRepository
{
    public Task<Person> GetClientById(int personId, CancellationToken cancellationToken)
    {
        return context.Persons
            .AsNoTracking()
            .FirstOrDefaultAsync(
                person => person.PersonId == personId && person.IsActive,
                cancellationToken);
    }

    public async Task<Person> AddNewClient(Person person, CancellationToken cancellationToken)
    {
        await context.Persons.AddAsync(person, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return person;
    }

    public async Task<PagedResult<Person>> GetPageClient(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken)
    {
        var people = context.Persons
            .AsNoTracking()
            .Where(person => person.IsActive);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.Trim();
            people = people.Where(person =>
                person.FullNames.Contains(search)
                || person.Surname.Contains(search)
                || (person.Title != null && person.Title.Contains(search))
                || (person.Intials != null && person.Intials.Contains(search))
                || (person.IdNumber != null && person.IdNumber.Contains(search))
                || (person.PassportNumber != null && person.PassportNumber.Contains(search)));
        }

        var totalCount = await people.CountAsync(cancellationToken);
        var items = await people
            .OrderBy(person => person.PersonId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Person>(items, pageNumber, pageSize, totalCount);
    }

    public async Task<bool> DeactivateClient(
        int personId,
        string modifiedBy,
        CancellationToken cancellationToken)
    {
        var person = await context.Persons
            .FirstOrDefaultAsync(
                item => item.PersonId == personId && item.IsActive,
                cancellationToken);

        if (person is null)
        {
            return false;
        }

        person.IsActive = false;
        person.ModifiedBy = modifiedBy;
        person.ModifiedDate = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
