using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DDT.Application.Common.Interfaces;
using DDT.Domain.Repositories;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace DDT.Application.TimeEntries
{
    public class GetTimeEntriesQuery : IRequest<List<TimeEntryDto>>, IQuery
    {
        public GetTimeEntriesQuery()
        {
        }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetTimeEntriesQueryHandler : IRequestHandler<GetTimeEntriesQuery, List<TimeEntryDto>>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IMapper _mapper;

        [IntentManaged(Mode.Ignore)]
        public GetTimeEntriesQueryHandler(ITimeEntryRepository timeEntryRepository, IMapper mapper)
        {
            _timeEntryRepository = timeEntryRepository;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<TimeEntryDto>> Handle(GetTimeEntriesQuery request, CancellationToken cancellationToken)
        {
            var timeEntries = await _timeEntryRepository.FindAllAsync(cancellationToken);
            return timeEntries.MapToTimeEntryDtoList(_mapper);
        }
    }

    public class GetTimeEntriesQueryValidator : AbstractValidator<GetTimeEntriesQuery>
    {
        [IntentManaged(Mode.Merge)]
        public GetTimeEntriesQueryValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}