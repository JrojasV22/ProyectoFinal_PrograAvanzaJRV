# Sistema de Matrícula Universitaria – ProyectoFinal_RojasJeffry

Proyecto final del curso de Programación Avanzada en Web (Universidad Fidélitas).
Sistema de matrícula universitaria desarrollado con **ASP.NET Core MVC (.NET 8)**,
**Entity Framework Core (Code First)**, **MySQL 8** (proveedor Pomelo) y **ASP.NET Core Identity**.

## Estado actual (Avance 2 – parte 2)
## Historias de usuario implementadas (Avance 2)

| Historia | Descripción 

HU-01  Registro de estudiante con validaciones en cliente (jQuery) y servidor | `/Cuenta/Registro` |
HU-02  Selección de carrera al registrarse; los cursos de la carrera se cargan por AJAX | `/Cuenta/Registro` |
HU-03  Inicio y cierre de sesión con ASP.NET Core Identity; inicio del estudiante | `/Cuenta/Login`, `/Estudiante/Inicio` 
HU-09 Panel de administración exclusivo para el rol Administrador | `/Admin` |
HU-10 Crear, listar, editar y eliminar carreras, con buscador y paginación | `/Carreras` |
HU-11 Crear, editar y eliminar cursos asociados a carreras; filtro por carrera con AJAX | `/Cursos` |
HU-12 Registrar, editar y eliminar docentes | `/Docentes` |


Además, la página pública `/Home/Carreras` muestra las carreras disponibles.

## Base de datos

Tablas del sistema: **Carreras, Cursos, Docentes, DocentesCarreras, Estudiantes, Matriculas,
DetallesMatricula**, más las tablas de ASP.NET Core Identity (AspNetUsers, AspNetRoles,
AspNetUserRoles, etc.). Las relaciones usan borrado restringido: no se puede eliminar una carrera
con cursos o estudiantes, ni un docente con cursos asignados.

## Seguridad

- Roles **Administrador** y **Estudiante** (ASP.NET Core Identity).
- Los controladores de administración usan `[Authorize(Roles = "Administrador")]`.
- Si un estudiante intenta entrar a una ruta de administración, ve la página de acceso denegado.

## Cómo ejecutar

Requisitos: .NET 8 SDK, MySQL 8 y la herramienta `dotnet-ef` (`dotnet tool install --global dotnet-ef --version 8.0.11`).
Se puede abrir con Visual Studio 2022 o con VS Code.

1. En `ProyectoFinal_RojasJeffry/appsettings.json`, poner el usuario y la contraseña de MySQL en `DefaultConnection`.
2. En una terminal, dentro de la carpeta `ProyectoFinal_RojasJeffry`:
   ```
   dotnet build
   dotnet ef database update
   dotnet run
   ```
3. Abrir la dirección que muestra la terminal. Al iniciar se crean los roles, el administrador y el catálogo
   de ejemplo (6 carreras, 4 docentes y 8 cursos).

La carpeta `BaseDatos` de la entrega incluye también el script `ProyectoFinal_RojasJeffry.sql`, que crea la
base con sus datos directamente en MySQL

**Administrador inicial:** `admin@matriculau.com` / `Admin123!`
**Estudiante:** se crea desde la opción *Regístrese aquí* de la pantalla de inicio de sesión.
