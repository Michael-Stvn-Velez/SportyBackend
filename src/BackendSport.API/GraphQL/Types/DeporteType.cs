using BackendSport.Domain.Entities.DeporteEntities;

namespace BackendSport.API.GraphQL.Types;

/// <summary>
/// Tipo GraphQL para la entidad Deporte
/// </summary>
public class DeporteType : ObjectType<Deporte>
{
    protected override void Configure(IObjectTypeDescriptor<Deporte> descriptor)
    {
        descriptor.Description("Representa un deporte en el sistema");

        descriptor
            .Field(d => d.Id)
            .Description("Identificador único del deporte");

        descriptor
            .Field(d => d.Name)
            .Description("Nombre del deporte");

        descriptor
            .Field(d => d.Modalities)
            .Description("Modalidades disponibles para el deporte");

        descriptor
            .Field(d => d.Surfaces)
            .Description("Superficies de juego para el deporte");

        descriptor
            .Field(d => d.Positions)
            .Description("Posiciones disponibles en el deporte");

        descriptor
            .Field(d => d.Statistics)
            .Description("Estadísticas que se pueden medir en el deporte");

        descriptor
            .Field(d => d.PerformanceMetrics)
            .Description("Métricas de rendimiento para el deporte");

        descriptor
            .Field(d => d.EvaluationTypes)
            .Description("Tipos de evaluación disponibles");

        descriptor
            .Field(d => d.Formations)
            .Description("Formaciones tácticas del deporte");

        descriptor
            .Field(d => d.CompetitiveLevel)
            .Description("Niveles competitivos disponibles");
    }
}
