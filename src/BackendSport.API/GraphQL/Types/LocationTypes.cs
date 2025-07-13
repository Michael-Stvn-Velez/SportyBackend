using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.API.GraphQL.Types;

/// <summary>
/// Tipo GraphQL para la entidad Country
/// </summary>
public class CountryType : ObjectType<Country>
{
    protected override void Configure(IObjectTypeDescriptor<Country> descriptor)
    {
        descriptor.Description("Representa un país en el sistema");

        descriptor
            .Field(c => c.Id)
            .Description("Identificador único del país");

        descriptor
            .Field(c => c.Name)
            .Description("Nombre del país");

        descriptor
            .Field(c => c.Code)
            .Description("Código ISO del país");

        descriptor
            .Field(c => c.IsActive)
            .Description("Indica si el país está activo");
    }
}

/// <summary>
/// Tipo GraphQL para la entidad Department
/// </summary>
public class DepartmentType : ObjectType<Department>
{
    protected override void Configure(IObjectTypeDescriptor<Department> descriptor)
    {
        descriptor.Description("Representa un departamento/estado");

        descriptor
            .Field(d => d.Id)
            .Description("Identificador único del departamento");

        descriptor
            .Field(d => d.Name)
            .Description("Nombre del departamento");

        descriptor
            .Field(d => d.Code)
            .Description("Código del departamento");

        descriptor
            .Field(d => d.CountryId)
            .Description("ID del país al que pertenece");

        descriptor
            .Field(d => d.IsActive)
            .Description("Indica si el departamento está activo");
    }
}

/// <summary>
/// Tipo GraphQL para la entidad Municipality
/// </summary>
public class MunicipalityType : ObjectType<Municipality>
{
    protected override void Configure(IObjectTypeDescriptor<Municipality> descriptor)
    {
        descriptor.Description("Representa un municipio");

        descriptor
            .Field(m => m.Id)
            .Description("Identificador único del municipio");

        descriptor
            .Field(m => m.Name)
            .Description("Nombre del municipio");

        descriptor
            .Field(m => m.Code)
            .Description("Código del municipio");

        descriptor
            .Field(m => m.DepartmentId)
            .Description("ID del departamento al que pertenece");

        descriptor
            .Field(m => m.Type)
            .Description("Tipo de municipio");

        descriptor
            .Field(m => m.IsCapital)
            .Description("Indica si es capital del departamento");

        descriptor
            .Field(m => m.IsActive)
            .Description("Indica si el municipio está activo");
    }
}

/// <summary>
/// Tipo GraphQL para la entidad DocumentType
/// </summary>
public class DocumentTypeType : ObjectType<DocumentType>
{
    protected override void Configure(IObjectTypeDescriptor<DocumentType> descriptor)
    {
        descriptor.Description("Representa un tipo de documento");

        descriptor
            .Field(dt => dt.Id)
            .Description("Identificador único del tipo de documento");

        descriptor
            .Field(dt => dt.Name)
            .Description("Nombre del tipo de documento");

        descriptor
            .Field(dt => dt.Code)
            .Description("Código del tipo de documento");

        descriptor
            .Field(dt => dt.CountryId)
            .Description("ID del país al que pertenece");

        descriptor
            .Field(dt => dt.IsActive)
            .Description("Indica si el tipo de documento está activo");
    }
}
