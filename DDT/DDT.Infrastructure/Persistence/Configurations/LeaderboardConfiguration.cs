using DDT.Domain.Entities;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.EntityTypeConfiguration", Version = "1.0")]

namespace DDT.Infrastructure.Persistence.Configurations
{
    public class LeaderboardConfiguration : IEntityTypeConfiguration<Leaderboard>
    {
        public void Configure(EntityTypeBuilder<Leaderboard> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Score)
                .IsRequired();

            builder.Ignore(e => e.DomainEvents);
        }
    }
}