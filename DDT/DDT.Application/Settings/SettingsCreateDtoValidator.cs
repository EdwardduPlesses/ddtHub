using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.FluentValidation.Dtos.DTOValidator", Version = "2.0")]

namespace DDT.Application.Settings
{
    public class SettingsCreateDtoValidator : AbstractValidator<SettingsCreateDto>
    {
        [IntentManaged(Mode.Merge)]
        public SettingsCreateDtoValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Property)
                .NotNull();

            RuleFor(v => v.Value)
                .NotNull();
        }
    }
}