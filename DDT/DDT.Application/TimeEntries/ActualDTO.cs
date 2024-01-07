using System;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DDT.Application.TimeEntries
{
    public class ActualDTO
    {
        public ActualDTO()
        {
        }

        public DateTimeOffset ActualDateTime { get; set; }

        public static ActualDTO Create(DateTimeOffset actualDateTime)
        {
            return new ActualDTO
            {
                ActualDateTime = actualDateTime
            };
        }
    }
}