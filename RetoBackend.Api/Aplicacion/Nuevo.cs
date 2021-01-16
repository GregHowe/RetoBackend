using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RetoBackend.Api.Modelo;
using RetoBackend.Api.Persistencia;

namespace RetoBackend.Api.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
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

                var persona = new Persona
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    FecNacimiento = request.FecNacimiento,
                    Documento = request.Documento,
                    TipoDocumento = request.TipoDocumento
                };

                _contexto.Persona.Add(persona);
                var value = await _contexto.SaveChangesAsync();

                if (value == 0)
                {
                    throw new Exception("No se pudo guardar el Persona");
                }

                int PersonaId = persona.PersonaId;

                foreach (var obj in request.PersonaDetalleLista)
                {
                    var detalle = new PersonaDetalle
                    {
                        PersonaId = PersonaId,
                        Direccion = obj.Direccion,
                        Email = obj.Email,
                        Telefono = obj.Telefono
                    };
                    _contexto.PersonaDetalle.Add(detalle);
                }

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
