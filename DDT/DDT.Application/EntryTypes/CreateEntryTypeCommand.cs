using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DDT.Application.Common.Interfaces;
using DDT.Domain;
using DDT.Domain.Entities;
using DDT.Domain.Repositories;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace DDT.Application.EntryTypes
{
    public class CreateEntryTypeCommand : IRequest<Guid>, ICommand
    {
        public CreateEntryTypeCommand(EntryTypeEnum type, string description)
        {
            Type = type;
            Description = description;
        }

        public EntryTypeEnum Type { get; set; }
        public string Description { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateEntryTypeCommandHandler : IRequestHandler<CreateEntryTypeCommand, Guid>
    {
        private readonly IEntryTypeRepository _entryTypeRepository;

        [IntentManaged(Mode.Merge)]
        public CreateEntryTypeCommandHandler(IEntryTypeRepository entryTypeRepository)
        {
            _entryTypeRepository = entryTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateEntryTypeCommand request, CancellationToken cancellationToken)
        {
            var newEntryType = new EntryType
            {
                Type = request.Type,
                Description = request.Description,
            };

            _entryTypeRepository.Add(newEntryType);
            await _entryTypeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return newEntryType.Id;
        }
    }

    public class CreateEntryTypeCommandValidator : AbstractValidator<CreateEntryTypeCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateEntryTypeCommandValidator()
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