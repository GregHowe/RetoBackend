using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RetoBackend.Api.Modelo;

namespace RetoBackend.Api.Persistencia
{
    public class ContextoPersona : DbContext
    {
        public ContextoPersona() { }
     
        public ContextoPersona(DbContextOptions<ContextoPersona> options) : base(options) { }

        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<PersonaDetalle> PersonaDetalle { get; set; }

    }
}
