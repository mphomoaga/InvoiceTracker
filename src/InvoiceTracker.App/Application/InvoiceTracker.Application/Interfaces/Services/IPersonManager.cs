using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Domain.Models;

namespace InvoiceTracker.Application.Interfaces.Services;

public interface IPersonManager
{
    Task<Person?> GetClientById(int personId, CancellationToken cancellationToken);

    Task<Person> AddNewClient(Person person, CancellationToken cancellationToken);

    Task<PagedResult<Person>> GetPageClient(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken);

    Task<bool> DeactivateClient(
        int personId,
        string modifiedBy,
        CancellationToken cancellationToken);
}
