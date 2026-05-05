# CleanArquitecture.Template

Template base en .NET 8 con arquitectura por capas y enfoque Clean Architecture.

El objetivo del proyecto es servir como punto de partida para APIs con:
- separacion clara de responsabilidades por capa,
- persistencia con Entity Framework Core + MySQL,
- ejecucion dockerizada para ambiente de Testing.

## Stack principal

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (Pomelo MySQL)
- MediatR + FluentValidation
- AutoMapper
- Serilog + Seq
- Docker + Docker Compose

## Arquitectura por capas

La solucion se divide en cuatro proyectos:

- `Domain`
  - Contiene entidades de dominio y contratos (interfaces) de repositorios.
  - No depende de infraestructura ni de framework de persistencia.

- `Application`
  - Contiene casos de uso (queries/commands), DTOs, mapeos y validaciones.
  - Orquesta reglas de aplicacion via MediatR.
  - Depende de abstracciones de `Domain`, no de implementaciones concretas.

- `Infraestructure`
  - Implementaciones concretas de repositorios y servicios.
  - Configuracion de EF Core (`AppDbContext`, configuraciones de entidades, migraciones).
  - Integracion con MySQL y servicios tecnicos.

- `Presentation`
  - Capa de entrada HTTP: controllers, middlewares y configuracion de la app.
  - Bootstrap de DI, autenticacion, CORS, rate limiting y swagger.

## Estructura de carpetas (resumen)

```text
.
|- Domain/
|  |- Entities/
|  |- Repositories/
|- Application/
|  |- Examples/
|  |  |- Queries/
|  |- Common/
|- Infraestructure/
|  |- Persistence/
|  |  |- Data/
|  |  |  |- Configurations/
|  |  |  |- Migrations/
|  |  |  |- AppDbContext.cs
|  |- Persistence/Repositories/
|- Presentation/
   |- Controllers/
   |- Middlewares/
   |- Program.cs
```

## Flujo de una request (ejemplo real)

Caso de uso: listar ejemplos.

1. Entra `GET /app/examples` en `Presentation/Controllers/ExampleController.cs`.
2. El controller envia `GetExamplesByFiltersQuery` por MediatR.
3. El handler `Application/Examples/Queries/GetExamplesByFIlters/GetExamplesByFiltersHandler.cs` ejecuta el caso de uso.
4. El handler depende de `Domain/Repositories/IExamplesRepository.cs`.
5. `Infraestructure/Persistence/Repositories/ExamplesRepository.cs` resuelve la consulta sobre `AppDbContext`.
6. Se mapea entidad a DTO y se devuelve `200 OK`.

Esto refleja la regla principal del template: la capa de aplicacion conoce contratos, y la infraestructura conoce implementaciones.

## Levantar el proyecto (solo Docker)

Prerequisito unico:
- Docker Desktop (o Docker Engine + Compose plugin) instalado y funcionando.

Parado en la raiz del repo, ejecutar:

```bash
docker build -t ca-template:1.0.0 -f Dockerfile .
docker compose up
```

Opcional en background:

```bash
docker compose up -d
```

Servicios esperados:
- API: `http://localhost:8080`
- Seq UI: `http://localhost:8083`
- MySQL expuesto en host: `localhost:3307`

Para bajar el entorno:

```bash
docker compose down
```

Para bajar y eliminar volumen de base:

```bash
docker compose down -v
```

## Ambientes y Swagger

El `docker-compose.yml` define:
- `ASPNETCORE_ENVIRONMENT=Testing` para la API.

En `Presentation/Program.cs`, Swagger se habilita solo para:
- `Local`
- `Development`

Por eso:
- En Docker (Testing) no se mostrara Swagger,
- Las pruebas funcionales del compose se hacen con Postman (u otro cliente HTTP).

Ademas, actualmente no existe `Presentation/appsettings.Development.json`, por lo que debe considerarse pendiente si se quiere usar entorno `Development`.

## Migraciones EF Core

### Donde ejecutar

Siempre desde la raiz del repositorio

### Comandos

PowerShell:

```powershell
$env:ASPNETCORE_ENVIRONMENT='Local'
dotnet ef migrations add NombreDeLaMigracion --project Infraestructure --startup-project Presentation --output-dir Persistence/Data/Migrations
dotnet ef database update --project Infraestructure --startup-project Presentation
```

Notas:
- `--project Infraestructure` apunta al proyecto que contiene `AppDbContext`.
- `--startup-project Presentation` usa la configuracion de arranque de la API.
- `--output-dir Persistence/Data/Migrations` guarda las migraciones dentro de `Infraestructure/Persistence/Data/Migrations`.

## Guia para crear nuevas entidades y configuraciones

### 1) Crear la entidad de dominio

Crear la clase en `Domain/Entities` (o subcarpeta de negocio correspondiente), heredando de la base adecuada:
- `IdentifiableBaseEntity` si solo requiere `Id`,
- `NameableBaseEntity` si requiere `Id` + `Name`.

Definir:
- propiedades escalares,
- FK explicita cuando aplique,
- propiedades de navegacion.

Para relaciones 1:N:
- en la entidad hija: FK + referencia a la entidad padre,
- en la entidad padre: `ICollection<EntidadHija>`.

### 2) Agregar DbSet y configuracion EF

En `Infraestructure/Persistence/Data/Configurations`, crear:
- `DbSet` via clase parcial del contexto (`internal partial class AppDbContext`),
- clase `IEntityTypeConfiguration<T>` para mapear tabla, longitudes, precision, constraints.

Usar configuraciones base cuando aplique:
- `IdentifiableBaseEntityConfiguration.Configure(builder)`
- `NameableBaseEntityConfiguration.Configure(builder)`

### 3) Seeds (si corresponde)

Si la tabla necesita datos iniciales:
- crear metodo `public static void Seed(ModelBuilder modelBuilder)` en la configuracion de la entidad,
- invocarlo desde `OnModelCreating` en `AppDbContext`.

### 4) Validar en local

1. Setear environment (`Local` o `Testing`).
2. Crear migracion temporal.
3. Ejecutar `database update`.
4. Validar relaciones/restricciones en DB.


## Donde debe ir cada cosa (checklist rapido)

- Entidad y reglas de negocio: `Domain`
- Contratos de repositorio: `Domain/Repositories`
- Caso de uso (query/command + handler): `Application`
- DTO y mapper: `Application`
- Repositorio concreto EF + consultas SQL/ORM: `Infraestructure`
- Configuracion de entidad EF + seeds + DbSet: `Infraestructure/Persistence/Data`
- Endpoint HTTP y middleware: `Presentation`
