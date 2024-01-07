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

namespace DDT.Application.EntryTypes
{
    public class GetEntryTypeByIdQuery : IRequest<EntryTypeDto>, IQuery
    {
        public GetEntryTypeByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetEntryTypeByIdQueryHandler : IRequestHandler<GetEntryTypeByIdQuery, EntryTypeDto>
    {
        private readonly IEntryTypeRepository _entryTypeRepository;
        private readonly IMapper _mapper;

        [IntentManaged(Mode.Ignore)]
        public GetEntryTypeByIdQueryHandler(IEntryTypeRepository entryTypeRepository, IMapper mapper)
        {
            _entryTypeRepository = entryTypeRepository;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<EntryTypeDto> Handle(GetEntryTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var entryType = await _entryTypeRepository.FindByIdAsync(request.Id, cancellationToken);
            if (entryType is null)
            {
                throw new NotFoundException($"Could not find EntryType '{request.Id}'");
            }

            return entryType.MapToEntryTypeDto(_mapper);
        }
    }

    public class GetEntryTypeByIdQueryValidator : AbstractValidator<GetEntryTypeByIdQuery>
    {
        [IntentManaged(Mode.Merge)]
        public GetEntryTypeByIdQueryValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}