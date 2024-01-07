using DDT.Application.Common.Interfaces;
using DDT.Domain.Common.Interfaces;
using DDT.Domain.Repositories;
using DDT.Infrastructure.Persistence;
using DDT.Infrastructure.Repositories;
using DDT.Infrastructure.Services;
using Intent.RoslynWeaver.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Infrastructure.DependencyInjection.DependencyInjection", Version = "1.0")]

namespace DDT.Infrastructure
{
    [IntentManaged(Mode.Merge)]
    public static class DependencyInjection
    {
        [IntentManaged(Mode.Merge)]
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                options.UseLazyLoadingProxies();
            });
            services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddTransient<IEntryTypeRepository, EntryTypeRepository>();
            services.AddTransient<ILeaderboardRepository, LeaderboardRepository>();
            services.AddTransient<ISettingsRepository, SettingsRepository>();
            services.AddTransient<ITimeEntryRepository, TimeEntryRepository>();
            services.AddScoped<IDomainEventService, DomainEventService>();
            return services;
        }
    }
}