# BackendSport API

API para la gestión de usuarios, roles y permisos, desarrollada con Clean Architecture y MongoDB. Ahora incluye soporte completo para **GraphQL** además de REST.

## Características principales

- ✅ **API REST** tradicional con Swagger UI
- 🆕 **GraphQL** con interfaz de exploración personalizada
- 🔐 **Autenticación JWT** integrada
- 🏗️ **Clean Architecture** con capas bien definidas
- 🍃 **MongoDB** como base de datos
- 📊 **Suscripciones en tiempo real** con GraphQL

## Requisitos
- .NET 9 SDK o superior
- MongoDB (local o remoto)

## Pasos para clonar, arrancar e inicializar el proyecto

### 1. Clonar el repositorio
```bash
git clone <URL_DEL_REPOSITORIO>
cd SportyBackend
```

### 2. Configuración inicial
- Copia los archivos de ejemplo:
  - `src/BackendSport.API/appsettings.example.json` → `src/BackendSport.API/appsettings.json`
  - `src/BackendSport.API/Properties/launchSettings.example.json` → `src/BackendSport.API/Properties/launchSettings.json`
- Edita `appsettings.json` para configurar la cadena de conexión de MongoDB y otros parámetros necesarios.

### 3. Restaurar dependencias y compilar
```powershell
dotnet restore
```
```powershell
dotnet build
```

### 4. Ejecutar la API
- Desde Visual Studio: selecciona el perfil `BackendSport.API` y ejecuta.
- Desde terminal:
```powershell
dotnet run --project src/BackendSport.API/BackendSport.API.csproj
```

### 5. Acceder a la documentación y herramientas

#### REST API (Swagger)
- Abre tu navegador en:
  - http://localhost:5000/
  - o https://localhost:5001/

Swagger UI estará disponible en la raíz del proyecto.

#### GraphQL
- **Playground GraphQL personalizado**: 
  - http://localhost:5000/graphql-ui
  - o https://localhost:5001/graphql-ui
- **Endpoint GraphQL**: 
  - http://localhost:5000/graphql
  - o https://localhost:5001/graphql
- **Esquema SDL**: 
  - http://localhost:5000/graphql-ui/schema.graphql (solo en desarrollo)

### 6. Usando GraphQL

#### Consultas de ejemplo:
```graphql
# Obtener todos los deportes
query {
  getDeportes {
    id
    nombre
    descripcion
    fechaCreacion
  }
}

# Buscar deportes
query {
  searchDeportes(searchTerm: "fútbol") {
    id
    nombre
    descripcion
  }
}

# Obtener usuario por email
query {
  getUserByEmail(email: "usuario@ejemplo.com") {
    id
    email
    nombre
    apellido
    roles {
      id
      nombre
    }
  }
}
```

#### Mutaciones de ejemplo:
```graphql
# Crear nuevo deporte
mutation {
  createDeporte(input: {
    nombre: "Baloncesto"
    descripcion: "Deporte de equipo jugado en cancha"
  }) {
    id
    nombre
    descripcion
    fechaCreacion
  }
}

# Asignar rol a usuario
mutation {
  assignRoleToUser(input: {
    usuarioId: "user-id-here"
    rolId: "role-id-here"
  })
}
```

#### Suscripciones de ejemplo:
```graphql
# Escuchar cambios en usuarios
subscription {
  onUserChanged {
    id
    email
    nombre
    apellido
  }
}

# Escuchar notificaciones del sistema
subscription {
  onSystemNotification {
    message
    type
    timestamp
  }
}
```

## Notas
- El entorno de desarrollo está configurado en `launchSettings.json`.
- No subas archivos de configuración sensibles al repositorio.
- Usa los archivos `.example.json` como referencia para nuevos entornos.
- **GraphQL UI** está disponible solo en modo desarrollo por seguridad.
- Las suscripciones GraphQL usan WebSockets para comunicación en tiempo real.

## Arquitectura

### Capas del proyecto:
- **BackendSport.API**: Controladores REST y GraphQL, middleware, configuración
- **BackendSport.Application**: Casos de uso, DTOs, interfaces de servicios
- **BackendSport.Domain**: Entidades, servicios de dominio
- **BackendSport.Infrastructure**: Persistencia, servicios externos

### Tecnologías utilizadas:
- **ASP.NET Core 9.0**: Framework web
- **HotChocolate 13.9.0**: Servidor GraphQL
- **MongoDB Driver**: Acceso a base de datos
- **JWT Bearer Authentication**: Autenticación
- **Swagger/OpenAPI**: Documentación REST API

---

¿Dudas? Consulta este README o contacta al equipo de desarrollo.
