using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBiblioteca.Controllers;
using ApiBiblioteca.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using ApiBiblioteca.DTOs;

namespace ApiBiblioteca.Tests.Controllers
{
    public class AutoresControllerTests
    {
        private readonly BibliotecaContext _context;
        private readonly AutoresController _controller;

        public AutoresControllerTests()
        {
            // Configurar el contexto con una base de datos en memoria
            var options = new DbContextOptionsBuilder<BibliotecaContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new BibliotecaContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _controller = new AutoresController(_context);
        }

        [Fact]
        public async Task GetAutorById_ReturnsNotFound_WhenAutorDoesNotExist()
        {
            // Act
            var result = await _controller.GetAutorById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAutorById_ReturnsAutor_WhenAutorExists()
        {
            // Arrange
            var autor = new Autor { ID = 1, Nombre = "Autor Test", Nacionalidad = "Argentina" };
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAutorById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Autor>(okResult.Value);
            Assert.Equal(1, returnValue.ID);
            Assert.Equal("Autor Test", returnValue.Nombre);
        }

        [Fact]
        public async Task GetAllAutores_ReturnsAllAutores()
        {
            // Arrange
            var autor = new Autor
            {
                Nombre = "Autor 1",
                FechaNacimiento = new DateTime(1980, 1, 1),
                Nacionalidad = "Argentina"
            };
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAllAutores();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var autores = Assert.IsType<List<Autor>>(okResult.Value);
            Assert.Single(autores);
        }

        [Fact]
        public async Task CreateAutor_ReturnsCreatedAtActionResult_WhenAutorIsCreated()
        {
            // Arrange
            var autor = new AutorCreateDto {  Nombre = "Nuevo Autor",
                FechaNacimiento = new DateTime(1980, 1, 1),
                Nacionalidad = "Argentina" };

            // Act
            var result = await _controller.CreateAutor(autor);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Autor>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.ID);
            Assert.Equal("Nuevo Autor", returnValue.Nombre);
        }

        [Fact]
        public async Task UpdateAutor_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var autor = new Autor { ID = 1, Nombre = "Autor Original", Nacionalidad = "Argentina" };
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();

            var updatedAutor = new AutorUpdateDto { ID = 1, Nombre = "Autor Actualizado" };

            // Act
            var result = await _controller.UpdateAutor(1, updatedAutor);

            // Assert
            Assert.IsType<NoContentResult>(result);
        } 


        
        [Fact]
        public async Task DeleteAutor_ReturnsNoContent_WhenAutorIsDeleted()
        {
            // Arrange
            var autor = new Autor { ID = 2, Nombre = "Autor a Eliminar", Nacionalidad = "Argentina" };
            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteAutor(2);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.Autores.FindAsync(2));
        }
    }
}
