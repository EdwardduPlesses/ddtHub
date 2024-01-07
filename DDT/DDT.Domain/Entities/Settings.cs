using System;
using System.Collections.Generic;
using DDT.Domain.Common;
using Intent.RoslynWeaver.Attributes;

namespace DDT.Domain.Entities
{
    public class Settings : IHasDomainEvent
    {
        public Guid Id { get; set; }

        public string Property { get; set; }

        public string Value { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}