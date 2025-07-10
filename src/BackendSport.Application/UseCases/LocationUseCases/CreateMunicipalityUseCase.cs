using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Application.UseCases.LocationUseCases;

public class CreateMunicipalityUseCase
{
    private readonly IMunicipalityRepository _municipalityRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public CreateMunicipalityUseCase(IMunicipalityRepository municipalityRepository, IDepartmentRepository departmentRepository)
    {
        _municipalityRepository = municipalityRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<MunicipalityDto> ExecuteAsync(CreateMunicipalityDto createMunicipalityDto)
    {
        // Validar que el departamento existe
        var department = await _departmentRepository.GetByIdAsync(createMunicipalityDto.DepartmentId);
        if (department == null)
        {
            throw new InvalidOperationException($"No existe un departamento con el ID {createMunicipalityDto.DepartmentId}");
        }

        // Validar que el código no esté duplicado
        var existingMunicipality = await _municipalityRepository.GetByCodeAsync(createMunicipalityDto.Code);
        if (existingMunicipality != null)
        {
            throw new InvalidOperationException($"Ya existe un municipio con el código {createMunicipalityDto.Code}");
        }

        var municipality = new Municipality
        {
            Id = Guid.NewGuid().ToString(),
            Name = createMunicipalityDto.Name,
            Code = createMunicipalityDto.Code,
            DepartmentId = createMunicipalityDto.DepartmentId,
            Type = createMunicipalityDto.Type,
            IsCapital = createMunicipalityDto.IsCapital,
            IsActive = true
        };

        var createdMunicipality = await _municipalityRepository.CreateAsync(municipality);

        return new MunicipalityDto
        {
            Id = createdMunicipality.Id,
            Name = createdMunicipality.Name,
            Code = createdMunicipality.Code,
            DepartmentId = createdMunicipality.DepartmentId,
            Type = createdMunicipality.Type,
            IsCapital = createdMunicipality.IsCapital
        };
    }
} 