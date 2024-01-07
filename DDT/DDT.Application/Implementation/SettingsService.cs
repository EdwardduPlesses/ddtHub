using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DDT.Application.Interfaces;
using DDT.Application.Settings;
using DDT.Domain.Common.Exceptions;
using DDT.Domain.Entities;
using DDT.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.ServiceImplementations.ServiceImplementation", Version = "1.0")]

namespace DDT.Application.Implementation
{
    [IntentManaged(Mode.Merge)]
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        [IntentManaged(Mode.Merge)]
        public SettingsService(ISettingsRepository settingsRepository, IMapper mapper,
                               IMediator mediator)
        {
            _settingsRepository = settingsRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> CreateSettings(SettingsCreateDto dto, CancellationToken cancellationToken = default)
        {
            var newSettings = new Domain.Entities.Settings
            {
                Property = dto.Property,
                Value = dto.Value,
            };
            _settingsRepository.Add(newSettings);
            await _settingsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return newSettings.Id;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<SettingsDto> FindSettingsById(Guid id, CancellationToken cancellationToken = default)
        {
            var element = await _settingsRepository.FindByIdAsync(id, cancellationToken);

            if (element is null)
            {
                throw new NotFoundException($"Could not find Settings {id}");
            }
            return element.MapToSettingsDto(_mapper);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<SettingsDto>> FindSettings(CancellationToken cancellationToken = default)
        {
            var elements = await _settingsRepository.FindAllAsync(cancellationToken);
            return elements.MapToSettingsDtoList(_mapper);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task UpdateSettings(Guid id, SettingsUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var existingSettings = await _settingsRepository.FindByIdAsync(id, cancellationToken);

            if (existingSettings is null)
            {
                throw new NotFoundException($"Could not find Settings {id}");
            }
            existingSettings.Property = dto.Property;
            existingSettings.Value = dto.Value;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task DeleteSettings(Guid id, CancellationToken cancellationToken = default)
        {
            var existingSettings = await _settingsRepository.FindByIdAsync(id, cancellationToken);

            if (existingSettings is null)
            {
                throw new NotFoundException($"Could not find Settings {id}");
            }
            _settingsRepository.Remove(existingSettings);
        }

        public void Dispose()
        {
        }
    }
}