using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.API.GraphQL.Types;

/// <summary>
/// Tipo GraphQL para la entidad User
/// </summary>
public class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        descriptor.Description("Representa un usuario del sistema deportivo");

        descriptor
            .Field(u => u.Id)
            .Description("Identificador único del usuario");

        descriptor
            .Field(u => u.Name)
            .Description("Nombre completo del usuario");

        descriptor
            .Field(u => u.Email)
            .Description("Dirección de correo electrónico del usuario");

        descriptor
            .Field(u => u.DocumentTypeId)
            .Description("ID del tipo de documento");

        descriptor
            .Field(u => u.DocumentNumber)
            .Description("Número de documento de identidad");

        descriptor
            .Field(u => u.CountryId)
            .Description("ID del país de residencia");

        descriptor
            .Field(u => u.DepartmentId)
            .Description("ID del departamento/estado");

        descriptor
            .Field(u => u.MunicipalityId)
            .Description("ID del municipio");

        descriptor
            .Field(u => u.CityId)
            .Description("ID de la ciudad");

        descriptor
            .Field(u => u.LocalityId)
            .Description("ID de la localidad");

        descriptor
            .Field(u => u.RolIds)
            .Description("Lista de IDs de roles asignados al usuario");

        descriptor
            .Field(u => u.Sports)
            .Description("Lista de deportes configurados para el usuario");

        descriptor
            .Field(u => u.CreatedAt)
            .Description("Fecha de creación del usuario");

        descriptor
            .Field(u => u.UpdatedAt)
            .Description("Fecha de última actualización");

        // Ocultar campos sensibles
        descriptor
            .Field(u => u.Password)
            .Ignore();
    }
}
