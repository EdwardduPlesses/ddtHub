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

namespace DDT.Application.TimeEntries
{
    public class UpdateTimeEntryCommand : IRequest, ICommand
    {
        public UpdateTimeEntryCommand(Guid id, DateTimeOffset entryDateTime, DateTimeOffset date, Guid userId, Guid entryTypeId)
        {
            Id = id;
            EntryDateTime = entryDateTime;
            Date = date;
            UserId = userId;
            EntryTypeId = entryTypeId;
        }

        public Guid Id { get; set; }
        public DateTimeOffset EntryDateTime { get; set; }
        public DateTimeOffset Date { get; set; }
        public Guid UserId { get; set; }
        public Guid EntryTypeId { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateTimeEntryCommandHandler : IRequestHandler<UpdateTimeEntryCommand>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateTimeEntryCommandHandler(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateTimeEntryCommand request, CancellationToken cancellationToken)
        {
            var existingTimeEntry = await _timeEntryRepository.FindByIdAsync(request.Id, cancellationToken);
            if (existingTimeEntry is null)
            {
                throw new NotFoundException($"Could not find TimeEntry '{request.Id}'");
            }

            existingTimeEntry.EntryDateTime = request.EntryDateTime;
            existingTimeEntry.Date = request.Date;
            existingTimeEntry.UserId = request.UserId;
            existingTimeEntry.EntryTypeId = request.EntryTypeId;
        }
    }

    public class UpdateTimeEntryCommandValidator : AbstractValidator<UpdateTimeEntryCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateTimeEntryCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}