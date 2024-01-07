using System;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.TimeEntries
{
    public class GuessDTO
    {
        public GuessDTO()
        {
        }

        public DateTimeOffset GuessDateTime { get; set; }

        public static GuessDTO Create(DateTimeOffset guessDateTime)
        {
            return new GuessDTO
            {
                GuessDateTime = guessDateTime
            };
        }
    }
}