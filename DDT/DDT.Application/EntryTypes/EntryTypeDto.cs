using System;
using AutoMapper;
using DDT.Application.Common.Mappings;
using DDT.Domain;
using DDT.Domain.Entities;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.EntryTypes
{
    public class EntryTypeDto : IMapFrom<EntryType>
    {
        public EntryTypeDto()
        {
            Description = null!;
        }

        public Guid Id { get; set; }
        public EntryTypeEnum Type { get; set; }
        public string Description { get; set; }

        public static EntryTypeDto Create(Guid id, EntryTypeEnum type, string description)
        {
            return new EntryTypeDto
            {
                Id = id,
                Type = type,
                Description = description
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EntryType, EntryTypeDto>();
        }
    }
}