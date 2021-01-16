using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetoBackend.Api.Modelo
{
    public class PersonaDetalle
    {
        public int PersonaDetalleId { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public Persona Persona { get; set; }
        public int PersonaId { get; set; }


    }
}
