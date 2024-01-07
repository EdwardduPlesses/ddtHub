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

namespace DDT.Application.Leaderboards
{
    public class CreateLeaderboardCommand : IRequest<Guid>, ICommand
    {
        public CreateLeaderboardCommand(Guid userId, int score)
        {
            UserId = userId;
            Score = score;
        }

        public Guid UserId { get; set; }
        public int Score { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateLeaderboardCommandHandler : IRequestHandler<CreateLeaderboardCommand, Guid>
    {
        private readonly ILeaderboardRepository _leaderboardRepository;

        [IntentManaged(Mode.Merge)]
        public CreateLeaderboardCommandHandler(ILeaderboardRepository leaderboardRepository)
        {
            _leaderboardRepository = leaderboardRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateLeaderboardCommand request, CancellationToken cancellationToken)
        {
            var newLeaderboard = new Leaderboard
            {
                UserId = request.UserId,
                Score = request.Score,
            };

            _leaderboardRepository.Add(newLeaderboard);
            await _leaderboardRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return newLeaderboard.Id;
        }
    }

    public class CreateLeaderboardCommandValidator : AbstractValidator<CreateLeaderboardCommand>
    {
        [IntentManaged(Mode.Merge)]
        public CreateLeaderboardCommandValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}