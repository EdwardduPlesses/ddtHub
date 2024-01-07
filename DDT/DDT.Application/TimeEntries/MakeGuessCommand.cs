using System;
using System.Threading;
using System.Threading.Tasks;
using DDT.Application.Common.Interfaces;
using DDT.Application.Interfaces;
using DDT.Domain;
using DDT.Domain.Common.Exceptions;
using DDT.Domain.Entities;
using DDT.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.AspNetCore.Identity;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace DDT.Application.TimeEntries
{
    public class MakeGuessCommand : IRequest, ICommand
    {
        public MakeGuessCommand(GuessDTO guess)
        {
            Guess = guess;
        }

        public GuessDTO Guess { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class MakeGuessCommandHandler : IRequestHandler<MakeGuessCommand>
    {
        private readonly IEntryTypeRepository _entryTypeRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly ITimeEntriesService _timeEntriesService;

        [IntentManaged(Mode.Merge)]
        public MakeGuessCommandHandler(IEntryTypeRepository entryTypeRepository,
                                       ICurrentUserService currentUserService,
                                       UserManager<ApplicationIdentityUser> userManager,
                                       ITimeEntryRepository timeEntryRepository,
                                       ITimeEntriesService timeEntriesService)
        {
            _entryTypeRepository = entryTypeRepository;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _timeEntryRepository = timeEntryRepository;
            _timeEntriesService = timeEntriesService;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(MakeGuessCommand request, CancellationToken cancellationToken)
        {
            await _timeEntriesService.AddTimeEntry(request.Guess.GuessDateTime, EntryTypeEnum.Guess, cancellationToken);
        }

        public class MakeGuessCommandValidator : AbstractValidator<MakeGuessCommand>
        {
            [IntentManaged(Mode.Merge)]
            public MakeGuessCommandValidator()
            {
                ConfigureValidationRules();
            }

            private void ConfigureValidationRules()
            {
                RuleFor(v => v.Guess)
                    .NotNull();
            }
        }
    }

    public class MakeGuessCommandValidator : AbstractValidator<MakeGuessCommand>
    {
        [IntentManaged(Mode.Merge)]
        public MakeGuessCommandValidator()
        {
            //IntentMatch("ConfigureValidationRules")
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(v => v.Guess)
                .NotNull();
        }
    }
}