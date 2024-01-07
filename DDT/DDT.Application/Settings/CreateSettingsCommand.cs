using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DDT.Application.Common.Interfaces;
using DDT.Domain.Entities;
using DDT.Domain.Repositories;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace DDT.Application.Settings
{
    public class CreateSettingsCommand : IRequest<Guid>, ICommand
    {
        public CreateSettingsCommand(string property, string value)
        {
            Property = property;
            Value = value;
        }

        public string Property { get; set; }
        public string Value { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateSettingsCommandHandler : IRequestHandler<CreateSettingsCommand, Guid>
    {
        private readonly ISettingsRepository _settingsRepository;

        [IntentManaged(Mode.Merge)]
        public CreateSettingsCommandHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateSettingsCommand request, CancellationToken cancellationToken)
        {
            var newSettings = new Domain.Entities.Settings
            {
                Property = request.Property,
                Value = request.Value,
            };

            _settingsRepository.Add(newSettings);
            await _settingsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return newSettings.Id;
        }
    }

    public class CreateSettingsCommandValidator : AbstractValidator<CreateSettingsCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateSettingsCommandValidator()
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