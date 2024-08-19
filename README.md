# API de Biblioteca

Esta es una API de biblioteca construida con .NET 8. Proporciona funcionalidades para gestionar libros y autores.

## Instrucciones para Ejecutar el Proyecto Localmente

### Requisitos Previos

Asegúrate de tener instalados los siguientes elementos en tu máquina:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL](https://dev.mysql.com/downloads/installer/)

### Dependencias

El proyecto tiene las siguientes dependencias:

- **Paquetes principales:**
  - `Microsoft.AspNetCore.Authentication` 2.2.0
  - `Microsoft.EntityFrameworkCore` 8.0.7
  - `Microsoft.EntityFrameworkCore.Design` 8.0.7
  - `Microsoft.EntityFrameworkCore.InMemory` 8.0.7
  - `Microsoft.Extensions.Configuration` 8.0.0
  - `Moq` 4.20.70
  - `Pomelo.EntityFrameworkCore.MySql` 8.0.2
  - `Swashbuckle.AspNetCore` 6.7.0

- **Paquetes de pruebas:**
  - `coverlet.collector` 6.0.0
  - `Microsoft.NET.Test.Sdk` 17.8.0
  - `xunit` 2.5.3
  - `xunit.runner.visualstudio` 2.5.3

### Configuración del Proyecto

1. **Clona el repositorio:**

   ```bash
   git clone https://github.com/usuario/repo.git
   cd nombre-del-directorio-del-proyecto
Si todo está configurado correctamente, la API debería levantarse en http://localhost:5000.

### Autenticación
Las rutas de la API están protegidas por autenticación básica. Para acceder a los endpoints, deberás:

1. **Crear un usuario en la base de datos:**

Debes agregar un usuario a la tabla usuarios en la base de datos para obtener acceso a los endpoints.

2. **Utilizar autenticación básica:**

Asegúrate de proporcionar las credenciales correctas en las solicitudes para autenticarte y obtener acceso. 
