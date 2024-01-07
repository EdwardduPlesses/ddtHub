using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DDT.Application.Common.Interfaces;
using DDT.Domain.Common.Exceptions;
using DDT.Domain.Repositories;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace DDT.Application.TimeEntries
{
    public class GetTimeEntryByIdQuery : IRequest<TimeEntryDto>, IQuery
    {
        public GetTimeEntryByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetTimeEntryByIdQueryHandler : IRequestHandler<GetTimeEntryByIdQuery, TimeEntryDto>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IMapper _mapper;

        [IntentManaged(Mode.Ignore)]
        public GetTimeEntryByIdQueryHandler(ITimeEntryRepository timeEntryRepository, IMapper mapper)
        {
            _timeEntryRepository = timeEntryRepository;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<TimeEntryDto> Handle(GetTimeEntryByIdQuery request, CancellationToken cancellationToken)
        {
            var timeEntry = await _timeEntryRepository.FindByIdAsync(request.Id, cancellationToken);
            if (timeEntry is null)
            {
                throw new NotFoundException($"Could not find TimeEntry '{request.Id}'");
            }

            return timeEntry.MapToTimeEntryDto(_mapper);
        }
    }

    public class GetTimeEntryByIdQueryValidator : AbstractValidator<GetTimeEntryByIdQuery>
    {
        [IntentManaged(Mode.Merge)]
        public GetTimeEntryByIdQueryValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}