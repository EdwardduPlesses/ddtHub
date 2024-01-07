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

namespace DDT.Application.TimeEntries
{
    public class DeleteTimeEntryCommand : IRequest, ICommand
    {
        public DeleteTimeEntryCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class DeleteTimeEntryCommandHandler : IRequestHandler<DeleteTimeEntryCommand>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        [IntentManaged(Mode.Merge)]
        public DeleteTimeEntryCommandHandler(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(DeleteTimeEntryCommand request, CancellationToken cancellationToken)
        {
            var existingTimeEntry = await _timeEntryRepository.FindByIdAsync(request.Id, cancellationToken);
            if (existingTimeEntry is null)
            {
                throw new NotFoundException($"Could not find TimeEntry '{request.Id}'");
            }

            _timeEntryRepository.Remove(existingTimeEntry);
        }
    }

    public class DeleteTimeEntryCommandValidator : AbstractValidator<DeleteTimeEntryCommand>
    {
        [IntentManaged(Mode.Merge)]
        public DeleteTimeEntryCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}