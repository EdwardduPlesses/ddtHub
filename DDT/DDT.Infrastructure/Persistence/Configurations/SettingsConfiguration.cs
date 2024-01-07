using DDT.Domain.Entities;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.EntityTypeConfiguration", Version = "1.0")]

namespace DDT.Infrastructure.Persistence.Configurations
{
    public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Property)
                .IsRequired();

            builder.Property(x => x.Value)
                .IsRequired();

            builder.Ignore(e => e.DomainEvents);
        }
    }
}