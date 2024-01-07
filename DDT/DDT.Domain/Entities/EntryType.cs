using System;
using System.Collections.Generic;
using DDT.Domain.Common;
using Intent.RoslynWeaver.Attributes;

namespace DDT.Domain.Entities
{
    public class EntryType : IHasDomainEvent
    {
        public Guid Id { get; set; }

        public EntryTypeEnum Type { get; set; }

        public string Description { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}