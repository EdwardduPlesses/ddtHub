using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DDT.Application.EntryTypes;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Contracts.ServiceContract", Version = "1.0")]

namespace DDT.Application.Interfaces
{
    public interface IEntryTypesService : IDisposable
    {
        Task<Guid> CreateEntryType(EntryTypeCreateDto dto, CancellationToken cancellationToken = default);
        Task<EntryTypeDto> FindEntryTypeById(Guid id, CancellationToken cancellationToken = default);
        Task<List<EntryTypeDto>> FindEntryTypes(CancellationToken cancellationToken = default);
        Task UpdateEntryType(Guid id, EntryTypeUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteEntryType(Guid id, CancellationToken cancellationToken = default);

    }
}