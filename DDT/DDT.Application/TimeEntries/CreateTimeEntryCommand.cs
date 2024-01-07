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

namespace DDT.Application.TimeEntries
{
    public class CreateTimeEntryCommand : IRequest<Guid>, ICommand
    {
        public CreateTimeEntryCommand(DateTimeOffset entryDateTime, DateTimeOffset date, Guid userId, Guid entryTypeId)
        {
            EntryDateTime = entryDateTime;
            Date = date;
            UserId = userId;
            EntryTypeId = entryTypeId;
        }

        public DateTimeOffset EntryDateTime { get; set; }
        public DateTimeOffset Date { get; set; }
        public Guid UserId { get; set; }
        public Guid EntryTypeId { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateTimeEntryCommandHandler : IRequestHandler<CreateTimeEntryCommand, Guid>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        [IntentManaged(Mode.Merge)]
        public CreateTimeEntryCommandHandler(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateTimeEntryCommand request, CancellationToken cancellationToken)
        {
            var newTimeEntry = new TimeEntry
            {
                EntryDateTime = request.EntryDateTime,
                Date = request.Date,
                UserId = request.UserId,
                EntryTypeId = request.EntryTypeId,
            };

            _timeEntryRepository.Add(newTimeEntry);
            await _timeEntryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return newTimeEntry.Id;
        }
    }

    public class CreateTimeEntryCommandValidator : AbstractValidator<CreateTimeEntryCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateTimeEntryCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}