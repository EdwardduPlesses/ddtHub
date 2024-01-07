using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DDT.Application.EntryTypes;
using DDT.Application.Interfaces;
using DDT.Domain.Common.Exceptions;
using DDT.Domain.Entities;
using DDT.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.ServiceImplementations.ServiceImplementation", Version = "1.0")]

namespace DDT.Application.Implementation
{
    [IntentManaged(Mode.Merge)]
    public class EntryTypesService : IEntryTypesService
    {
        private readonly IEntryTypeRepository _entryTypeRepository;
        private readonly IMapper _mapper;

        [IntentManaged(Mode.Merge)]
        public EntryTypesService(IEntryTypeRepository entryTypeRepository, IMapper mapper)
        {
            _entryTypeRepository = entryTypeRepository;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> CreateEntryType(EntryTypeCreateDto dto, CancellationToken cancellationToken = default)
        {
            var newEntryType = new EntryType
            {
                Type = dto.Type,
                Description = dto.Description,
            };
            _entryTypeRepository.Add(newEntryType);
            await _entryTypeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return newEntryType.Id;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<EntryTypeDto> FindEntryTypeById(Guid id, CancellationToken cancellationToken = default)
        {
            var element = await _entryTypeRepository.FindByIdAsync(id, cancellationToken);

            if (element is null)
            {
                throw new NotFoundException($"Could not find EntryType {id}");
            }
            return element.MapToEntryTypeDto(_mapper);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<EntryTypeDto>> FindEntryTypes(CancellationToken cancellationToken = default)
        {
            var elements = await _entryTypeRepository.FindAllAsync(cancellationToken);
            return elements.MapToEntryTypeDtoList(_mapper);
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task UpdateEntryType(Guid id, EntryTypeUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var existingEntryType = await _entryTypeRepository.FindByIdAsync(id, cancellationToken);

            if (existingEntryType is null)
            {
                throw new NotFoundException($"Could not find EntryType {id}");
            }
            existingEntryType.Type = dto.Type;
            existingEntryType.Description = dto.Description;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task DeleteEntryType(Guid id, CancellationToken cancellationToken = default)
        {
            var existingEntryType = await _entryTypeRepository.FindByIdAsync(id, cancellationToken);

            if (existingEntryType is null)
            {
                throw new NotFoundException($"Could not find EntryType {id}");
            }
            _entryTypeRepository.Remove(existingEntryType);
        }

        public void Dispose()
        {
        }
    }
}