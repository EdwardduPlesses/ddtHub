using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DDT.Application.Leaderboards;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Contracts.ServiceContract", Version = "1.0")]

namespace DDT.Application.Interfaces
{
    [IntentManaged(Mode.Merge)]
    public interface ILeaderboardsService : IDisposable
    {
        Task<Guid> CreateLeaderboard(LeaderboardCreateDto dto, CancellationToken cancellationToken = default);
        Task<LeaderboardDto> FindLeaderboardById(Guid id, CancellationToken cancellationToken = default);
        Task<List<LeaderboardDto>> FindLeaderboards(CancellationToken cancellationToken = default);
        Task UpdateLeaderboard(Guid id, LeaderboardUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteLeaderboard(Guid id, CancellationToken cancellationToken = default);
        Task<List<LeaderboardDataDto>> GetLeaderboard(LeaderboardGetDto leaderboardGetData, CancellationToken cancellationToken = default);
        Task UpdateLeaderboard(DateTimeOffset actualDateTimeForDay, CancellationToken cancellationToken = default);
    }
}