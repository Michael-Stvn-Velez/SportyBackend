using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Services;
using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.Application.UseCases.AuthUseCases{
    public class CreateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public CreateUserUseCase(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<UserResponseDto> ExecuteAsync(CreateUserDto createUserDto)
        {
            if (await _userRepository.ExistsByEmailAsync(createUserDto.Email))
            {
                throw new InvalidOperationException($"Ya existe un usuario con el email {createUserDto.Email}");
            }

            // Crear el usuario con la contraseña hasheada
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = createUserDto.Email,
                Password = _passwordService.HashPassword(createUserDto.Password),
                CreatedAt = DateTime.UtcNow
            };

            // Guardar en la base de datos
            var createdUser = await _userRepository.CreateAsync(user);

            // Retornar respuesta sin la contraseña
            return new UserResponseDto
            {
                Id = createdUser.Id.ToString(),
                Email = createdUser.Email,
                CreatedAt = createdUser.CreatedAt
            };
        }
    } 
}