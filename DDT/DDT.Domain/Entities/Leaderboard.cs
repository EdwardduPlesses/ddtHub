using System;
using System.Collections.Generic;
using DDT.Domain.Common;
using Intent.RoslynWeaver.Attributes;

namespace DDT.Domain.Entities
{
    public class Leaderboard : IHasDomainEvent
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public int Score { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}