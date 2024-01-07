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

namespace DDT.Application.Leaderboards
{
    public class DeleteLeaderboardCommand : IRequest, ICommand
    {
        public DeleteLeaderboardCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class DeleteLeaderboardCommandHandler : IRequestHandler<DeleteLeaderboardCommand>
    {
        private readonly ILeaderboardRepository _leaderboardRepository;

        [IntentManaged(Mode.Merge)]
        public DeleteLeaderboardCommandHandler(ILeaderboardRepository leaderboardRepository)
        {
            _leaderboardRepository = leaderboardRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(DeleteLeaderboardCommand request, CancellationToken cancellationToken)
        {
            var existingLeaderboard = await _leaderboardRepository.FindByIdAsync(request.Id, cancellationToken);
            if (existingLeaderboard is null)
            {
                throw new NotFoundException($"Could not find Leaderboard '{request.Id}'");
            }

            _leaderboardRepository.Remove(existingLeaderboard);
        }
    }

    public class DeleteLeaderboardCommandValidator : AbstractValidator<DeleteLeaderboardCommand>
    {
        [IntentManaged(Mode.Merge)]
        public DeleteLeaderboardCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}