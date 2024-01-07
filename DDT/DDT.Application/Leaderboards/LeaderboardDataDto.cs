using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.Leaderboards
{
    public class LeaderboardDataDto
    {
        public LeaderboardDataDto()
        {
            User = null!;
        }

        public string User { get; set; }
        public int Score { get; set; }

        public static LeaderboardDataDto Create(string user, int score)
        {
            return new LeaderboardDataDto
            {
                User = user,
                Score = score
            };
        }
    }
}