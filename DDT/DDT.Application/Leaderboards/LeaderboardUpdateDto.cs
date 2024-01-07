using System;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.Leaderboards
{
    public class LeaderboardUpdateDto
    {
        public LeaderboardUpdateDto()
        {
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Score { get; set; }

        public static LeaderboardUpdateDto Create(Guid id, Guid userId, int score)
        {
            return new LeaderboardUpdateDto
            {
                Id = id,
                UserId = userId,
                Score = score
            };
        }
    }
}