using BackendSport.Application.UseCases.DeporteUseCases;
using BackendSport.Application.DTOs.DeporteDTOs;
using HotChocolate.Authorization;

namespace BackendSport.API.GraphQL.Mutations;

/// <summary>
/// Mutaciones GraphQL para deportes
/// </summary>
[ExtendObjectType("Mutation")]
public class DeporteMutations
{
    /// <summary>
    /// Crea un nuevo deporte
    /// </summary>
    /// <param name="input">Datos del deporte a crear</param>
    /// <param name="createDeporteUseCase">Caso de uso para crear deportes</param>
    /// <returns>Deporte creado</returns>
    [Authorize]
    public async Task<DeporteListDto> CreateDeporteAsync(
        DeporteDto input,
        [Service] CreateDeporteUseCase createDeporteUseCase)
    {
        return await createDeporteUseCase.ExecuteAsync(input);
    }

    /// <summary>
    /// Actualiza un deporte existente
    /// </summary>
    /// <param name="id">ID del deporte a actualizar</param>
    /// <param name="input">Nuevos datos del deporte</param>
    /// <param name="updateDeporteUseCase">Caso de uso para actualizar deportes</param>
    /// <returns>Deporte actualizado</returns>
    [Authorize]
    public async Task<DeporteListDto> UpdateDeporteAsync(
        string id,
        DeporteUpdateDto input,
        [Service] UpdateDeporteUseCase updateDeporteUseCase)
    {
        return await updateDeporteUseCase.ExecuteAsync(id, input);
    }

    /// <summary>
    /// Elimina un deporte
    /// </summary>
    /// <param name="id">ID del deporte a eliminar</param>
    /// <param name="deleteDeporteUseCase">Caso de uso para eliminar deportes</param>
    /// <returns>Resultado de la operación</returns>
    [Authorize]
    public async Task<bool> DeleteDeporteAsync(
        string id,
        [Service] DeleteDeporteUseCase deleteDeporteUseCase)
    {
        await deleteDeporteUseCase.ExecuteAsync(id);
        return true;
    }
}
