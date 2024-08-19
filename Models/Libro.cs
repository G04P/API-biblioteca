using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiBiblioteca.Models
{
    public class Libro
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public int AutorID { get; set; }
        [JsonIgnore]
        public Autor Autor { get; set; }
    }
}
