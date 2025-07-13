# 🚀 GraphQL Integration - SportyBackend API

## 📋 **Resumen de la Implementación**

Se ha integrado **GraphQL** como una API alternativa complementaria a los endpoints REST existentes, proporcionando una interfaz más flexible y eficiente para consultas de datos.

---

## 🎯 **Características Implementadas**

### **✅ Esquema GraphQL Completo**
- **Queries**: Consultas para usuarios, deportes, roles, permisos y ubicaciones
- **Mutations**: Operaciones de creación, actualización y eliminación
- **Subscriptions**: Eventos en tiempo real para cambios del sistema
- **Types**: Tipos fuertemente tipados para todas las entidades

### **✅ Seguridad Integrada**
- **Autenticación JWT**: Reutiliza el sistema de autenticación existente
- **Autorización**: Control de permisos a nivel de campo y operación
- **Filtros de Error**: Manejo personalizado de errores y excepciones
- **Limitación de Complejidad**: Protección contra consultas maliciosas

### **✅ Funcionalidades Avanzadas**
- **Filtering**: Filtrado dinámico de resultados
- **Sorting**: Ordenamiento flexible de datos
- **Projections**: Selección eficiente de campos
- **Subscriptions**: Notificaciones en tiempo real

---

## 🌐 **Endpoints Disponibles**

### **GraphQL Principal**
```
POST /graphql
GET  /graphql (para introspección)
```

### **GraphQL Playground** (Solo Desarrollo)
```
GET /graphql/playground
```

### **WebSocket** (Para Subscriptions)
```
WS /graphql (con subprotocolo graphql-ws)
```

---

## 📖 **Ejemplos de Uso**

### **Query: Obtener Usuarios**
```graphql
query GetUsers {
  users {
    id
    name
    email
    sports {
      sportId
      positions
      level
    }
    createdAt
  }
}
```

### **Query: Buscar Deportes**
```graphql
query SearchSports($searchTerm: String!) {
  searchDeportes(searchTerm: $searchTerm) {
    id
    name
    positions
    modalities
    competitiveLevel
  }
}
```

### **Query: Obtener Jerarquía de Ubicaciones**
```graphql
query GetLocationHierarchy($countryId: String!) {
  departmentsByCountry(countryId: $countryId) {
    id
    name
    code
  }
  documentTypesByCountry(countryId: $countryId) {
    id
    name
    code
  }
}
```

### **Mutation: Crear Usuario**
```graphql
mutation CreateUser($input: CreateUserDto!) {
  createUser(input: $input) {
    id
    name
    email
    createdAt
  }
}
```

### **Variables para Mutation**
```json
{
  "input": {
    "name": "Juan Pérez",
    "email": "juan@example.com",
    "password": "password123",
    "documentTypeId": "doc-type-id",
    "documentNumber": "12345678",
    "countryId": "country-id"
  }
}
```

### **Subscription: Cambios en Usuarios**
```graphql
subscription UserChanges {
  onUserChanged {
    id
    name
    email
    updatedAt
  }
}
```

---

## 🔐 **Autenticación en GraphQL**

### **Headers Requeridos**
```http
Authorization: Bearer {tu-jwt-token}
Content-Type: application/json
```

### **Ejemplo de Request**
```http
POST /graphql
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "query": "query { users { id name email } }",
  "variables": {}
}
```

---

## 🛠️ **Configuración de Desarrollo**

### **GraphQL Playground**
En entorno de desarrollo, accede a:
```
https://localhost:5001/graphql/playground
```

### **Características del Playground**
- **Schema Explorer**: Exploración interactiva del esquema
- **Query Builder**: Constructor visual de consultas  
- **Documentation**: Documentación automática de tipos y campos
- **Variables Panel**: Panel para definir variables
- **Headers Panel**: Configuración de headers de autenticación

---

## 📊 **Comparación REST vs GraphQL**

| Aspecto | REST API | GraphQL API |
|---------|----------|-------------|
| **Endpoints** | Múltiples endpoints | Un solo endpoint |
| **Over-fetching** | Datos fijos por endpoint | Solo datos solicitados |
| **Under-fetching** | Múltiples requests | Una sola request |
| **Versionado** | Versionado de URL | Schema evolution |
| **Caching** | HTTP caching | Query-based caching |
| **Real-time** | Polling/WebHooks | Subscriptions nativas |

---

## 🧪 **Testing con GraphQL**

### **Usando cURL**
```bash
curl -X POST https://localhost:5001/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{"query": "{ users { id name } }"}'
```

### **Usando Postman**
1. **Method**: POST
2. **URL**: `https://localhost:5001/graphql`
3. **Headers**: 
   - `Content-Type: application/json`
   - `Authorization: Bearer {token}`
4. **Body** (raw JSON):
```json
{
  "query": "query GetUsers { users { id name email } }",
  "variables": {}
}
```

---

## 🎯 **Mejores Prácticas**

### **✅ Queries Eficientes**
- Solicita solo los campos necesarios
- Usa variables para valores dinámicos
- Implementa paginación cuando sea necesario

### **✅ Manejo de Errores**
```graphql
{
  "data": null,
  "errors": [
    {
      "message": "No tienes permisos para realizar esta operación",
      "extensions": {
        "code": "UNAUTHORIZED"
      }
    }
  ]
}
```

### **✅ Subscriptions**
- Usa subscriptions para datos en tiempo real
- Maneja la conexión WebSocket apropiadamente
- Implementa reconexión automática

---

## 🔧 **Arquitectura Técnica**

### **Tecnologías Utilizadas**
- **HotChocolate 14.1.0**: Framework GraphQL para .NET
- **Schema-First Approach**: Tipos definidos en C#
- **Dependency Injection**: Integración completa con DI de ASP.NET Core
- **Authorization**: Reutilización del sistema de autenticación JWT

### **Estructura de Archivos**
```
GraphQL/
├── Types/              # Definiciones de tipos GraphQL
│   ├── UserType.cs
│   ├── DeporteType.cs
│   ├── RolType.cs
│   ├── PermisosType.cs
│   └── LocationTypes.cs
├── Queries/            # Consultas GraphQL
│   ├── UserQueries.cs
│   ├── DeporteQueries.cs
│   ├── SecurityQueries.cs
│   └── LocationQueries.cs
├── Mutations/          # Mutaciones GraphQL
│   ├── UserMutations.cs
│   ├── DeporteMutations.cs
│   └── SecurityMutations.cs
└── Subscriptions/      # Suscripciones GraphQL
    └── SystemSubscriptions.cs
```

---

## 🚀 **Próximos Pasos**

### **Mejoras Futuras**
- [ ] **DataLoader**: Optimización de consultas N+1
- [ ] **Pagination**: Implementación de cursor-based pagination
- [ ] **File Upload**: Soporte para carga de archivos
- [ ] **Batching**: Agrupación de requests
- [ ] **Persisted Queries**: Cache de consultas frecuentes
- [ ] **Schema Stitching**: Federación con otros servicios GraphQL

### **Monitoreo y Analytics**
- [ ] **Query Analytics**: Métricas de uso de consultas
- [ ] **Performance Monitoring**: Monitoreo de rendimiento
- [ ] **Error Tracking**: Seguimiento detallado de errores

---

## 📚 **Recursos Adicionales**

- [HotChocolate Documentation](https://chillicream.com/docs/hotchocolate/v14)
- [GraphQL Best Practices](https://graphql.org/learn/best-practices/)
- [GraphQL Playground Guide](https://github.com/graphql/graphql-playground)

---

**¡GraphQL está listo para usar junto con la API REST existente!** 🎉
