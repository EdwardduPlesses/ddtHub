using DDT.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.EntryTypes
{
    public class EntryTypeCreateDto
    {
        public EntryTypeCreateDto()
        {
            Description = null!;
        }

        public EntryTypeEnum Type { get; set; }
        public string Description { get; set; }

        public static EntryTypeCreateDto Create(EntryTypeEnum type, string description)
        {
            return new EntryTypeCreateDto
            {
                Type = type,
                Description = description
            };
        }
    }
}