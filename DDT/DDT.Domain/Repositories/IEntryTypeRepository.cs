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
    public interface IEntryTypeRepository : IEFRepository<EntryType, EntryType>
    {
        [IntentManaged(Mode.Fully)]
        Task<EntryType?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
        [IntentManaged(Mode.Fully)]
        Task<List<EntryType>> FindByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Seed Entry Type Data
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SeedEntryTypeStatusAsync(CancellationToken cancellationToken);
    }
}