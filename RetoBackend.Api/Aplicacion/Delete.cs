using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RetoBackend.Api.Modelo;
using RetoBackend.Api.Persistencia;

namespace RetoBackend.Api.Aplicacion
{
    public class Delete
    {
        public class DeletePersona : IRequest
        {
            public int PersonaId { get; set; }
        }

        public class Manejador : IRequestHandler<DeletePersona>
        {
            private readonly ContextoPersona _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoPersona contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(DeletePersona request, CancellationToken cancellationToken)
            {
                _contexto.Persona.Remove(new Persona() { PersonaId = request.PersonaId });
                var value = await _contexto.SaveChangesAsync();

                if (value > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar a la Persona");
            }
        }

    }
}
