using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiBiblioteca.Models
{
    public class ApiKey
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Nacionalidad { get; set; }
        [JsonIgnore]
        public ICollection<Libro> Libros { get; set; }
    }
}
