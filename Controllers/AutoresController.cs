using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBiblioteca.Models;
using ApiBiblioteca.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ApiBiblioteca.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public AutoresController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: api/autores/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAllAutores()
        {
            var autores = await _context.Autores.ToListAsync();
            return Ok(autores);
        }

        // GET: api/autores/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Autor>> GetAutorById(int id)
        {
            var autor = await _context.Autores.FindAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            return Ok(autor);
        }

        // GET: api/autores/{id}/libros
        [HttpGet("{id}/libros")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosByAutorId(int id)
        {
            try
            {
                var libros = await _context.Libros
                    .Where(l => l.AutorID == id)
                    .Include(l => l.Autor) // Opcional, si deseas incluir la información del autor
                    .ToListAsync();

                if (libros == null || !libros.Any())
                {
                    return NotFound(); // Retorna 404 si no se encuentran libros para el autor
                }

                return Ok(libros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los libros del autor: {ex.Message}");
            }
        }

        // POST: api/autores
        [HttpPost]
        public async Task<ActionResult<Autor>> CreateAutor([FromBody] AutorCreateDto autorDto)
        {
            if (autorDto == null)
            {
                return BadRequest("Los datos del autor no pueden ser nulos.");
            }

            try
            {
                var nuevoAutor = new Autor
                {
                    Nombre = autorDto.Nombre,
                    FechaNacimiento = autorDto.FechaNacimiento,
                    Nacionalidad = autorDto.Nacionalidad
                };

                _context.Autores.Add(nuevoAutor);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAutorById), new { id = nuevoAutor.ID }, nuevoAutor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el autor: {ex.Message}");
            }
        }

        // DELETE: api/autores/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutor(int id)
        {
            try
            {
                var autor = await _context.Autores.FindAsync(id);

                if (autor == null)
                {
                    return NotFound();
                }

                _context.Autores.Remove(autor);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el autor: {ex.Message}");
            }
        }

        // PUT: api/autores/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAutor(int id, [FromBody] AutorUpdateDto autorActualizado)
        {
            try
            {
                if (id != autorActualizado.ID)
                {
                    return BadRequest("El ID del autor en la URL no coincide con el ID del autor en el cuerpo de la solicitud.");
                }

                var autor = await _context.Autores.FindAsync(id);

                if (autor == null)
                {
                    return NotFound();
                }

                autor.Nombre = autorActualizado.Nombre;
                autor.FechaNacimiento = autorActualizado.FechaNacimiento;
                autor.Nacionalidad = autorActualizado.Nacionalidad;

                _context.Entry(autor).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Autores.Any(e => e.ID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el autor: {ex.Message}");
            }
        }
    }
}
