using BackendSport.Domain.Entities.RolEntities;

namespace BackendSport.API.GraphQL.Types;

/// <summary>
/// Tipo GraphQL para la entidad Rol
/// </summary>
public class RolType : ObjectType<Rol>
{
    protected override void Configure(IObjectTypeDescriptor<Rol> descriptor)
    {
        descriptor.Description("Representa un rol en el sistema de permisos");

        descriptor
            .Field(r => r.Id)
            .Description("Identificador único del rol");

        descriptor
            .Field(r => r.Nombre)
            .Description("Nombre del rol");

        descriptor
            .Field(r => r.Descripcion)
            .Description("Descripción detallada del rol");

        descriptor
            .Field(r => r.Permisos)
            .Description("Lista de permisos asignados al rol");
    }
}
