using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RetoBackend.Api.Modelo;

namespace RetoBackend.Api.Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Persona, PersonaDto>();
            CreateMap<PersonaDetalle, PersonaDetalleDto>();
        }
    }
}
