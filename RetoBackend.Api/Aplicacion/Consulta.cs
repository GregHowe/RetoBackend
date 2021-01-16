using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RetoBackend.Api.Modelo;
using RetoBackend.Api.Persistencia;

namespace RetoBackend.Api.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<List<PersonaDto>>
        {
        }

        public class Manejador : IRequestHandler<Ejecuta, List<PersonaDto>>
        {
            private readonly ContextoPersona _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoPersona contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;

            }

            public async Task<List<PersonaDto>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                var Personas = await _contexto.Persona.ToListAsync();
                var PersonasDto = _mapper.Map<List<Persona>, List<PersonaDto>>(Personas);
                return PersonasDto;
            }
        }

    }
}
