using BackendSport.Application.DTOs.PermisosDTOs;
using BackendSport.Application.UseCases.PermisosUseCases;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackendSport.API.Controllers.PermisosControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermisosController : ControllerBase
    {
        private readonly CreatePermisosUseCase _createPermisosUseCase;
        private readonly UpdatePermisosUseCase _updatePermisosUseCase;
        private readonly DeletePermisosUseCase _deletePermisosUseCase;
        private readonly GetPermisosByIdUseCase _getPermisosByIdUseCase;
        private readonly GetAllPermisosUseCase _getAllPermisosUseCase;

        public PermisosController(CreatePermisosUseCase createPermisosUseCase, UpdatePermisosUseCase updatePermisosUseCase, DeletePermisosUseCase deletePermisosUseCase, GetPermisosByIdUseCase getPermisosByIdUseCase, GetAllPermisosUseCase getAllPermisosUseCase)
        {
            _createPermisosUseCase = createPermisosUseCase;
            _updatePermisosUseCase = updatePermisosUseCase;
            _deletePermisosUseCase = deletePermisosUseCase;
            _getPermisosByIdUseCase = getPermisosByIdUseCase;
            _getAllPermisosUseCase = getAllPermisosUseCase;
        }

        /// <summary>
        /// Crea un nuevo permiso.
        /// </summary>
        /// <param name="permisosDto">Datos del permisos a crear (nombre y descripcion)</param>
        /// <returns>El permisos creado (nombre y descripcion)</returns>
        /// <response code="200">Permisos creado correctamente</response>
        /// <response code="400">Ya existe un permisos con ese nombre</response>
        [HttpPost]
        [ProducesResponseType(typeof(PermisosDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CrearPermisos([FromBody] PermisosDto permisosDto)
        {
            try
            {
                var result = await _createPermisosUseCase.ExecuteAsync(permisosDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Edita un permiso existente.
        /// </summary>
        /// <param name="id">Id del permiso a editar</param>
        /// <param name="permisosUpdateDto">Datos nuevos del permiso</param>
        /// <returns>True si se editó correctamente</returns>
        /// <response code="200">Permiso editado correctamente</response>
        /// <response code="400">Error de validación</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> EditarPermisos(string id, [FromBody] PermisosUpdateDto permisosUpdateDto)
        {
            try
            {
                var result = await _updatePermisosUseCase.ExecuteAsync(id, permisosUpdateDto);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un permiso existente.
        /// </summary>
        /// <param name="id">Id del permiso a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        /// <response code="200">Permiso eliminado correctamente</response>
        /// <response code="400">Error de validación</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> EliminarPermisos(string id)
        {
            try
            {
                var result = await _deletePermisosUseCase.ExecuteAsync(id);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un permiso por su id.
        /// </summary>
        /// <param name="id">Id del permiso</param>
        /// <returns>El permiso encontrado (id, nombre, descripcion)</returns>
        /// <response code="200">Permiso encontrado</response>
        /// <response code="404">Permiso no encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PermisosListDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ObtenerPermisosPorId(string id)
        {
            var result = await _getPermisosByIdUseCase.ExecuteAsync(id);
            if (result == null)
                return NotFound(new { mensaje = "Permiso no encontrado." });
            return Ok(result);
        }

        /// <summary>
        /// Obtiene todos los permisos.
        /// </summary>
        /// <returns>Lista de permisos</returns>
        /// <response code="200">Lista de permisos</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<PermisosListDto>), 200)]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _getAllPermisosUseCase.ExecuteAsync();
            return Ok(result);
        }
    }
} 