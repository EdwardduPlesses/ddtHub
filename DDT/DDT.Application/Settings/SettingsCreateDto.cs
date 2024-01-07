using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.Settings
{
    public class SettingsCreateDto
    {
        public SettingsCreateDto()
        {
            Property = null!;
            Value = null!;
        }

        public string Property { get; set; }
        public string Value { get; set; }

        public static SettingsCreateDto Create(string property, string value)
        {
            return new SettingsCreateDto
            {
                Property = property,
                Value = value
            };
        }
    }
}