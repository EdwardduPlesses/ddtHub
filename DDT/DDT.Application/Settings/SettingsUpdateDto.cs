using System;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.Settings
{
    public class SettingsUpdateDto
    {
        public SettingsUpdateDto()
        {
            Property = null!;
            Value = null!;
        }

        public Guid Id { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }

        public static SettingsUpdateDto Create(Guid id, string property, string value)
        {
            return new SettingsUpdateDto
            {
                Id = id,
                Property = property,
                Value = value
            };
        }
    }
}