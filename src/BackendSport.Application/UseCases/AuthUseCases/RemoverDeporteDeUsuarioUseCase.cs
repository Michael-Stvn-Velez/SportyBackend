using System;
using System.Threading.Tasks;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.Interfaces.AuthInterfaces;

namespace BackendSport.Application.UseCases.AuthUseCases
{
    public class RemoverDeporteDeUsuarioUseCase
    {
        private readonly IUserRepository _userRepository;

        public RemoverDeporteDeUsuarioUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ExecuteAsync(RemoverDeporteUsuarioDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new InvalidOperationException("Usuario no encontrado");

            if (!user.HasSport(dto.SportId))
                throw new InvalidOperationException("El usuario no tiene este deporte asignado");

            var result = await _userRepository.RemoveSportFromUserAsync(dto.UserId, dto.SportId);
            if (!result)
                throw new InvalidOperationException("No se pudo remover el deporte del usuario");
        }
    }
} 