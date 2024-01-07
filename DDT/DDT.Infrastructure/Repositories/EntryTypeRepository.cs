using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DDT.Domain;
using DDT.Domain.Entities;
using DDT.Domain.Repositories;
using DDT.Infrastructure.Persistence;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.Repositories.Repository", Version = "1.0")]

namespace DDT.Infrastructure.Repositories
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class EntryTypeRepository : RepositoryBase<EntryType, EntryType, ApplicationDbContext>, IEntryTypeRepository
    {
        public EntryTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<EntryType?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await FindAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<EntryType>> FindByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default)
        {
            return await FindAllAsync(x => ids.Contains(x.Id), cancellationToken);
        }


        public async Task SeedEntryTypeStatusAsync(CancellationToken cancellationToken)
        {

            await AddEntryTypeSeed(new EntryType
            {
                Type = EntryTypeEnum.Actual,
                Description = "The Actual Time of Arrival"
            }, cancellationToken);

            await AddEntryTypeSeed(new EntryType
            {
                Type = EntryTypeEnum.Guess,
                Description = "A Users Guess for the time of arrival"
            }, cancellationToken);
        }

        private async Task AddEntryTypeSeed(EntryType entryType, CancellationToken cancellationToken)
        {
            var existingEntryType = await FindAsync(x => x.Type == entryType.Type, cancellationToken);
            if (existingEntryType is not null)
            {
                existingEntryType.Type = entryType.Type;
                existingEntryType.Description = entryType.Description;
            }
            else
            {
                Add(entryType);
            }
        }
    }
}