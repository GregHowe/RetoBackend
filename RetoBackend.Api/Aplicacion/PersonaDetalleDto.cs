using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetoBackend.Api.Aplicacion
{
    public class PersonaDetalleDto
    {
        public int PersonaDetalleId { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public int PersonaId { get; set; }


    }
}
