using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DDT.Domain.Entities;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.MappingExtensions", Version = "1.0")]

namespace DDT.Application.Settings
{
    public static class SettingsDtoMappingExtensions
    {
        public static SettingsDto MapToSettingsDto(this Domain.Entities.Settings projectFrom, IMapper mapper)
            => mapper.Map<SettingsDto>(projectFrom);

        public static List<SettingsDto> MapToSettingsDtoList(this IEnumerable<Domain.Entities.Settings> projectFrom, IMapper mapper)
            => projectFrom.Select(x => x.MapToSettingsDto(mapper)).ToList();
    }
}