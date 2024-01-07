using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.FluentValidation.Dtos.DTOValidator", Version = "2.0")]

namespace DDT.Application.Leaderboards
{
    public class LeaderboardDataDtoValidator : AbstractValidator<LeaderboardDataDto>
    {
        [IntentManaged(Mode.Merge)]
        public LeaderboardDataDtoValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.User)
                .NotNull();
        }
    }
}