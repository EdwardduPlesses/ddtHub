using System;
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
    public class DeleteSettingsCommand : IRequest, ICommand
    {
        public DeleteSettingsCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class DeleteSettingsCommandHandler : IRequestHandler<DeleteSettingsCommand>
    {
        private readonly ISettingsRepository _settingsRepository;

        [IntentManaged(Mode.Merge)]
        public DeleteSettingsCommandHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(DeleteSettingsCommand request, CancellationToken cancellationToken)
        {
            var existingSettings = await _settingsRepository.FindByIdAsync(request.Id, cancellationToken);
            if (existingSettings is null)
            {
                throw new NotFoundException($"Could not find Settings '{request.Id}'");
            }

            _settingsRepository.Remove(existingSettings);
        }
    }

    public class DeleteSettingsCommandValidator : AbstractValidator<DeleteSettingsCommand>
    {
        [IntentManaged(Mode.Merge)]
        public DeleteSettingsCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}