using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RetoBackend.Api.Modelo;
using RetoBackend.Api.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace RetoBackend.Api.Aplicacion
{
    public class Update
    {
        public class Ejecuta : IRequest
        {
            public int PersonaId { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime FecNacimiento { get; set; }
            public string Documento { get; set; }
            public string TipoDocumento { get; set; }
            public List<PersonaDetalle> PersonaDetalleLista { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {

            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
                RuleFor(x => x.FecNacimiento).NotEmpty();
                RuleFor(x => x.Documento).NotEmpty();
                RuleFor(x => x.TipoDocumento).NotEmpty();
            }
        }


        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoPersona _contexto;

            public Manejador(ContextoPersona contexto)
            {
                _contexto = contexto;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                int PersonaId = request.PersonaId;
                var detail = request.PersonaDetalleLista;
                var PersonaUpd = await _contexto.Persona.Where(x => x.PersonaId == PersonaId).FirstOrDefaultAsync();

                PersonaUpd.Nombre = request.Nombre;
                PersonaUpd.Apellido = request.Apellido;
                PersonaUpd.FecNacimiento = request.FecNacimiento;
                PersonaUpd.Documento = request.Documento;
                PersonaUpd.TipoDocumento = request.TipoDocumento;

                var value = await _contexto.SaveChangesAsync();

                if (value == 0)
                {
                    throw new Exception("No se pudo actualizar a la  Persona");
                }

                var ListPersonaDetalleUpdContext = await _contexto.PersonaDetalle.Where(x => x.PersonaId == PersonaId).ToListAsync();

                //Update detail
                foreach (var obj in ListPersonaDetalleUpdContext)
                {
                    var personaDetalleUpd = detail.Where(x => x.PersonaDetalleId == obj.PersonaDetalleId).FirstOrDefault();
                    if (personaDetalleUpd != null)
                    {
                        obj.Direccion = personaDetalleUpd.Direccion;
                        obj.Email = personaDetalleUpd.Email;
                        obj.Telefono = personaDetalleUpd.Telefono;
                    }
                }
                //Insert detail
                foreach (var obj in detail)
                {
                    if (obj.PersonaDetalleId == 0)
                        _contexto.PersonaDetalle.Add(obj);
                };

                value = await _contexto.SaveChangesAsync();

                if (value > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo guardar el detalle de la Persona");

            }
        }

    }
}
