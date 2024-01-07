using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DDT.Application.Common.Interfaces;
using DDT.Application.Interfaces;
using DDT.Application.TimeEntries;
using DDT.Domain;
using DDT.Domain.Common.Exceptions;
using DDT.Domain.Entities;
using DDT.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.AspNetCore.Identity;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.ServiceImplementations.ServiceImplementation", Version = "1.0")]

namespace DDT.Application.Implementation
{
    [IntentManaged(Mode.Merge)]
    public class TimeEntriesService : ITimeEntriesService
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IMapper _mapper;
        private readonly IEntryTypeRepository _entryTypeRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly IMediator _mediator;
        private readonly ILeaderboardsService _leaderboardsService;


        [IntentManaged(Mode.Merge)]
        public TimeEntriesService(ITimeEntryRepository timeEntryRepository,
                                  IMapper mapper,
                                  ICurrentUserService currentUserService,
                                  IEntryTypeRepository entryTypeRepository,
                                  UserManager<ApplicationIdentityUser> userManager,
                                  IMediator mediator,
                                  ILeaderboardsService leaderboardsService)
        {
            _timeEntryRepository = timeEntryRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _entryTypeRepository = entryTypeRepository;
            _userManager = userManager;
            _mediator = mediator;
            _leaderboardsService = leaderboardsService;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> CreateTimeEntry(TimeEntryCreateDto dto, CancellationToken cancellationToken = default)
        {
            var newTimeEntry = new TimeEntry
            {
                EntryDateTime = dto.EntryDateTime,
                Date = dto.Date,
                UserId = dto.UserId,
                EntryTypeId = dto.EntryTypeId,
            };
            _timeEntryRepository.Add(newTimeEntry);
            await _timeEntryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return newTimeEntry.Id;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<TimeEntryDto> FindTimeEntryById(Guid id, CancellationToken cancellationToken = default)
        {
            var element = await _timeEntryRepository.FindByIdAsync(id, cancellationToken);

            if (element is null)
            {
                throw new NotFoundException($"Could not find TimeEntry {id}");
            }

            return element.MapToTimeEntryDto(_mapper);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<TimeEntryDto>> FindTimeEntries(CancellationToken cancellationToken = default)
        {
            var elements = await _timeEntryRepository.FindAllAsync(cancellationToken);
            return elements.MapToTimeEntryDtoList(_mapper);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task UpdateTimeEntry(Guid id, TimeEntryUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var existingTimeEntry = await _timeEntryRepository.FindByIdAsync(id, cancellationToken);

            if (existingTimeEntry is null)
            {
                throw new NotFoundException($"Could not find TimeEntry {id}");
            }

            existingTimeEntry.EntryDateTime = dto.EntryDateTime;
            existingTimeEntry.Date = dto.Date;
            existingTimeEntry.UserId = dto.UserId;
            existingTimeEntry.EntryTypeId = dto.EntryTypeId;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task DeleteTimeEntry(Guid id, CancellationToken cancellationToken = default)
        {
            var existingTimeEntry = await _timeEntryRepository.FindByIdAsync(id, cancellationToken);

            if (existingTimeEntry is null)
            {
                throw new NotFoundException($"Could not find TimeEntry {id}");
            }

            _timeEntryRepository.Remove(existingTimeEntry);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Guess(GuessDTO guess, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new MakeGuessCommand(guess), cancellationToken);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Actual(ActualDTO actual, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new SetActualCommand(actual), cancellationToken);
            await _leaderboardsService.UpdateLeaderboard(actual.ActualDateTime, cancellationToken);
        }

        public async Task AddTimeEntry(DateTimeOffset date,
                                       EntryTypeEnum entryTypeEnum,
                                       CancellationToken cancellationToken = default)
        {
            var userEmail = _currentUserService.UserId;

            if (userEmail == null)
            {
                throw new NotFoundException("User Email not found");
            }

            var userId = _userManager.FindByEmailAsync(userEmail).Result?.Id;
            var userIdGuid = Guid.Parse(userId);

            var entryType = await _entryTypeRepository.FindAsync(x => x.Type == entryTypeEnum, cancellationToken) ?? throw new NotFoundException("Entry Type not found");
            var entryTypeId = entryType.Id;

            var existingEntry = await _timeEntryRepository.AnyAsync(x => x.UserId == userIdGuid
                && x.EntryTypeId == entryType.Id
                && x.EntryDateTime.UtcDateTime.Date == date.UtcDateTime.Date, cancellationToken);

            if (Guid.TryParse(userId, out Guid theGuid))
            {
                if (!existingEntry)
                {
                    _timeEntryRepository.Add(new TimeEntry()
                    {
                        Date = DateTimeOffset.Now.UtcDateTime,
                        UserId = userIdGuid,
                        EntryTypeId = entryTypeId,
                        EntryDateTime = date,
                    });
                }
                else
                {
                    var message = entryTypeEnum switch
                    {
                        EntryTypeEnum.Actual => "User has already made an actual entry for this date",
                        EntryTypeEnum.Guess => "User has already made a guess entry for this date",
                        _ => throw new ArgumentOutOfRangeException(nameof(entryTypeEnum), entryTypeEnum, null)
                    };
                    throw new ValidationException(new[] { new ValidationFailure("Date", message) });
                }
            }
            else
            {
                throw new Exception("This is not a GUID man");
            }
        }

        public void Dispose()
        {
        }
    }
}