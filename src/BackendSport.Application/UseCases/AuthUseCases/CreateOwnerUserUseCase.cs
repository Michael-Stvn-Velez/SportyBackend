using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Services;
using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.Application.UseCases.AuthUseCases{
    public class CreateOwnerUserUseCase
    {
        private readonly IOwnerUserRepository _ownerUserRepository;
        private readonly IPasswordService _passwordService;

        public CreateOwnerUserUseCase(IOwnerUserRepository ownerUserRepository, IPasswordService passwordService)
        {
            _ownerUserRepository = ownerUserRepository;
            _passwordService = passwordService;
        }

        public async Task<OwnerUserResponseDto> ExecuteAsync(CreateOwnerUserDto createOwnerUserDto)
        {
            if (await _ownerUserRepository.ExistsByEmailAsync(createOwnerUserDto.Email))
            {
                throw new InvalidOperationException($"Ya existe un usuario con el email {createOwnerUserDto.Email}");
            }

            // Crear el usuario con la contraseña hasheada
            var ownerUser = new OwnerUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = createOwnerUserDto.Name,
                Email = createOwnerUserDto.Email,
                Phone = createOwnerUserDto.Phone,
                Password = _passwordService.HashPassword(createOwnerUserDto.Password),
                DocumentTypeId = createOwnerUserDto.DocumentTypeId,
                DocumentNumber = createOwnerUserDto.DocumentNumber,
                CountryId = createOwnerUserDto.CountryId,
                DepartmentId = createOwnerUserDto.DepartmentId,
                MunicipalityId = createOwnerUserDto.MunicipalityId,
                CityId = createOwnerUserDto.CityId,
                LocalityId = createOwnerUserDto.LocalityId,
                CreatedAt = DateTime.UtcNow,
                RolIds = new List<string> { "h76f23h73hd" } // Asignar solo un rolId por defecto
            };

            // Guardar en la base de datos
            var createdOwnerUser = await _ownerUserRepository.CreateAsync(ownerUser);

            // Retornar respuesta sin la contraseña
            return new OwnerUserResponseDto
            {
                Id = createdOwnerUser.Id.ToString(),
                Name = createdOwnerUser.Name,
                Email = createdOwnerUser.Email,
                DocumentTypeId = createdOwnerUser.DocumentTypeId,
                DocumentNumber = createdOwnerUser.DocumentNumber,
                CountryId = createdOwnerUser.CountryId,
                DepartmentId = createdOwnerUser.DepartmentId,
                MunicipalityId = createdOwnerUser.MunicipalityId,
                CityId = createdOwnerUser.CityId,
                LocalityId = createdOwnerUser.LocalityId,
                CreatedAt = createdOwnerUser.CreatedAt
            };
        }
    } 
}