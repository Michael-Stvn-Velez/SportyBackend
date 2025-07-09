using System;
using System.Threading.Tasks;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Interfaces.DeporteInterfaces;

namespace BackendSport.Application.UseCases.AuthUseCases
{
    public class AsignarDeporteAUsuarioUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IDeporteRepository _deporteRepository;

        public AsignarDeporteAUsuarioUseCase(IUserRepository userRepository, IDeporteRepository deporteRepository)
        {
            _userRepository = userRepository;
            _deporteRepository = deporteRepository;
        }

        public async Task ExecuteAsync(AsignarDeporteUsuarioDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new InvalidOperationException("Usuario no encontrado");

            var deporte = await _deporteRepository.GetByIdAsync(dto.SportId);
            if (deporte == null)
                throw new InvalidOperationException("Deporte no encontrado");

            if (user.GetSportsCount() >= 3)
                throw new InvalidOperationException("El usuario ya tiene el máximo de 3 deportes permitidos");

            if (user.HasSport(dto.SportId))
                throw new InvalidOperationException("El usuario ya tiene este deporte asignado");

            var result = await _userRepository.AddSportToUserAsync(dto.UserId, dto.SportId);
            if (!result)
                throw new InvalidOperationException("No se pudo asignar el deporte al usuario");
        }
    }
} 