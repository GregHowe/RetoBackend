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
    public class ConsultaFiltro
    {
        public class PersonaUnico : MediatR.IRequest<PersonaDto>
        {
            public int PersonaId { get; set; }
        }

        public class Manejador : IRequestHandler<PersonaUnico, PersonaDto>
        {
            private readonly ContextoPersona _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoPersona contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }
            public async Task<PersonaDto> Handle(PersonaUnico request, CancellationToken cancellationToken)
            {
                var Persona = await _contexto.Persona.Where(x => x.PersonaId == request.PersonaId).FirstOrDefaultAsync();
                if (Persona == null)
                {
                    throw new Exception("No se encontro la persona");
                }

                var PersonaDetalle = await _contexto.PersonaDetalle.Where(x => x.PersonaId == request.PersonaId).ToListAsync();
                
                var PersonaDto = _mapper.Map<Persona, PersonaDto>(Persona);
                var PersonaDetalleDto = _mapper.Map< List< PersonaDetalle>, List<PersonaDetalleDto>>(PersonaDetalle);

                PersonaDto.PersonaDetalleDtoLista.AddRange(PersonaDetalleDto);

                return PersonaDto;
            }
        }

    }
}
