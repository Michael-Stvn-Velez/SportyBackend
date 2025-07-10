using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.Interfaces.LocationInterfaces;

namespace BackendSport.Application.UseCases.LocationUseCases;

public class GetLocationHierarchyUseCase
{
    private readonly ICountryRepository _countryRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMunicipalityRepository _municipalityRepository;
    private readonly ILocalityRepository _localityRepository;

    public GetLocationHierarchyUseCase(
        ICountryRepository countryRepository,
        IDepartmentRepository departmentRepository,
        IMunicipalityRepository municipalityRepository,
        ILocalityRepository localityRepository)
    {
        _countryRepository = countryRepository;
        _departmentRepository = departmentRepository;
        _municipalityRepository = municipalityRepository;
        _localityRepository = localityRepository;
    }

    public async Task<LocationHierarchyResponseDto> ExecuteAsync(string countryId)
    {
        var countries = await _countryRepository.GetAllAsync();
        var departments = await _departmentRepository.GetByCountryIdAsync(countryId);
        var municipalities = await _municipalityRepository.GetAllAsync();
        var localities = await _localityRepository.GetAllAsync();

        return new LocationHierarchyResponseDto
        {
            Countries = countries.Where(c => c.IsActive)
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Code = c.Code
                }).ToList(),
            
            Departments = departments.Where(d => d.IsActive)
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Code = d.Code,
                    CountryId = d.CountryId
                }).ToList(),
            
            Municipalities = municipalities.Where(m => m.IsActive)
                .Select(m => new MunicipalityDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Code = m.Code,
                    DepartmentId = m.DepartmentId,
                    Type = m.Type,
                    IsCapital = m.IsCapital
                }).ToList(),
            
            Localities = localities.Where(l => l.IsActive)
                .Select(l => new LocalityDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    Code = l.Code,
                    MunicipalityId = l.MunicipalityId,
                    Type = l.Type
                }).ToList()
        };
    }
} 