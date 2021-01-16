using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using RetoBackend.Api.Aplicacion;
using RetoBackend.Api.Modelo;

namespace RetoBackend.Api.Tests
{
    public class MappingTest : Profile
    {
        public MappingTest() {
            CreateMap<Persona, PersonaDto>();
            CreateMap<PersonaDetalle, PersonaDetalleDto>();
        }

    }
}
