using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DDT.Application.Common.Interfaces;
using DDT.Application.Interfaces;
using DDT.Application.Leaderboards;
using DDT.Domain;
using DDT.Domain.Common.Exceptions;
using DDT.Domain.Entities;
using DDT.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.AspNetCore.Identity;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.ServiceImplementations.ServiceImplementation", Version = "1.0")]

namespace DDT.Application.Implementation
{
    [IntentManaged(Mode.Merge)]
    public class LeaderboardsService : ILeaderboardsService
    {
        private readonly ILeaderboardRepository _leaderboardRepository;
        private readonly IMapper _mapper;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IEntryTypeRepository _entryTypeRepository;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        [IntentManaged(Mode.Merge)]
        public LeaderboardsService(ILeaderboardRepository leaderboardRepository,
                                   IMapper mapper,
                                   ITimeEntryRepository timeEntryRepository,
                                   IEntryTypeRepository entryTypeRepository,
                                   ICurrentUserService currentUserService,
                                   UserManager<ApplicationIdentityUser> userManager)
        {
            _leaderboardRepository = leaderboardRepository;
            _mapper = mapper;
            _timeEntryRepository = timeEntryRepository;
            _entryTypeRepository = entryTypeRepository;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> CreateLeaderboard(LeaderboardCreateDto dto,
                                                  CancellationToken cancellationToken = default)
        {
            var newLeaderboard = new Leaderboard
            {
                UserId = dto.UserId,
                Score = dto.Score,
            };
            _leaderboardRepository.Add(newLeaderboard);
            await _leaderboardRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return newLeaderboard.Id;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<LeaderboardDto> FindLeaderboardById(Guid id,
                                                              CancellationToken cancellationToken = default)
        {
            var element = await _leaderboardRepository.FindByIdAsync(id, cancellationToken);

            if (element is null)
            {
                throw new NotFoundException($"Could not find Leaderboard {id}");
            }

            return element.MapToLeaderboardDto(_mapper);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<LeaderboardDto>> FindLeaderboards(CancellationToken cancellationToken = default)
        {
            var elements = await _leaderboardRepository.FindAllAsync(cancellationToken);
            return elements.MapToLeaderboardDtoList(_mapper);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task UpdateLeaderboard(Guid id,
                                            LeaderboardUpdateDto dto,
                                            CancellationToken cancellationToken = default)
        {
            var existingLeaderboard = await _leaderboardRepository.FindByIdAsync(id, cancellationToken);

            if (existingLeaderboard is null)
            {
                throw new NotFoundException($"Could not find Leaderboard {id}");
            }

            existingLeaderboard.UserId = dto.UserId;
            existingLeaderboard.Score = dto.Score;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task DeleteLeaderboard(Guid id,
                                            CancellationToken cancellationToken = default)
        {
            var existingLeaderboard = await _leaderboardRepository.FindByIdAsync(id, cancellationToken);

            if (existingLeaderboard is null)
            {
                throw new NotFoundException($"Could not find Leaderboard {id}");
            }

            _leaderboardRepository.Remove(existingLeaderboard);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<List<LeaderboardDataDto>> GetLeaderboard(
            LeaderboardGetDto leaderboardGetData,
            CancellationToken cancellationToken = default)
        {
            var leaderBoardDataDto = new List<LeaderboardDataDto>();
            var leaderboard = await _leaderboardRepository.FindAllAsync(cancellationToken);

            foreach (var leaderboardData in leaderboard)
            {
                var user = await _userManager.FindByIdAsync(leaderboardData.UserId.ToString());
                leaderBoardDataDto.Add(LeaderboardDataDto.Create(user?.UserName ?? "User", leaderboardData.Score));
            }

            var orderedLeaderboard = leaderBoardDataDto.OrderByDescending(x => x.Score).ToList();
            return orderedLeaderboard;
        }

        public async Task UpdateLeaderboard(DateTimeOffset actualDateTimeForDay,
                                            CancellationToken cancellationToken = default)
        {
            var entryTypeGuessId = (await _entryTypeRepository.FindAsync(x => x.Type == EntryTypeEnum.Guess, cancellationToken))?.Id;
            var entryTypeActualId = (await _entryTypeRepository.FindAsync(x => x.Type == EntryTypeEnum.Actual, cancellationToken))?.Id;

            var listTimeEntries = await _timeEntryRepository.FindAllAsync(x =>
                x.EntryTypeId == entryTypeGuessId
                && x.EntryDateTime.UtcDateTime.Date == actualDateTimeForDay.UtcDateTime.Date, cancellationToken);

            var actualForDay = await _timeEntryRepository.FindAllAsync(x =>
                x.EntryTypeId == entryTypeActualId
                && x.EntryDateTime.UtcDateTime.Date == actualDateTimeForDay.UtcDateTime.Date, cancellationToken);

            var differences = new List<(TimeEntry, double)>();

            foreach (var entry in listTimeEntries)
            {
                var actualEntry = actualForDay.First();
                var differenceInMinutes = CalculateDifferenceInMinutes(entry, actualEntry);
                differences.Add((entry, differenceInMinutes));
            }

            var orderedDifferences = differences.OrderBy(x => x.Item2).ToList();

            foreach (var orderedDifference in orderedDifferences)
            {
                if (orderedDifference.Item2 < 1)
                {
                    //Exact match
                    await AddOrUpdateUserScore(orderedDifference, 50);
                }
                else
                {
                    var index = orderedDifferences.IndexOf(orderedDifference);
                    switch (index)
                    {
                        case 0:
                            //First place
                            await AddOrUpdateUserScore(orderedDifference, 20);
                            break;
                        case 1:
                            //Second place
                            await AddOrUpdateUserScore(orderedDifference, 15);
                            break;
                        case 2:
                            //Third place
                            await AddOrUpdateUserScore(orderedDifference, 10);
                            break;
                        default:
                        {
                            //Fourth place and below
                            var score = CalculateScore(orderedDifference.Item2);
                            await AddOrUpdateUserScore(orderedDifference, score);
                            break;
                        }
                    }
                }
            }

            Console.WriteLine("Leaderboard updated");
        }

        private int CalculateScore(double differenceInMinutes)
        {
            if (differenceInMinutes > 60)
            {
                return 0;
            }

            var scoreDouble = Math.Ceiling((60.0 - differenceInMinutes) / 6);
            return int.Parse(scoreDouble.ToString(CultureInfo.CurrentCulture));
        }


        private async Task AddOrUpdateUserScore((TimeEntry, double) orderedDifference,
                                                int score)
        {
            var userRecord = await _leaderboardRepository.FindAsync(x => x.UserId == orderedDifference.Item1.UserId);

            if (userRecord is not null)
            {
                userRecord.Score += score;
                _leaderboardRepository.Update(userRecord);
            }
            else
            {
                _leaderboardRepository.Add(new Leaderboard
                {
                    UserId = orderedDifference.Item1.UserId,
                    Score = score,
                });
            }
        }

        private double CalculateDifferenceInMinutes(TimeEntry entry,
                                                    TimeEntry actual)
        {
            var timeSpan = actual.EntryDateTime.UtcDateTime - entry.EntryDateTime.UtcDateTime;
            return Math.Abs(timeSpan.TotalMinutes);
        }


        public void Dispose()
        {
        }
    }
}