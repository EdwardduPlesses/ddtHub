using System;
using System.Threading;
using System.Threading.Tasks;
using DDT.Application.Common.Interfaces;
using DDT.Application.Interfaces;
using DDT.Domain;
using DDT.Domain.Repositories;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace DDT.Application.TimeEntries
{
    public class SetActualCommand : IRequest, ICommand
    {
        public SetActualCommand(ActualDTO actual)
        {
            Actual = actual;
        }

        public ActualDTO Actual { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetActualCommandHandler : IRequestHandler<SetActualCommand>
    {
        private readonly ITimeEntriesService _timeEntriesService;

        [IntentManaged(Mode.Merge)]
        public SetActualCommandHandler(ITimeEntriesService timeEntriesService)
        {
            _timeEntriesService = timeEntriesService;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SetActualCommand request, CancellationToken cancellationToken)
        {
            await _timeEntriesService.AddTimeEntry(request.Actual.ActualDateTime, EntryTypeEnum.Actual, cancellationToken);
        }
    }

    public class SetActualCommandValidator : AbstractValidator<SetActualCommand>
    {
        [IntentManaged(Mode.Merge)]
        public SetActualCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Actual)
                .NotNull();
        }
    }
}