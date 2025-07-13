# Documentación GraphQL - SportyBackend

## Introducción

SportyBackend ahora incluye una API GraphQL completa que coexiste con la API REST tradicional. GraphQL proporciona una manera más flexible y eficiente de consultar datos, permitiendo a los clientes solicitar exactamente la información que necesitan.

## Configuración

### Dependencias añadidas
```xml
<PackageReference Include="HotChocolate.AspNetCore" Version="13.9.0" />
<PackageReference Include="HotChocolate.AspNetCore.Authorization" Version="13.9.0" />
<PackageReference Include="HotChocolate.Subscriptions" Version="13.9.0" />
<PackageReference Include="HotChocolate.Data" Version="13.9.0" />
<PackageReference Include="HotChocolate.Types.Analyzers" Version="13.9.0" />
```

### Endpoints disponibles

- **GraphQL Endpoint**: `/graphql`
- **GraphQL UI**: `/graphql-ui` (solo en desarrollo)
- **Esquema SDL**: `/graphql-ui/schema.graphql` (solo en desarrollo)

## Estructura del proyecto GraphQL

```
src/BackendSport.API/GraphQL/
├── Types/                     # Definiciones de tipos GraphQL
│   ├── UserType.cs           # Tipo para usuarios
│   ├── DeporteType.cs        # Tipo para deportes
│   ├── RolType.cs            # Tipo para roles
│   ├── PermisosType.cs       # Tipo para permisos
│   └── LocationTypes.cs      # Tipos para ubicaciones
├── Queries/                   # Consultas de solo lectura
│   ├── UserQueries.cs        # Consultas de usuarios
│   ├── DeporteQueries.cs     # Consultas de deportes
│   ├── RolQueries.cs         # Consultas de roles
│   ├── PermisosQueries.cs    # Consultas de permisos
│   ├── LocationQueries.cs    # Consultas de ubicaciones
│   └── SecurityQueries.cs    # Consultas relacionadas con seguridad
├── Mutations/                 # Operaciones de escritura
│   ├── UserMutations.cs      # Mutaciones de usuarios
│   ├── DeporteMutations.cs   # Mutaciones de deportes
│   └── RolMutations.cs       # Mutaciones de roles
├── Subscriptions/             # Eventos en tiempo real
│   ├── UserSubscriptions.cs  # Suscripciones de usuarios
│   ├── DeporteSubscriptions.cs # Suscripciones de deportes
│   └── SystemSubscriptions.cs  # Notificaciones del sistema
└── DTOs/                      # Tipos de entrada específicos de GraphQL
    ├── CreateUserDto.cs      # DTO para crear usuarios
    ├── AsignarRolUsuarioDto.cs # DTO para asignar roles
    └── AsignarDeporteUsuarioDto.cs # DTO para asignar deportes
```

## Tipos principales

### User
```graphql
type User {
  id: String!
  email: String!
  nombre: String!
  apellido: String!
  fechaCreacion: DateTime!
  fechaActualizacion: DateTime
  roles: [Rol!]!
  deportes: [Deporte!]!
}
```

### Deporte
```graphql
type Deporte {
  id: String!
  nombre: String!
  descripcion: String
  fechaCreacion: DateTime!
  fechaActualizacion: DateTime
}
```

### Rol
```graphql
type Rol {
  id: String!
  nombre: String!
  descripcion: String
  fechaCreacion: DateTime!
  fechaActualizacion: DateTime
  permisos: [Permisos!]!
}
```

## Operaciones disponibles

### Queries (Consultas)

#### Usuarios
- `getUserById(id: String!): User` - Obtener usuario por ID
- `getUserByEmail(email: String!): User` - Obtener usuario por email

#### Deportes
- `getDeportes: [Deporte!]!` - Obtener todos los deportes
- `getDeporteById(id: String!): Deporte` - Obtener deporte por ID
- `searchDeportes(searchTerm: String!): [Deporte!]!` - Buscar deportes

#### Roles y Permisos
- `getRoles: [Rol!]!` - Obtener todos los roles
- `getRolById(id: String!): Rol` - Obtener rol por ID
- `getPermissions: [Permisos!]!` - Obtener todos los permisos

#### Ubicaciones
- `getCountries: [Country!]!` - Obtener países
- `getDepartmentsByCountry(countryId: String!): [Department!]!` - Obtener departamentos por país

### Mutations (Mutaciones)

#### Gestión de usuarios
- `createUser(input: CreateUserDto!): UserResponseDto!` - Crear nuevo usuario
- `assignRoleToUser(input: AsignarRolUsuarioDto!): Boolean!` - Asignar rol a usuario
- `assignSportToUser(input: AsignarDeporteUsuarioDto!): Boolean!` - Asignar deporte a usuario

#### Gestión de deportes
- `createDeporte(input: DeporteDto!): DeporteListDto!` - Crear nuevo deporte
- `updateDeporte(id: String!, input: DeporteUpdateDto!): DeporteListDto!` - Actualizar deporte
- `deleteDeporte(id: String!): Boolean!` - Eliminar deporte

### Subscriptions (Suscripciones)

#### Eventos de usuarios
- `onUserChanged: User!` - Cambios en usuarios
- `onUserCreated: User!` - Nuevos usuarios creados

#### Eventos de deportes
- `onDeporteChanged: Deporte!` - Cambios en deportes

#### Notificaciones del sistema
- `onSystemNotification: SystemNotification!` - Notificaciones generales

## Ejemplos de uso

### Consulta básica
```graphql
query {
  getDeportes {
    id
    nombre
    descripcion
    fechaCreacion
  }
}
```

### Consulta con argumentos
```graphql
query {
  getUserByEmail(email: "admin@ejemplo.com") {
    id
    nombre
    apellido
    roles {
      nombre
      permisos {
        nombre
      }
    }
  }
}
```

### Mutación para crear deporte
```graphql
mutation {
  createDeporte(input: {
    nombre: "Tenis"
    descripcion: "Deporte de raqueta"
  }) {
    id
    nombre
    descripcion
    fechaCreacion
  }
}
```

### Suscripción para cambios de usuario
```graphql
subscription {
  onUserChanged {
    id
    email
    nombre
    apellido
  }
}
```

## Seguridad

### Autenticación
GraphQL utiliza el mismo sistema de autenticación JWT que la API REST:

```graphql
# Headers requeridos para operaciones protegidas
{
  "Authorization": "Bearer YOUR_JWT_TOKEN"
}
```

### Autorización
Las consultas y mutaciones están protegidas según los permisos del usuario:

- Queries públicas: `getDeportes`, `getCountries`
- Queries protegidas: `getUserById`, `getUserByEmail`
- Mutations: Requieren autenticación y permisos específicos

### Middleware de seguridad
- Rate limiting aplicado
- Sanitización de entrada
- Headers de seguridad
- Filtrado de errores en producción

## Herramientas de desarrollo

### GraphQL UI personalizada
Interfaz web personalizada disponible en `/graphql-ui` que incluye:

- ✅ Editor de consultas con sintaxis highlighting
- ✅ Ejemplos precargados para todas las operaciones
- ✅ Información de autenticación
- ✅ Descarga del esquema SDL
- ✅ Documentación integrada
- ✅ Diseño moderno y responsive

### Introspección
En modo desarrollo, la introspección está habilitada para:
- Exploración automática del esquema
- Autocompletado en herramientas como GraphiQL
- Generación de documentación automática

## Configuración avanzada

### WebSockets para suscripciones
```csharp
// Configurado automáticamente en GraphQLExtensions.cs
builder.Services.AddGraphQLServer()
    .AddSubscriptionType<SystemSubscriptions>()
    .AddInMemorySubscriptions();

// En Program.cs
app.UseWebSockets();
```

### Filtros y ordenamiento
```csharp
// Habilitado automáticamente para queries
builder.Services.AddGraphQLServer()
    .AddFiltering()
    .AddSorting();
```

### Autorización
```csharp
// Integración con JWT
builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddAuthorizationHandler<PermissionRequirementHandler>();
```

## Rendimiento

### DataLoader (planeado)
Para optimizar consultas N+1, se puede implementar DataLoader en el futuro:

```csharp
// Ejemplo de DataLoader para roles
public async Task<Rol[]> GetRolesByUserIdsAsync(
    IReadOnlyList<string> userIds,
    CancellationToken cancellationToken)
{
    // Implementación optimizada
}
```

### Caching
GraphQL se beneficia del caching HTTP estándar y se puede extender con:
- Persisted queries
- Response caching
- DataLoader caching

## Migración desde REST

### Ventajas de GraphQL
- **Flexibilidad**: Los clientes solicitan exactamente los datos necesarios
- **Tipado fuerte**: Esquema autodocumentado y validado
- **Tiempo real**: Suscripciones nativas para eventos en vivo
- **Versionado**: Un solo endpoint sin versiones de API
- **Herramientas**: Ecosistema rico de desarrollo y testing

### Coexistencia
GraphQL y REST pueden usarse simultáneamente:
- REST para operaciones simples y cacheo agresivo
- GraphQL para consultas complejas y tiempo real
- Misma lógica de negocio compartida en Application layer

## Troubleshooting

### Errores comunes

1. **Error de autorización**
   ```
   No autorizado para acceder a este recurso
   ```
   - Verificar token JWT válido
   - Confirmar permisos del usuario

2. **Error de sintaxis GraphQL**
   ```
   Syntax Error: Expected Name, found }
   ```
   - Validar sintaxis de consulta
   - Usar GraphQL UI para autocompletado

3. **Campo no encontrado**
   ```
   Cannot query field "fieldName" on type "TypeName"
   ```
   - Verificar esquema actualizado
   - Confirmar que el campo existe en el tipo

### Logs de desarrollo
En desarrollo, los logs incluyen:
- Consultas GraphQL ejecutadas
- Tiempo de respuesta
- Errores de validación
- Información de autorización

### Monitoreo en producción
- Métricas de rendimiento
- Análisis de consultas
- Detección de consultas maliciosas
- Rate limiting por usuario

---

Para más información, consulta la [documentación oficial de HotChocolate](https://chillicream.com/docs/hotchocolate).
