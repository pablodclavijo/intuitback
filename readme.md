# Challenge Intuit API

  

API REST desarrollada en **ASP.NET Core 8** con arquitectura por capas (Domain, Application, Infrastructure, API) para coding challenge de la empresa Intuit Salud / Yappa. Por Pablo Clavijo

  

---

  

## Tecnologías

  

- .NET 8

- Entity Framework Core

- SQL Server

- ASP.NET Core Web API

- Mapster

- Swagger para documentación y pruebas
  

---

  

## Estructura del proyecto

  

IntuitBack/

├── IntuitBack.Domain/ # Entidades del dominio (Cliente, Log)

├── IntuitBack.Application/ # Interfaces y servicios de negocio

├── IntuitBack.Infrastructure/ # Acceso a datos (DbContext, Jobs)

├── IntuitBack.Api/ # API (controllers, middleware, configuración)

  
  

---

  

## Configuración mínima

  

### 1. Base de datos

  

Scripts ejecutados

    CREATE DATABASE IntuitBackDb;
    GO

    USE IntuitBackDb;
    GO
    
      
    
    CREATE TABLE Clientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombres NVARCHAR(100) NOT NULL,   
    Apellidos NVARCHAR(100) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Cuit NVARCHAR(20) NOT NULL UNIQUE,
    Domicilio NVARCHAR(150),
    TelefonoCelular NVARCHAR(50) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    FechaModificacion DATETIME2 NULL,
    Eliminado BIT NOT NULL DEFAULT 0
    );

  

    CREATE TABLE Logs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nivel NVARCHAR(50) NOT NULL,
    Mensaje NVARCHAR(500) NOT NULL,
    Detalle NVARCHAR(4000),
    FechaCreacion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

  
  

2. Configurar conexión a la base de datos

En IntuitBack.Api/appsettings.json:

  
 

     
    
    {
    
	    "ConnectionStrings": {
	    
		    "DefaultConnection": "Server=localhost;Database=IntuitBackDb;Trusted_Connection=True;TrustServerCertificate=True;" //cambiar acá por la cadena de conexión que corresponda, yo usé esta porque estaba en mi local
	    
	    }
    
    }

  
  
  

## Endpoints

### clientes
 - GET /api/clientes => Lista todos los clientes
 - GET /api/clientes/{id} => Obtiene un cliente por ID
 - GET /api/clientes/search?nombre=... => Busca por nombre/apellido
 -  POST /api/clientes => Crea un cliente nuevo
 -  PUT /api/clientes/{id} => Actualiza un cliente existente
 -  DELETE /api/clientes/{id} => Elimina un cliente (lógico)

###logs
 - GET /api/logs/GetAll/ultimos?=... => Lista los últimos n registros de
   logs, por defecto 100

  

## Extras

 - #### Logs en base de datos registrando acciones y excepciones lanzadas
 - #### Middleware de manejo de errores
 - #### Servicio en segundo plano que limpia logs mayores a un año.
 - #### Data annotations en los dtos


## Consideraciones

-#### se deshabilitó el CORS
-#### no se implementó autenticación
