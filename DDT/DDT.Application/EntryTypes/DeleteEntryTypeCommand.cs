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

namespace DDT.Application.EntryTypes
{
    public class DeleteEntryTypeCommand : IRequest, ICommand
    {
        public DeleteEntryTypeCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class DeleteEntryTypeCommandHandler : IRequestHandler<DeleteEntryTypeCommand>
    {
        private readonly IEntryTypeRepository _entryTypeRepository;

        [IntentManaged(Mode.Merge)]
        public DeleteEntryTypeCommandHandler(IEntryTypeRepository entryTypeRepository)
        {
            _entryTypeRepository = entryTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(DeleteEntryTypeCommand request, CancellationToken cancellationToken)
        {
            var existingEntryType = await _entryTypeRepository.FindByIdAsync(request.Id, cancellationToken);
            if (existingEntryType is null)
            {
                throw new NotFoundException($"Could not find EntryType '{request.Id}'");
            }

            _entryTypeRepository.Remove(existingEntryType);
        }
    }

    public class DeleteEntryTypeCommandValidator : AbstractValidator<DeleteEntryTypeCommand>
    {
        [IntentManaged(Mode.Merge)]
        public DeleteEntryTypeCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}