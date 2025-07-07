using System;
using System.Threading.Tasks;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Interfaces.RolInterfaces;

namespace BackendSport.Application.UseCases.AuthUseCases
{
    public class AsignarRolAUsuarioUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRolRepository _rolRepository;

        public AsignarRolAUsuarioUseCase(IUserRepository userRepository, IRolRepository rolRepository)
        {
            _userRepository = userRepository;
            _rolRepository = rolRepository;
        }

        public async Task ExecuteAsync(AsignarRolUsuarioDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new InvalidOperationException("Usuario no encontrado");

            var rol = await _rolRepository.ObtenerPorIdAsync(dto.RolId);
            if (rol == null)
                throw new InvalidOperationException("Rol no encontrado");

            var result = await _userRepository.AddRolToUserAsync(dto.UserId, dto.RolId);
            if (!result)
                throw new InvalidOperationException("No se pudo asignar el rol o ya estaba asignado");
        }
    }
} 