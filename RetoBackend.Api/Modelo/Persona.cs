using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetoBackend.Api.Modelo
{
    public class Persona
    {
        public int PersonaId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FecNacimiento { get; set; }
        public string Documento { get; set; }
        public string TipoDocumento { get; set; }
        public List<PersonaDetalle> PersonaDetalleLista { get; set; }



    }
}
