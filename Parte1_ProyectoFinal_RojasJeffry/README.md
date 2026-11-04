# Sistema de Matrícula Universitaria – ProyectoFinal_RojasJeffry

Proyecto final del curso de Programación Avanzada en Web (Universidad Fidélitas).
Sistema de matrícula universitaria desarrollado con **ASP.NET Core MVC (.NET 8)**,
**Entity Framework Core (Code First)**, **MySQL 8** (proveedor Pomelo) y **ASP.NET Core Identity**.

## Estado actual (Avance 2 – parte 1)

- Base de datos con las tablas principales: Carreras, Cursos, Docentes, DocentesCarreras,
  Estudiantes, Matriculas y DetallesMatricula, además de las tablas de Identity.
- Roles **Administrador** y **Estudiante**, con un administrador inicial creado automáticamente.
- Registro de estudiantes con selección de carrera y validaciones en cliente y servidor.
- Carga de los cursos de la carrera seleccionada por AJAX (`/api/cursos?carreraId=`).
- Inicio y cierre de sesión, y página de inicio del estudiante.

## Cómo ejecutar

Requisitos: .NET 8 SDK, MySQL 8 y la herramienta `dotnet-ef` (`dotnet tool install --global dotnet-ef --version 8.0.11`).

1. En `ProyectoFinal_RojasJeffry/appsettings.json`, poner el usuario y la contraseña de MySQL 
2. En una terminal, dentro de la carpeta `ProyectoFinal_RojasJeffry`:
   ```
   dotnet ef database update
   dotnet run
   ```

Administrador inicial: `admin@matriculau.com` / `Admin123!`
