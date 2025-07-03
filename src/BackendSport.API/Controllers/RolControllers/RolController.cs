using BackendSport.Application.DTOs.RolDTOs;
using BackendSport.Application.UseCases.RolUseCases;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackendSport.API.Controllers.RolControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly CreateRolUseCase _createRolUseCase;
        private readonly UpdateRolUseCase _updateRolUseCase;
        private readonly DeleteRolUseCase _deleteRolUseCase;
        private readonly GetRolByIdUseCase _getRolByIdUseCase;
        private readonly GetAllRolesUseCase _getAllRolesUseCase;

        public RolController(CreateRolUseCase createRolUseCase, UpdateRolUseCase updateRolUseCase, DeleteRolUseCase deleteRolUseCase, GetRolByIdUseCase getRolByIdUseCase, GetAllRolesUseCase getAllRolesUseCase)
        {
            _createRolUseCase = createRolUseCase;
            _updateRolUseCase = updateRolUseCase;
            _deleteRolUseCase = deleteRolUseCase;
            _getRolByIdUseCase = getRolByIdUseCase;
            _getAllRolesUseCase = getAllRolesUseCase;
        }

        /// <summary>
        /// Crea un nuevo rol.
        /// </summary>
        /// <param name="rolDto">Datos del rol a crear (nombre y descripcion)</param>
        /// <returns>El rol creado (nombre y descripcion)</returns>
        /// <response code="200">Rol creado correctamente</response>
        /// <response code="400">Ya existe un rol con ese nombre</response>
        [HttpPost]
        [ProducesResponseType(typeof(RolDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CrearRol([FromBody] RolDto rolDto)
        {
            try
            {
                var result = await _createRolUseCase.ExecuteAsync(rolDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Edita un rol existente.
        /// </summary>
        /// <param name="id">Id del rol a editar</param>
        /// <param name="rolUpdateDto">Datos nuevos del rol</param>
        /// <returns>True si se editó correctamente</returns>
        /// <response code="200">Rol editado correctamente</response>
        /// <response code="400">Error de validación</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> EditarRol(string id, [FromBody] RolUpdateDto rolUpdateDto)
        {
            try
            {
                var result = await _updateRolUseCase.ExecuteAsync(id, rolUpdateDto);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un rol existente.
        /// </summary>
        /// <param name="id">Id del rol a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        /// <response code="200">Rol eliminado correctamente</response>
        /// <response code="400">Error de validación</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> EliminarRol(string id)
        {
            try
            {
                var result = await _deleteRolUseCase.ExecuteAsync(id);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un rol por su id.
        /// </summary>
        /// <param name="id">Id del rol</param>
        /// <returns>El rol encontrado (id, nombre, descripcion, permisos)</returns>
        /// <response code="200">Rol encontrado</response>
        /// <response code="404">Rol no encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolListDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ObtenerRolPorId(string id)
        {
            var result = await _getRolByIdUseCase.ExecuteAsync(id);
            if (result == null)
                return NotFound(new { mensaje = "Rol no encontrado." });
            return Ok(result);
        }

        /// <summary>
        /// Obtiene todos los roles.
        /// </summary>
        /// <returns>Lista de roles</returns>
        /// <response code="200">Lista de roles</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<RolListDto>), 200)]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _getAllRolesUseCase.ExecuteAsync();
            return Ok(result);
        }
    }
} 