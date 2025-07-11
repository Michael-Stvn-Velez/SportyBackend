# ETAPA 1: SEGURIDAD AVANZADA

## 📋 Resumen de Implementación

Esta etapa implementa un sistema completo de seguridad avanzada para la API BackendSport, siguiendo las mejores prácticas de seguridad web y preparando la base para GraphQL.

## 🛡️ Características Implementadas

### 1. **JWT Mejorado con Mejores Prácticas**

#### Configuración Robusta (`JwtSettings.cs`)
- **Clave secreta**: Mínimo 32 caracteres para seguridad
- **Claims personalizados**: Roles, deportes, JWT ID para blacklisting
- **Validación completa**: Emisor, audiencia, tiempo de vida, firma
- **Configuración flexible**: Todos los parámetros configurables

#### TokenService Mejorado (`TokenService.cs`)
- **Generación segura**: HMAC-SHA512, RandomNumberGenerator para refresh tokens
- **Claims enriquecidos**: User ID, email, name, roles, deportes, timestamps
- **Validación robusta**: Métodos para validar y verificar expiración
- **Refresh token rotation**: Tokens criptográficamente seguros

### 2. **Rate Limiting Inteligente**

#### RateLimitingService (`RateLimitingService.cs`)
- **FixedWindowRateLimiter**: Implementación nativa de .NET
- **Thread-safe**: ConcurrentDictionary para almacenamiento en memoria
- **Múltiples niveles**: Por IP, usuario y endpoint específico
- **Configuración granular**: Límites diferenciados por tipo de endpoint

#### RateLimitingMiddleware (`RateLimitingMiddleware.cs`)
- **Detección de IP real**: Soporte para proxies y load balancers
- **Headers estándar**: X-RateLimit-*, Retry-After
- **Logging detallado**: Registro de intentos de ataque
- **Respuestas HTTP 429**: Con información de rate limiting

### 3. **Headers de Seguridad**

#### SecurityHeadersMiddleware (`SecurityHeadersMiddleware.cs`)
- **X-Content-Type-Options**: Previene MIME type sniffing
- **X-Frame-Options**: Protección contra clickjacking
- **X-XSS-Protection**: Protección XSS del navegador
- **Content-Security-Policy**: Política de seguridad de contenido
- **Strict-Transport-Security**: Fuerza HTTPS (solo en HTTPS)
- **Permissions-Policy**: Control de permisos del navegador
- **Eliminación de headers informativos**: Server, X-Powered-By

### 4. **Sanitización de Inputs**

#### InputSanitizationMiddleware (`InputSanitizationMiddleware.cs`)
- **Detección XSS**: Patrones HTML, JavaScript, CSS maliciosos
- **Sanitización de query parameters**: Limpieza automática
- **Verificación de headers**: Headers de proxy sospechosos
- **Logging de ataques**: Registro de intentos de inyección
- **Expresiones regulares**: Patrones completos de detección

### 5. **Autenticación JWT Completa**

#### Configuración en DependencyInjection
- **JWT Bearer**: Configuración completa con validación
- **Eventos personalizados**: Manejo de tokens expirados
- **Respuestas HTTP apropiadas**: 401/403 según el caso
- **Clock skew**: Tolerancia de tiempo configurable

#### JwtAuthorizationAttribute (`JwtAuthorizationAttribute.cs`)
- **Autorización granular**: Roles y permisos específicos
- **Verificación en pipeline**: Antes de llegar a controladores
- **Respuestas estándar**: 401 Unauthorized, 403 Forbidden
- **Integración con permisos**: IPermissionCheckerService

## 🔧 Configuración

### Archivo de Configuración (`appsettings.example.json`)

```json
{
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-with-at-least-32-characters-for-security",
    "Issuer": "SportyBackend",
    "Audience": "SportyBackendUsers",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7,
    "ValidateIssuer": true,
    "ValidateAudience": true,
    "ValidateLifetime": true,
    "ClockSkewMinutes": 5
  },
  "RateLimiting": {
    "GlobalLimit": 100,
    "GlobalWindow": "00:01:00",
    "LoginLimit": 5,
    "LoginWindow": "00:01:00"
  },
  "Security": {
    "EnableHttpsRedirection": true,
    "EnableSecurityHeaders": true,
    "EnableInputSanitization": true,
    "EnableRateLimiting": true
  }
}
```

### Pipeline de Middlewares (`Program.cs`)

```csharp
// Middleware de headers de seguridad (debe ir primero)
app.UseMiddleware<SecurityHeadersMiddleware>();

// Middleware de sanitización de inputs
app.UseMiddleware<InputSanitizationMiddleware>();

// Middleware de rate limiting
app.UseMiddleware<RateLimitingMiddleware>();

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();
```

## 📊 Métricas de Seguridad

### Rate Limiting
- **IP Global**: 100 requests/minuto
- **Login**: 5 requests/minuto (prevención de fuerza bruta)
- **Refresh Token**: 10 requests/minuto
- **Endpoints generales**: 50 requests/minuto
- **Usuario autenticado**: 200 requests/minuto

### Headers de Seguridad
- **CSP**: Política restrictiva con 'self' y HTTPS
- **HSTS**: 1 año con preload y subdominios
- **X-Frame-Options**: DENY (previene clickjacking)
- **X-Content-Type-Options**: nosniff

### JWT
- **Access Token**: 15 minutos (seguridad)
- **Refresh Token**: 7 días (usabilidad)
- **Algoritmo**: HMAC-SHA512 (robusto)
- **Claims**: Enriquecidos con roles y deportes

## 🚀 Uso de las Nuevas Funcionalidades

### 1. **Autorización en Controladores**

```csharp
[ApiController]
[Route("api/[controller]")]
[JwtAuthorization(requiredRoles: new[] { "Admin" })]
public class AdminController : ControllerBase
{
    [HttpGet]
    [JwtAuthorization(requiredPermissions: new[] { "users.read" })]
    public async Task<IActionResult> GetUsers()
    {
        // Solo usuarios con rol Admin y permiso users.read
    }
}
```

### 2. **Rate Limiting Automático**

El rate limiting se aplica automáticamente a todas las solicitudes:
- **Headers de respuesta**: X-RateLimit-Limit, X-RateLimit-Remaining
- **Respuesta 429**: Cuando se excede el límite
- **Logging**: Registro de intentos de ataque

### 3. **Sanitización Automática**

La sanitización se aplica automáticamente:
- **Query parameters**: Limpieza de XSS y HTML malicioso
- **Headers**: Verificación de headers sospechosos
- **Logging**: Registro de intentos de inyección

## 🔍 Monitoreo y Logging

### Eventos Registrados
- **Rate limiting**: Intentos de exceder límites
- **Ataques XSS**: Detección de contenido malicioso
- **Autenticación**: Fallos de autenticación JWT
- **Autorización**: Accesos denegados

### Headers de Respuesta
- **X-RateLimit-***: Información de rate limiting
- **Security Headers**: Headers de seguridad aplicados
- **Token-Expired**: Cuando un token JWT ha expirado

## 🧪 Preparación para GraphQL

### Características Implementadas
- **Autenticación JWT**: Compatible con GraphQL
- **Rate limiting**: Aplicable a queries y mutations
- **Sanitización**: Protección contra ataques en GraphQL
- **Headers de seguridad**: Protección general de la API
- **Autorización granular**: Base para permisos GraphQL

### Próximos Pasos para GraphQL
1. **Directivas de autorización**: Implementar directivas GraphQL
2. **Rate limiting específico**: Por operación GraphQL
3. **Validación de queries**: Protección contra queries complejas
4. **Introspection control**: Control de acceso a schema

## 📈 Beneficios de Seguridad

### Protección Contra Ataques
- **XSS**: Sanitización y CSP
- **CSRF**: Tokens JWT y headers de seguridad
- **Clickjacking**: X-Frame-Options
- **MIME sniffing**: X-Content-Type-Options
- **Fuerza bruta**: Rate limiting por endpoint
- **Token hijacking**: JWT con claims enriquecidos

### Cumplimiento de Estándares
- **OWASP Top 10**: Cobertura de vulnerabilidades principales
- **Security Headers**: Headers de seguridad estándar
- **JWT RFC**: Implementación conforme a estándares
- **Rate Limiting**: Headers RFC 6585

## 🔄 Próximas Etapas

### Etapa 2: Monitoreo y Observabilidad
- Health checks personalizados
- Logging estructurado con Serilog
- Métricas de rendimiento
- Correlación de IDs

### Etapa 3: Testing Completo
- Tests de integración
- Unit tests con mocks
- Tests de rendimiento
- Tests de seguridad

### Etapa 4: Optimizaciones
- Caching multi-nivel
- Compresión de respuestas
- Connection pooling
- Serialización optimizada

## 📝 Notas de Implementación

### Dependencias Agregadas
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `System.Threading.RateLimiting`
- `Microsoft.Extensions.Logging.Abstractions`

### Archivos Modificados
- `Program.cs`: Pipeline de middlewares
- `DependencyInjection.cs`: Configuración JWT
- `appsettings.example.json`: Configuración de seguridad

### Archivos Nuevos
- `JwtSettings.cs`: Configuración JWT
- `RateLimitingService.cs`: Servicio de rate limiting
- `RateLimitingMiddleware.cs`: Middleware de rate limiting
- `SecurityHeadersMiddleware.cs`: Headers de seguridad
- `InputSanitizationMiddleware.cs`: Sanitización de inputs
- `JwtAuthorizationAttribute.cs`: Atributo de autorización

## ✅ Checklist de Seguridad

- [x] JWT con mejores prácticas
- [x] Rate limiting por IP, usuario y endpoint
- [x] Headers de seguridad completos
- [x] Sanitización de inputs
- [x] Autenticación JWT robusta
- [x] Autorización granular
- [x] Logging de eventos de seguridad
- [x] Configuración flexible
- [x] Documentación completa
- [x] Preparación para GraphQL 