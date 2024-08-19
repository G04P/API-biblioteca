using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiBiblioteca.Models;
using Microsoft.EntityFrameworkCore;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) {
    
    }
    public DbSet<Libro> Libros { get; set; }
    public DbSet<Autor> Autores { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Libro>()
            .HasOne(l => l.Autor)
            .WithMany(a => a.Libros)
            .HasForeignKey(l => l.AutorID)
            .OnDelete(DeleteBehavior.Cascade);

        // Configura la serialización
        modelBuilder.Entity<Autor>()
            .Ignore(a => a.Libros);
    }

}