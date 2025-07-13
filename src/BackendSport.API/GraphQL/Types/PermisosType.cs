using BackendSport.Domain.Entities.PermisosEntities;

namespace BackendSport.API.GraphQL.Types;

/// <summary>
/// Tipo GraphQL para la entidad Permisos
/// </summary>
public class PermisosType : ObjectType<Permisos>
{
    protected override void Configure(IObjectTypeDescriptor<Permisos> descriptor)
    {
        descriptor.Description("Representa un permiso en el sistema");

        descriptor
            .Field(p => p.Id)
            .Description("Identificador único del permiso");

        descriptor
            .Field(p => p.Nombre)
            .Description("Nombre del permiso");

        descriptor
            .Field(p => p.Descripcion)
            .Description("Descripción detallada del permiso");
    }
}
