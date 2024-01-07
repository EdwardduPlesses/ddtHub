using System;
using System.Threading;
using System.Threading.Tasks;
using DDT.Application.Common.Interfaces;
using DDT.Domain.Repositories;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace DDT.Application.Seeding
{
    public class SeedDataCommand : IRequest, ICommand
    {
        public SeedDataCommand()
        {
        }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SeedDataCommandHandler : IRequestHandler<SeedDataCommand>
    {
        private readonly IEntryTypeRepository _entryTypeRepository;
        [IntentManaged(Mode.Merge)]
        public SeedDataCommandHandler(IEntryTypeRepository entryTypeRepository)
        {
            _entryTypeRepository = entryTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SeedDataCommand request, CancellationToken cancellationToken)
        {
            await _entryTypeRepository.SeedEntryTypeStatusAsync(cancellationToken);
        }
    }

    public class SeedDataCommandValidator : AbstractValidator<SeedDataCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SeedDataCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}