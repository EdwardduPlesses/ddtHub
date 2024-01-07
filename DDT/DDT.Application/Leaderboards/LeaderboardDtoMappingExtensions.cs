using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DDT.Domain.Entities;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.AutoMapper.MappingExtensions", Version = "1.0")]

namespace DDT.Application.Leaderboards
{
    public static class LeaderboardDtoMappingExtensions
    {
        public static LeaderboardDto MapToLeaderboardDto(this Leaderboard projectFrom, IMapper mapper)
            => mapper.Map<LeaderboardDto>(projectFrom);

        public static List<LeaderboardDto> MapToLeaderboardDtoList(this IEnumerable<Leaderboard> projectFrom, IMapper mapper)
            => projectFrom.Select(x => x.MapToLeaderboardDto(mapper)).ToList();
    }
}