using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.FluentValidation.Dtos.DTOValidator", Version = "2.0")]

namespace DDT.Application.Settings
{
    public class SettingsDtoValidator : AbstractValidator<SettingsDto>
    {
        [IntentManaged(Mode.Merge)]
        public SettingsDtoValidator()
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