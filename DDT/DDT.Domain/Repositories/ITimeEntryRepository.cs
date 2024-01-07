using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DDT.Domain.Entities;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Entities.Repositories.Api.EntityRepositoryInterface", Version = "1.0")]

namespace DDT.Domain.Repositories
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public interface ITimeEntryRepository : IEFRepository<TimeEntry, TimeEntry>
    {
        [IntentManaged(Mode.Fully)]
        Task<TimeEntry?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
        [IntentManaged(Mode.Fully)]
        Task<List<TimeEntry>> FindByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default);
    }
}