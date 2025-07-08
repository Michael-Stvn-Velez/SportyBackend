# BackendSport API

API para la gestión de usuarios, roles y permisos, desarrollada con Clean Architecture y MongoDB.

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

### 5. Acceder a la documentación Swagger
- Abre tu navegador en:
  - http://localhost:5000/
  - o https://localhost:5001/

Swagger UI estará disponible en la raíz del proyecto.

## Notas
- El entorno de desarrollo está configurado en `launchSettings.json`.
- No subas archivos de configuración sensibles al repositorio.
- Usa los archivos `.example.json` como referencia para nuevos entornos.

---

¿Dudas? Consulta este README o contacta al equipo de desarrollo.
