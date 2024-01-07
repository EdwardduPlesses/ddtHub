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

namespace DDT.Application.Settings
{
    public class GetSettingsByIdQuery : IRequest<SettingsDto>, IQuery
    {
        public GetSettingsByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetSettingsByIdQueryHandler : IRequestHandler<GetSettingsByIdQuery, SettingsDto>
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IMapper _mapper;

        [IntentManaged(Mode.Ignore)]
        public GetSettingsByIdQueryHandler(ISettingsRepository settingsRepository, IMapper mapper)
        {
            _settingsRepository = settingsRepository;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<SettingsDto> Handle(GetSettingsByIdQuery request, CancellationToken cancellationToken)
        {
            var settings = await _settingsRepository.FindByIdAsync(request.Id, cancellationToken);
            if (settings is null)
            {
                throw new NotFoundException($"Could not find Settings '{request.Id}'");
            }

            return settings.MapToSettingsDto(_mapper);
        }
    }

    public class GetSettingsByIdQueryValidator : AbstractValidator<GetSettingsByIdQuery>
    {
        [IntentManaged(Mode.Merge)]
        public GetSettingsByIdQueryValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}