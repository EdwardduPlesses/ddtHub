using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DDT.Domain.Entities;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.MappingExtensions", Version = "1.0")]

namespace DDT.Application.EntryTypes
{
    public static class EntryTypeDtoMappingExtensions
    {
        public static EntryTypeDto MapToEntryTypeDto(this EntryType projectFrom, IMapper mapper)
            => mapper.Map<EntryTypeDto>(projectFrom);

        public static List<EntryTypeDto> MapToEntryTypeDtoList(this IEnumerable<EntryType> projectFrom, IMapper mapper)
            => projectFrom.Select(x => x.MapToEntryTypeDto(mapper)).ToList();
    }
}