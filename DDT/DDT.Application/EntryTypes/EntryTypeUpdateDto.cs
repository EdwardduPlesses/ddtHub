using System;
using DDT.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.EntryTypes
{
    public class EntryTypeUpdateDto
    {
        public EntryTypeUpdateDto()
        {
            Description = null!;
        }

        public Guid Id { get; set; }
        public EntryTypeEnum Type { get; set; }
        public string Description { get; set; }

        public static EntryTypeUpdateDto Create(Guid id, EntryTypeEnum type, string description)
        {
            return new EntryTypeUpdateDto
            {
                Id = id,
                Type = type,
                Description = description
            };
        }
    }
}