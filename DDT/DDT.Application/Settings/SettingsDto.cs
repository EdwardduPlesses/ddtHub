using System;
using AutoMapper;
using DDT.Application.Common.Mappings;
using DDT.Domain.Entities;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.Settings
{
    public class SettingsDto : IMapFrom<Domain.Entities.Settings>
    {
        public SettingsDto()
        {
            Property = null!;
            Value = null!;
        }

        public Guid Id { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }

        public static SettingsDto Create(Guid id, string property, string value)
        {
            return new SettingsDto
            {
                Id = id,
                Property = property,
                Value = value
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Settings, SettingsDto>();
        }
    }
}