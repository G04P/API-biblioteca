using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBiblioteca.DTOs
{
        public class AutorUpdateDto
        {
            public int ID { get; set; }
            public string Nombre { get; set; }
            public DateTime FechaNacimiento { get; set; }
            public string Nacionalidad { get; set; }
        }

}
