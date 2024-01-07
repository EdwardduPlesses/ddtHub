using System;
using AutoMapper;
using DDT.Application.Common.Mappings;
using DDT.Domain.Entities;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.TimeEntries
{
    public class TimeEntryDto : IMapFrom<TimeEntry>
    {
        public TimeEntryDto()
        {
        }

        public Guid Id { get; set; }
        public DateTimeOffset EntryDateTime { get; set; }
        public DateTimeOffset Date { get; set; }
        public Guid UserId { get; set; }
        public Guid EntryTypeId { get; set; }

        public static TimeEntryDto Create(
            Guid id,
            DateTimeOffset entryDateTime,
            DateTimeOffset date,
            Guid userId,
            Guid entryTypeId)
        {
            return new TimeEntryDto
            {
                Id = id,
                EntryDateTime = entryDateTime,
                Date = date,
                UserId = userId,
                EntryTypeId = entryTypeId
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TimeEntry, TimeEntryDto>();
        }
    }
}