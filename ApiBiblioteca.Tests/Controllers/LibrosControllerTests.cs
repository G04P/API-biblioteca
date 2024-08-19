using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBiblioteca.Controllers;
using ApiBiblioteca.Models;
using ApiBiblioteca.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace ApiBiblioteca.Tests.Controllers
{
    public class LibrosControllerTests
    {
        private readonly BibliotecaContext _context;
        private readonly LibrosController _controller;

        public LibrosControllerTests()
        {
            // Configurar el contexto con una base de datos en memoria
            var options = new DbContextOptionsBuilder<BibliotecaContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new BibliotecaContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _controller = new LibrosController(_context);
        }
        public void Dispose() // Limpieza del Contexto entre Pruebas
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetLibroById_ReturnsNotFound_WhenLibroDoesNotExist()
        {
            // Act
            var result = await _controller.GetLibroById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetLibroById_ReturnsLibro_WhenLibroExists()
        {
            // Arrange
            var libro = new Libro { ID = 1, Titulo = "Libro Test", Descripcion = "Descripción Test", AutorID = 1 };
            var autor = new Autor { ID = 1, Nombre = "Autor Test", Nacionalidad = "Argentina" };
            _context.Autores.Add(autor);
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetLibroById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Libro>(okResult.Value);
            Assert.Equal(1, returnValue.ID);
            Assert.Equal("Libro Test", returnValue.Titulo);
        }

        [Fact]
        public async Task GetLibros_ReturnsAllLibros()
        {
            // Arrange
            var autor = new Autor { ID = 1, Nombre = "Autor Test", Nacionalidad = "Argentina" };
            var libro = new Libro { ID = 1, Titulo = "Libro 1", Descripcion = "Descripción 1", AutorID = 1 };
            _context.Autores.Add(autor);
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetLibros();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var libros = Assert.IsType<List<Libro>>(okResult.Value);
            Assert.Single(libros);
        }

        [Fact]
        public async Task CreateLibro_ReturnsCreatedAtActionResult_WhenLibroIsCreated()
        {
            // Arrange
            var autor = new Autor { ID = 2, Nombre = "Autor Test" , Nacionalidad = "Argentina" };
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();

            var libroDto = new LibroCreateDto
            {
                Titulo = "Nuevo Libro",
                Descripcion = "Nueva Descripción",
                FechaPublicacion = new DateTime(2020, 1, 1),
                AutorID = 2
            };

            // Act
            var result = await _controller.CreateLibro(libroDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Libro>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.ID);
            Assert.Equal("Nuevo Libro", returnValue.Titulo);
        }

        [Fact]
        public async Task UpdateLibro_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var autor = new Autor { ID = 1, Nombre = "Autor Test" , Nacionalidad = "Argentina" };
            var libro = new Libro { ID = 1, Titulo = "Libro Original", Descripcion = "Descripción Original", AutorID = 1 };
            _context.Autores.Add(autor);
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            var updatedLibroDto = new LibroUpdateDto
            {
                Titulo = "Libro Actualizado",
                Descripcion = "Descripción Actualizada",
                FechaPublicacion = new DateTime(2021, 1, 1),
                AutorID = 1
            };

            // Act
            var result = await _controller.UpdateLibro(1, updatedLibroDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteLibro_ReturnsNoContent_WhenLibroIsDeleted()
        {
            // Arrange
            var autor = new Autor { ID = 1, Nombre = "Autor Test", Nacionalidad = "Argentina" };
            var libro = new Libro { ID = 1, Titulo = "Libro a Eliminar", Descripcion = "Descripción a Eliminar", AutorID = 1 };
            _context.Autores.Add(autor);
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteLibro(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.Libros.FindAsync(1));
        }
    }
}
