# API de Biblioteca

Esta es una API de biblioteca construida con .NET 8. Proporciona funcionalidades para gestionar libros y autores.

## Instrucciones para Ejecutar el Proyecto Localmente

### Requisitos Previos

Asegúrate de tener instalados los siguientes elementos en tu máquina:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL](https://dev.mysql.com/downloads/installer/)

### Configuración del Proyecto

1. **Clona el repositorio:**

   ```bash
   git clone https://github.com/G04P/API-biblioteca.git
   cd ApiBiblioteca
Si todo está configurado correctamente, la API debería levantarse en http://localhost:5000.

### Autenticación
Las rutas de la API están protegidas por autenticación básica. Para acceder a los endpoints, deberás:

1. **Crear un usuario en la base de datos:**

Debes agregar un usuario a la tabla usuarios en la base de datos para obtener acceso a los endpoints.

2. **Utilizar autenticación básica:**

Asegúrate de proporcionar las credenciales correctas en las solicitudes para autenticarte y obtener acceso. 
