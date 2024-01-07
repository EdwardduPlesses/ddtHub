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
    public interface ISettingsRepository : IEFRepository<Settings, Settings>
    {
        [IntentManaged(Mode.Fully)]
        Task<Settings?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
        [IntentManaged(Mode.Fully)]
        Task<List<Settings>> FindByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default);
    }
}