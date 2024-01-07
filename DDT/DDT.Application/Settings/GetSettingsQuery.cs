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

namespace DDT.Application.Settings
{
    public class GetSettingsQuery : IRequest<List<SettingsDto>>, IQuery
    {
        public GetSettingsQuery()
        {
        }
    }

    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, List<SettingsDto>>
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IMapper _mapper;

        [IntentManaged(Mode.Ignore)]
        public GetSettingsQueryHandler(ISettingsRepository settingsRepository, IMapper mapper)
        {
            _settingsRepository = settingsRepository;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<SettingsDto>> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
        {
            var settings = await _settingsRepository.FindAllAsync(cancellationToken);
            return settings.MapToSettingsDtoList(_mapper);
        }
    }

    public class GetSettingsQueryValidator : AbstractValidator<GetSettingsQuery>
    {
        [IntentManaged(Mode.Merge)]
        public GetSettingsQueryValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
        }
    }
}