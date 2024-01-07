using System;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.Leaderboards
{
    public class LeaderboardGetDto
    {
        public LeaderboardGetDto()
        {
        }

        public DateTimeOffset? DateStart { get; set; }
        public DateTimeOffset? DateEnd { get; set; }

        public static LeaderboardGetDto Create(DateTimeOffset? dateStart, DateTimeOffset? dateEnd)
        {
            return new LeaderboardGetDto
            {
                DateStart = dateStart,
                DateEnd = dateEnd
            };
        }
    }
}