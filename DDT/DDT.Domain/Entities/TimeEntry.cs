using System;
using System.Collections.Generic;
using DDT.Domain.Common;
using Intent.RoslynWeaver.Attributes;

namespace DDT.Domain.Entities
{
    public class TimeEntry : IHasDomainEvent
    {
        public Guid Id { get; set; }

        public DateTimeOffset EntryDateTime { get; set; }

        public DateTimeOffset Date { get; set; }

        public Guid UserId { get; set; }

        public Guid EntryTypeId { get; set; }

        public virtual EntryType EntryType { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}