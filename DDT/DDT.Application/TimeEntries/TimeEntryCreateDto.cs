using System;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.TimeEntries
{
    public class TimeEntryCreateDto
    {
        public TimeEntryCreateDto()
        {
        }

        public DateTimeOffset EntryDateTime { get; set; }
        public DateTimeOffset Date { get; set; }
        public Guid UserId { get; set; }
        public Guid EntryTypeId { get; set; }

        public static TimeEntryCreateDto Create(
            DateTimeOffset entryDateTime,
            DateTimeOffset date,
            Guid userId,
            Guid entryTypeId)
        {
            return new TimeEntryCreateDto
            {
                EntryDateTime = entryDateTime,
                Date = date,
                UserId = userId,
                EntryTypeId = entryTypeId
            };
        }
    }
}