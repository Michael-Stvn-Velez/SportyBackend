using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BackendSport.API.Controllers;

/// <summary>
/// Controlador para la interfaz de usuario GraphQL Explorer
/// NOTA: /graphql = endpoint de datos | /graphql-ui = interfaz gráfica
/// </summary>
[ApiController]
[Route("graphql-ui")]
public class GraphQLUIController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public GraphQLUIController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    /// <summary>
    /// Sirve la interfaz GraphQL Explorer (solo en desarrollo)
    /// Acceso: /graphql-ui (interfaz gráfica) vs /graphql (endpoint de datos)
    /// </summary>
    /// <returns>Página HTML de la interfaz GraphQL</returns>
    [HttpGet]
    public async Task<IActionResult> GetGraphQLUI()
    {
        if (!_environment.IsDevelopment())
        {
            return NotFound("GraphQL UI está disponible solo en desarrollo");
        }

        var filePath = System.IO.Path.Combine(_environment.WebRootPath, "graphql-explorer.html");
        
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("Interfaz GraphQL no encontrada");
        }

        var html = await System.IO.File.ReadAllTextAsync(filePath);
        return Content(html, "text/html");
    }

    /// <summary>
    /// Obtiene información sobre el estado de GraphQL
    /// </summary>
    /// <returns>Información del estado</returns>
    [HttpGet("info")]
    public IActionResult GetGraphQLInfo()
    {
        var info = new
        {
            Status = "Active",
            Environment = _environment.EnvironmentName,
            Endpoint = "/graphql",
            WebSocketEndpoint = "/graphql",
            UIEndpoint = _environment.IsDevelopment() ? "/graphql/ui" : null,
            Introspection = _environment.IsDevelopment(),
            Features = new[]
            {
                "Queries",
                "Mutations", 
                "Subscriptions",
                "Authorization",
                "Filtering",
                "Sorting",
                "Projections"
            },
            Documentation = new
            {
                Integration = "/docs/GraphQL-Integration.md",
                Summary = "/docs/GraphQL-Implementation-Summary.md"
            },
            Examples = new
            {
                SimpleQuery = new
                {
                    Query = "{ getUserById(id: \"user-123\") { id name email } }",
                    Variables = new { },
                    Headers = new { Authorization = "Bearer {your-jwt-token}" }
                },
                SearchQuery = new
                {
                    Query = "query SearchSports($term: String!) { searchDeportes(searchTerm: $term) { id name positions } }",
                    Variables = new { term = "futbol" },
                    Headers = new { Authorization = "Bearer {your-jwt-token}" }
                },
                Mutation = new
                {
                    Query = "mutation CreateUser($input: CreateUserDto!) { createUser(input: $input) { id name email } }",
                    Variables = new { 
                        input = new {
                            name = "Juan Pérez",
                            email = "juan@example.com",
                            password = "password123",
                            documentTypeId = "doc-type-id",
                            documentNumber = "12345678",
                            countryId = "country-id"
                        }
                    },
                    Headers = new { Authorization = "Bearer {your-jwt-token}" }
                },
                Subscription = new
                {
                    Query = "subscription { onUserChanged { id name email updatedAt } }",
                    Variables = new { },
                    Headers = new { Authorization = "Bearer {your-jwt-token}" }
                }
            }
        };

        return Ok(info);
    }

    /// <summary>
    /// Descarga el esquema GraphQL en formato SDL
    /// </summary>
    /// <returns>Esquema en formato SDL</returns>
    [HttpGet("schema.graphql")]
    public IActionResult GetSchema()
    {
        if (!_environment.IsDevelopment())
        {
            return NotFound("Esquema disponible solo en desarrollo");
        }

        try
        {
            // Esquema simplificado para descarga
            var sdlContent = @"# SportyBackend GraphQL Schema
# Generado automáticamente

type Query {
  getUserById(id: String!): User
  getUserByEmail(email: String!): User
  getDeportes: [Deporte!]!
  getDeporteById(id: String!): Deporte
  searchDeportes(searchTerm: String!): [Deporte!]!
  getRoles: [Rol!]!
  getRolById(id: String!): Rol
  getPermissions: [Permisos!]!
  getCountries: [Country!]!
  getDepartmentsByCountry(countryId: String!): [Department!]!
}

type Mutation {
  createUser(input: CreateUserDto!): UserResponseDto!
  assignRoleToUser(input: AsignarRolUsuarioDto!): Boolean!
  assignSportToUser(input: AsignarDeporteUsuarioDto!): Boolean!
  createDeporte(input: DeporteDto!): DeporteListDto!
  updateDeporte(id: String!, input: DeporteUpdateDto!): DeporteListDto!
  deleteDeporte(id: String!): Boolean!
}

type Subscription {
  onUserChanged: User!
  onUserCreated: User!
  onDeporteChanged: Deporte!
  onSystemNotification: SystemNotification!
}

# Esquema completo disponible vía introspección en /graphql
";

            return Content(sdlContent, "text/plain", System.Text.Encoding.UTF8);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al generar esquema: {ex.Message}");
        }
    }
}
