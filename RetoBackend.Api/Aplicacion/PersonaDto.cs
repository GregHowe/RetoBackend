using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetoBackend.Api.Aplicacion
{
    public class PersonaDto
    {
        public PersonaDto()
        {
            PersonaDetalleDtoLista = new List<PersonaDetalleDto>();
        }
        public int PersonaId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FecNacimiento { get; set; }
        public string Documento { get; set; }
        public string TipoDocumento { get; set; }
        public List<PersonaDetalleDto> PersonaDetalleDtoLista { get; set; }

    }
}
