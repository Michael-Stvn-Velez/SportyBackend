using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Application.UseCases.LocationUseCases;

public class CreateDepartmentUseCase
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ICountryRepository _countryRepository;

    public CreateDepartmentUseCase(IDepartmentRepository departmentRepository, ICountryRepository countryRepository)
    {
        _departmentRepository = departmentRepository;
        _countryRepository = countryRepository;
    }

    public async Task<DepartmentDto> ExecuteAsync(CreateDepartmentDto createDepartmentDto)
    {
        // Validar que el país existe
        var country = await _countryRepository.GetByIdAsync(createDepartmentDto.CountryId);
        if (country == null)
        {
            throw new InvalidOperationException($"No existe un país con el ID {createDepartmentDto.CountryId}");
        }

        // Validar que el código no esté duplicado
        var existingDepartment = await _departmentRepository.GetByCodeAsync(createDepartmentDto.Code);
        if (existingDepartment != null)
        {
            throw new InvalidOperationException($"Ya existe un departamento con el código {createDepartmentDto.Code}");
        }

        var department = new Department
        {
            Id = Guid.NewGuid().ToString(),
            Name = createDepartmentDto.Name,
            Code = createDepartmentDto.Code,
            CountryId = createDepartmentDto.CountryId,
            IsActive = true
        };

        var createdDepartment = await _departmentRepository.CreateAsync(department);

        return new DepartmentDto
        {
            Id = createdDepartment.Id,
            Name = createdDepartment.Name,
            Code = createdDepartment.Code,
            CountryId = createdDepartment.CountryId
        };
    }
} 