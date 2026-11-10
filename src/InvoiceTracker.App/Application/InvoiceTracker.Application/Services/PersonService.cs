using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Application.Interfaces.Services;
using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Domain.Models;

namespace InvoiceTracker.Application.Services;

public sealed class PersonService(IPersonRepository personRepository) : IPersonManager
{
    public Task<Person?> GetClientById(int personId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(personId);
        return personRepository.GetClientById(personId, cancellationToken);
    }

    public Task<Person> AddNewClient(Person person, CancellationToken cancellationToken)
    {

        person.PersonId = 0;
        person.IsActive = true;
        person.CreatedDate = DateTime.UtcNow;
        person.ModifiedBy = null;
        person.ModifiedDate = null;

        return personRepository.AddNewClient(person, cancellationToken);
    }

    public Task<PagedResult<Person>> GetPageClient(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageNumber);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

        return personRepository.GetPageClient(pageNumber, pageSize, searchTerm, cancellationToken);
    }

    public Task<bool> DeactivateClient(
        int personId,
        string modifiedBy,
        CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(personId);
        ArgumentException.ThrowIfNullOrWhiteSpace(modifiedBy);

        return personRepository.DeactivateClient(personId, modifiedBy, cancellationToken);
    }
}
