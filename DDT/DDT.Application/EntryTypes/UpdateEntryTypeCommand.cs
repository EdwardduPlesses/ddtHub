using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DDT.Application.Common.Interfaces;
using DDT.Domain;
using DDT.Domain.Common.Exceptions;
using DDT.Domain.Repositories;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace DDT.Application.EntryTypes
{
    public class UpdateEntryTypeCommand : IRequest, ICommand
    {
        public UpdateEntryTypeCommand(Guid id, EntryTypeEnum type, string description)
        {
            Id = id;
            Type = type;
            Description = description;
        }

        public Guid Id { get; set; }
        public EntryTypeEnum Type { get; set; }
        public string Description { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateEntryTypeCommandHandler : IRequestHandler<UpdateEntryTypeCommand>
    {
        private readonly IEntryTypeRepository _entryTypeRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateEntryTypeCommandHandler(IEntryTypeRepository entryTypeRepository)
        {
            _entryTypeRepository = entryTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateEntryTypeCommand request, CancellationToken cancellationToken)
        {
            var existingEntryType = await _entryTypeRepository.FindByIdAsync(request.Id, cancellationToken);
            if (existingEntryType is null)
            {
                throw new NotFoundException($"Could not find EntryType '{request.Id}'");
            }

            existingEntryType.Type = request.Type;
            existingEntryType.Description = request.Description;
        }
    }

    public class UpdateEntryTypeCommandValidator : AbstractValidator<UpdateEntryTypeCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateEntryTypeCommandValidator()
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