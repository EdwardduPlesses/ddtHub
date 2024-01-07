using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DDT.Application.Common.Interfaces;
using DDT.Domain.Repositories;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace DDT.Application.Leaderboards
{
    public class GetLeaderboardsQuery : IRequest<List<LeaderboardDto>>, IQuery
    {
        public GetLeaderboardsQuery()
        {
        }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetLeaderboardsQueryHandler : IRequestHandler<GetLeaderboardsQuery, List<LeaderboardDto>>
    {
        private readonly ILeaderboardRepository _leaderboardRepository;
        private readonly IMapper _mapper;

        [IntentManaged(Mode.Ignore)]
        public GetLeaderboardsQueryHandler(ILeaderboardRepository leaderboardRepository, IMapper mapper)
        {
            _leaderboardRepository = leaderboardRepository;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<LeaderboardDto>> Handle(GetLeaderboardsQuery request, CancellationToken cancellationToken)
        {
            var leaderboards = await _leaderboardRepository.FindAllAsync(cancellationToken);
            return leaderboards.MapToLeaderboardDtoList(_mapper);
        }
    }

    public class GetLeaderboardsQueryValidator : AbstractValidator<GetLeaderboardsQuery>
    {
        [IntentManaged(Mode.Merge)]
        public GetLeaderboardsQueryValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}