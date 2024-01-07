using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.FluentValidation.Dtos.DTOValidator", Version = "2.0")]

namespace DDT.Application.EntryTypes
{
    public class EntryTypeCreateDtoValidator : AbstractValidator<EntryTypeCreateDto>
    {
        [IntentManaged(Mode.Merge)]
        public EntryTypeCreateDtoValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Type)
                .NotNull()
                .IsInEnum();

            RuleFor(v => v.Description)
                .NotNull()
                .MaximumLength(64);
        }
    }
}