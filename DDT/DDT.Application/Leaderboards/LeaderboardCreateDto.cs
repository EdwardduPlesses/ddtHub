using System;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.Leaderboards
{
    public class LeaderboardCreateDto
    {
        public LeaderboardCreateDto()
        {
        }

        public Guid UserId { get; set; }
        public int Score { get; set; }

        public static LeaderboardCreateDto Create(Guid userId, int score)
        {
            return new LeaderboardCreateDto
            {
                UserId = userId,
                Score = score
            };
        }
    }
}