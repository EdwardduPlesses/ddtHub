using System;
using AutoMapper;
using DDT.Application.Common.Mappings;
using DDT.Domain.Entities;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.Leaderboards
{
    public class LeaderboardDto : IMapFrom<Leaderboard>
    {
        public LeaderboardDto()
        {
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Score { get; set; }

        public static LeaderboardDto Create(Guid id, Guid userId, int score)
        {
            return new LeaderboardDto
            {
                Id = id,
                UserId = userId,
                Score = score
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Leaderboard, LeaderboardDto>();
        }
    }
}