using Marc2.Contracts.Accident;
using Marc2.Domain.Entities;
using Marc2.Domain.Exceptions;
using Marc2.Domain.Repositories;
using Marc2.Services.Abstractions;

namespace Marc2.Services
{
    public class AccidentService : IAccidentService
    {
        private readonly IRepositoryManager _repository;
        public AccidentService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<AccidentDto> CreateAccidentAsync(CreateAccidentDto createAccidentDto, string perpetratorEmail)
        {
            var perpetrator = await _repository.UserRepository.GetByEmailAsync(perpetratorEmail) ?? null!;

            var accident = MapFromCreateAccidentDto(createAccidentDto);
            accident.OrganizationId = perpetrator.Organization.Id;

            _repository.AccidentRepository.CreateAccident(accident);
            await _repository.UnitOfWork.SaveChangesAsync();

            return MapFromAccidentToDto(accident);
        }

        public async Task UpdateAccidentAsync(int accidentId, UpdateAccidentDto updateAccidentDto, string perpetratorEmail)
        {
            var accident = await _repository.AccidentRepository.GetAccidentById(accidentId);
            if (accident == null)
                throw new AccidentNotFoundException(accidentId);

            var perpetrator = await _repository.UserRepository.GetByEmailAsync(perpetratorEmail) ?? null!;
            if (perpetrator.Organization.Id != accident.OrganizationId)
                throw new AmbigousOrganizationException();

            MapFromUpdateAccidentDto(updateAccidentDto, accident);
            _repository.AccidentRepository.UpdateAccident(accident);
            await _repository.UnitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAccidentAsync(int accidentId, string perpetratorEmail)
        {
            var accident = await _repository.AccidentRepository.GetAccidentById(accidentId);
            if (accident == null)
                throw new AccidentNotFoundException(accidentId);

            var perpetrator = await _repository.UserRepository.GetByEmailAsync(perpetratorEmail) ?? null!;
            if (perpetrator.Organization.Id != accident.OrganizationId)
                throw new AmbigousOrganizationException();

            _repository.AccidentRepository.DeleteAccident(accident);
            await _repository.UnitOfWork.SaveChangesAsync();
        }

        public async Task<AccidentDto> GetAccidentByIdAsync(int accidentId, string perpetratorEmail)
        {
            var accident = await _repository.AccidentRepository.GetAccidentById(accidentId);
            if (accident == null)
                throw new AccidentNotFoundException(accidentId);

            var perpetrator = await _repository.UserRepository.GetByEmailAsync(perpetratorEmail) ?? null!;
            if (perpetrator.Organization.Id != accident.OrganizationId)
                throw new AmbigousOrganizationException();

            return MapFromAccidentToDto(accident);
        }

        public async Task<IEnumerable<AccidentDto>> GetAllAccidentsByOrganizationAsync(string perpetratorEmail)
        {
            var perpetrator = await _repository.UserRepository.GetByEmailAsync(perpetratorEmail) ?? null!;

            var accidents = await _repository.AccidentRepository.GetAccidentsByOrganizationAsync(perpetrator.Organization.Id);
            return accidents.ToList().Select(a => MapFromAccidentToDto(a)).AsEnumerable();
        }

        private void MapFromUpdateAccidentDto(UpdateAccidentDto updateAccidentDto, Accident accident)
        {
            accident.Address = updateAccidentDto.Address;
            accident.GeneralDescription = updateAccidentDto.GeneralDescription;
            accident.Lattitude = updateAccidentDto.Lattitude;
            accident.Longtitude = updateAccidentDto.Longtitude;
            accident.AccidentName = updateAccidentDto.AccidentName;
        }

        private Accident MapFromCreateAccidentDto(CreateAccidentDto createAccidentDto)
        {
            var accident = new Accident
            {
                Address = createAccidentDto.Address,
                Lattitude = createAccidentDto.Lattitude,
                Longtitude = createAccidentDto.Longtitude,
                GeneralDescription = createAccidentDto.GeneralDescription,
                AroseAt = DateTime.Now,
                AccidentName = createAccidentDto.AccidentName
            };

            return accident;
        }

        private AccidentDto MapFromAccidentToDto(Accident accident)
        {
            return new AccidentDto()
            {
                GeneralDescription = accident.GeneralDescription,
                Lattitude = accident.Lattitude,
                Longtitude = accident.Longtitude,
                Address = accident.Address,
                Id = accident.Id,
                OrganizationId = accident.OrganizationId,
                AroseAt = accident.AroseAt,
                AccidentName = accident.AccidentName
            };
        }
    }
}
