using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBiblioteca.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiBiblioteca.DTOs;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class LibrosController : ControllerBase
{
    private readonly BibliotecaContext _context;

    public LibrosController(BibliotecaContext context)
    {
        _context = context;
    }

    // GET: api/libros
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Libro>>> GetLibros()
    {
        try
        {
            var libros = await _context.Libros.Include(l => l.Autor).ToListAsync();
            return Ok(libros);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al obtener los libros.");
        }
    }

    // GET: api/libros/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Libro>> GetLibroById(int id)
    {
        try
        {
            var libro = await _context.Libros
                .Include(l => l.Autor)
                .FirstOrDefaultAsync(l => l.ID == id);

            if (libro == null)
            {
                return NotFound();
            }

            return Ok(libro);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al obtener el libro.");
        }
    }

    // POST: api/libros
    [HttpPost]
    public async Task<ActionResult<Libro>> CreateLibro([FromBody] LibroCreateDto libroDto)
    {
        if (libroDto == null)
        {
            return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
        }

        try
        {
            var libro = new Libro
            {
                Titulo = libroDto.Titulo,
                Descripcion = libroDto.Descripcion,
                FechaPublicacion = libroDto.FechaPublicacion,
                AutorID = libroDto.AutorID
            };

            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLibroById), new { id = libro.ID }, libro);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al crear el libro.");
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLibro(int id)
    {
        try
        {
            // Buscar el libro por ID
            var libro = await _context.Libros.FindAsync(id);

            if (libro == null)
            {// realiza la busqueda del libro , en caso de no existir, retorna error
                return NotFound(); 
            }

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna 204 No Content si la eliminación es exitosa
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al eliminar el libro: {ex.Message}"); 
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLibro(int id, [FromBody] LibroUpdateDto libroActualizadoDto)
    {
        try
        {
            if (libroActualizadoDto == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            var libro = await _context.Libros.FindAsync(id);

            if (libro == null)
            { // realiza la busqueda del libro , en caso de no existir, retorna error

                return NotFound(); 
            }

            // Actualizar los datos del libro
            libro.Titulo = libroActualizadoDto.Titulo;
            libro.Descripcion = libroActualizadoDto.Descripcion;
            libro.FechaPublicacion = libroActualizadoDto.FechaPublicacion;
            libro.AutorID = libroActualizadoDto.AutorID;

            // Guardar los cambios en la base de datos
            _context.Entry(libro).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LibroExists(id))
            {// Retorna error si el libro ya no existe en mi tabla
                return NotFound(); 
            }
            else
            {
                throw; // Relanza la excepción si ocurre un error de concurrencia
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al actualizar el libro: {ex.Message}"); 
        }
    }
    private bool LibroExists(int id)
    {
        return _context.Libros.Any(e => e.ID == id);
    }
}

