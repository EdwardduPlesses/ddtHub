using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DDT.Domain.Entities;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.MappingExtensions", Version = "1.0")]

namespace DDT.Application.TimeEntries
{
    public static class TimeEntryDtoMappingExtensions
    {
        public static TimeEntryDto MapToTimeEntryDto(this TimeEntry projectFrom, IMapper mapper)
            => mapper.Map<TimeEntryDto>(projectFrom);

        public static List<TimeEntryDto> MapToTimeEntryDtoList(this IEnumerable<TimeEntry> projectFrom, IMapper mapper)
            => projectFrom.Select(x => x.MapToTimeEntryDto(mapper)).ToList();
    }
}