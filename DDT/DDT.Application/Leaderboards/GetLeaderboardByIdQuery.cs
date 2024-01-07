using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DDT.Application.Common.Interfaces;
using DDT.Domain.Common.Exceptions;
using DDT.Domain.Repositories;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace DDT.Application.Leaderboards
{
    public class GetLeaderboardByIdQuery : IRequest<LeaderboardDto>, IQuery
    {
        public GetLeaderboardByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetLeaderboardByIdQueryHandler : IRequestHandler<GetLeaderboardByIdQuery, LeaderboardDto>
    {
        private readonly ILeaderboardRepository _leaderboardRepository;
        private readonly IMapper _mapper;

        [IntentManaged(Mode.Ignore)]
        public GetLeaderboardByIdQueryHandler(ILeaderboardRepository leaderboardRepository, IMapper mapper)
        {
            _leaderboardRepository = leaderboardRepository;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<LeaderboardDto> Handle(GetLeaderboardByIdQuery request, CancellationToken cancellationToken)
        {
            var leaderboard = await _leaderboardRepository.FindByIdAsync(request.Id, cancellationToken);
            if (leaderboard is null)
            {
                throw new NotFoundException($"Could not find Leaderboard '{request.Id}'");
            }

            return leaderboard.MapToLeaderboardDto(_mapper);
        }
    }

    public class GetLeaderboardByIdQueryValidator : AbstractValidator<GetLeaderboardByIdQuery>
    {
        [IntentManaged(Mode.Merge)]
        public GetLeaderboardByIdQueryValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}