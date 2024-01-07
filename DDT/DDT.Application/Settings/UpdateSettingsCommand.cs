using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DDT.Application.Common.Interfaces;
using DDT.Domain.Common.Exceptions;
using DDT.Domain.Repositories;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace DDT.Application.Settings
{
    public class UpdateSettingsCommand : IRequest, ICommand
    {
        public UpdateSettingsCommand(Guid id, string property, string value)
        {
            Id = id;
            Property = property;
            Value = value;
        }

        public Guid Id { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateSettingsCommandHandler : IRequestHandler<UpdateSettingsCommand>
    {
        private readonly ISettingsRepository _settingsRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateSettingsCommandHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
        {
            var existingSettings = await _settingsRepository.FindByIdAsync(request.Id, cancellationToken);
            if (existingSettings is null)
            {
                throw new NotFoundException($"Could not find Settings '{request.Id}'");
            }

            existingSettings.Property = request.Property;
            existingSettings.Value = request.Value;
        }
    }

    public class UpdateSettingsCommandValidator : AbstractValidator<UpdateSettingsCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateSettingsCommandValidator()
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