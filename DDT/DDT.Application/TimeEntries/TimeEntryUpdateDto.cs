using System;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.TimeEntries
{
    public class TimeEntryUpdateDto
    {
        public TimeEntryUpdateDto()
        {
        }

        public Guid Id { get; set; }
        public DateTimeOffset EntryDateTime { get; set; }
        public DateTimeOffset Date { get; set; }
        public Guid UserId { get; set; }
        public Guid EntryTypeId { get; set; }

        public static TimeEntryUpdateDto Create(
            Guid id,
            DateTimeOffset entryDateTime,
            DateTimeOffset date,
            Guid userId,
            Guid entryTypeId)
        {
            return new TimeEntryUpdateDto
            {
                Id = id,
                EntryDateTime = entryDateTime,
                Date = date,
                UserId = userId,
                EntryTypeId = entryTypeId
            };
        }
    }
}