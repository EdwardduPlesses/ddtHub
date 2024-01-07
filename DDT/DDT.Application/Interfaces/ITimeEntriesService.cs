using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DDT.Application.TimeEntries;
using DDT.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Contracts.ServiceContract", Version = "1.0")]

namespace DDT.Application.Interfaces
{
    [IntentManaged(Mode.Merge)]
    public interface ITimeEntriesService : IDisposable
    {
        Task<Guid> CreateTimeEntry(TimeEntryCreateDto dto, CancellationToken cancellationToken = default);
        Task<TimeEntryDto> FindTimeEntryById(Guid id, CancellationToken cancellationToken = default);
        Task<List<TimeEntryDto>> FindTimeEntries(CancellationToken cancellationToken = default);
        Task UpdateTimeEntry(Guid id, TimeEntryUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteTimeEntry(Guid id, CancellationToken cancellationToken = default);
        Task Guess(GuessDTO guess, CancellationToken cancellationToken = default);
        Task Actual(ActualDTO actual, CancellationToken cancellationToken = default);

        Task AddTimeEntry(DateTimeOffset date, EntryTypeEnum entryTypeEnum, CancellationToken cancellationToken = default);


    }
}