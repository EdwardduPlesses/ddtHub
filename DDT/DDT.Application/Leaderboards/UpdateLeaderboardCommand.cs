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

namespace DDT.Application.Leaderboards
{
    public class UpdateLeaderboardCommand : IRequest, ICommand
    {
        public UpdateLeaderboardCommand(Guid id, Guid userId, int score)
        {
            Id = id;
            UserId = userId;
            Score = score;
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Score { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateLeaderboardCommandHandler : IRequestHandler<UpdateLeaderboardCommand>
    {
        private readonly ILeaderboardRepository _leaderboardRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateLeaderboardCommandHandler(ILeaderboardRepository leaderboardRepository)
        {
            _leaderboardRepository = leaderboardRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateLeaderboardCommand request, CancellationToken cancellationToken)
        {
            var existingLeaderboard = await _leaderboardRepository.FindByIdAsync(request.Id, cancellationToken);
            if (existingLeaderboard is null)
            {
                throw new NotFoundException($"Could not find Leaderboard '{request.Id}'");
            }

            existingLeaderboard.UserId = request.UserId;
            existingLeaderboard.Score = request.Score;
        }
    }

    public class UpdateLeaderboardCommandValidator : AbstractValidator<UpdateLeaderboardCommand>
    {
        [IntentManaged(Mode.Merge)]
        public UpdateLeaderboardCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}