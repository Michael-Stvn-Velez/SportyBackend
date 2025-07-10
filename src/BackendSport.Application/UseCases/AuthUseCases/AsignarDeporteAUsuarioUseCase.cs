using System;
using System.Threading.Tasks;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Interfaces.DeporteInterfaces;
using BackendSport.Domain.Services;
using System.Collections.Generic; // Added for List

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

            if (!UserSportService.CanUserAddMoreSports(user))
                throw new InvalidOperationException("El usuario ya tiene el máximo de 3 deportes permitidos");

            if (UserSportService.UserHasSport(user, dto.SportId))
                throw new InvalidOperationException("El usuario ya tiene este deporte asignado");

            // Crear UserSport con configuración inicial basada en el deporte
            var userSport = CreateInitialUserSport(deporte);

            var result = await _userRepository.AddSportToUserAsync(dto.UserId, userSport);
            if (!result)
                throw new InvalidOperationException("No se pudo asignar el deporte al usuario");
        }

        private static BackendSport.Domain.Entities.AuthEntities.UserSport CreateInitialUserSport(BackendSport.Domain.Entities.DeporteEntities.Deporte deporte)
        {
            var userSport = new BackendSport.Domain.Entities.AuthEntities.UserSport
            {
                SportId = deporte.Id
            };

            // Si el deporte tiene posiciones, inicializar como array vacío
            if (deporte.Positions != null && deporte.Positions.Count > 0)
            {
                userSport.Positions = new List<string>();
            }

            // Si el deporte tiene métricas, inicializar como diccionario vacío
            if (deporte.PerformanceMetrics != null && deporte.PerformanceMetrics.Count > 0)
            {
                userSport.PerformanceMetrics = new Dictionary<string, int>();
            }

            return userSport;
        }
    }
} 