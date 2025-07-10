using BackendSport.Application.DTOs.DeporteDTOs;
using BackendSport.Application.UseCases.DeporteUseCases;
using BackendSport.API.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackendSport.API.Controllers.DeporteControllers
{
    /// <summary>
    /// Controlador para gestionar deportes
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [RequirePermission("administrar_deportes")]
    public class DeporteController : ControllerBase
    {
        private readonly CreateDeporteUseCase _createUseCase;
        private readonly GetAllDeportesUseCase _getAllUseCase;
        private readonly GetDeporteByIdUseCase _getByIdUseCase;
        private readonly UpdateDeporteUseCase _updateUseCase;
        private readonly DeleteDeporteUseCase _deleteUseCase;

        public DeporteController(
            CreateDeporteUseCase createUseCase,
            GetAllDeportesUseCase getAllUseCase,
            GetDeporteByIdUseCase getByIdUseCase,
            UpdateDeporteUseCase updateUseCase,
            DeleteDeporteUseCase deleteUseCase)
        {
            _createUseCase = createUseCase;
            _getAllUseCase = getAllUseCase;
            _getByIdUseCase = getByIdUseCase;
            _updateUseCase = updateUseCase;
            _deleteUseCase = deleteUseCase;
        }

        /// <summary>
        /// Obtiene todos los deportes.
        /// </summary>
        /// <returns>Lista de deportes</returns>
        /// <response code="200">Lista de deportes obtenida correctamente</response>
        /// <response code="400">Error al obtener los deportes</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<DeporteListDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _getAllUseCase.ExecuteAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un deporte por su id.
        /// </summary>
        /// <param name="id">Id del deporte</param>
        /// <returns>El deporte encontrado</returns>
        /// <response code="200">Deporte encontrado</response>
        /// <response code="400">Error al obtener el deporte</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DeporteListDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _getByIdUseCase.ExecuteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo deporte.
        /// </summary>
        /// <param name="dto">Datos del deporte a crear</param>
        /// <returns>El deporte creado</returns>
        /// <response code="200">Deporte creado correctamente</response>
        /// <response code="400">Error de validación o deporte duplicado</response>
        [HttpPost]
        [ProducesResponseType(typeof(DeporteDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] DeporteDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    return BadRequest(new { mensaje = "El nombre es obligatorio." });
                var result = await _createUseCase.ExecuteAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un deporte existente.
        /// </summary>
        /// <param name="id">Id del deporte a actualizar</param>
        /// <param name="dto">Datos nuevos del deporte</param>
        /// <returns>El deporte actualizado</returns>
        /// <response code="200">Deporte actualizado correctamente</response>
        /// <response code="400">Error de validación o deporte duplicado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DeporteDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(string id, [FromBody] DeporteUpdateDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    return BadRequest(new { mensaje = "El nombre es obligatorio." });
                var result = await _updateUseCase.ExecuteAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un deporte existente.
        /// </summary>
        /// <param name="id">Id del deporte a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        /// <response code="200">Deporte eliminado correctamente</response>
        /// <response code="400">Error al eliminar el deporte</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var success = await _deleteUseCase.ExecuteAsync(id);
                return Ok(success);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{deporteId}/user-config")]
        public async Task<IActionResult> GetDeporteConfigOptions(string deporteId, [FromServices] GetDeporteConfigOptionsUseCase getDeporteConfigOptionsUseCase)
        {
            try
            {
                var options = await getDeporteConfigOptionsUseCase.ExecuteAsync(deporteId);
                return Ok(options);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
} 