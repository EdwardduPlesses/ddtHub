using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DDT.Application.Settings;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Contracts.ServiceContract", Version = "1.0")]

namespace DDT.Application.Interfaces
{
    public interface ISettingsService : IDisposable
    {
        Task<Guid> CreateSettings(SettingsCreateDto dto, CancellationToken cancellationToken = default);
        Task<SettingsDto> FindSettingsById(Guid id, CancellationToken cancellationToken = default);
        Task<List<SettingsDto>> FindSettings(CancellationToken cancellationToken = default);
        Task UpdateSettings(Guid id, SettingsUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteSettings(Guid id, CancellationToken cancellationToken = default);
    }
}