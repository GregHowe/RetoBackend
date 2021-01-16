using AutoMapper;
using GenFu;
using MediatR;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RetoBackend.Api.Aplicacion;
using RetoBackend.Api.Modelo;
using RetoBackend.Api.Persistencia;
using Xunit;

namespace RetoBackend.Api.Tests
{
    public class PersonaServiceTest
    {

        private IEnumerable<Persona> ObtenerDataPrueba()
        {
            A.Configure<Persona>()
                    .Fill(x => x.Nombre).AsFirstName()
                     .Fill(x => x.Apellido).AsLastName()
                     .Fill(x => x.FecNacimiento).AsPastDate()
                     .Fill(x => x.Documento).AsArticleTitle()
                    .Fill(x => x.TipoDocumento).AsArticleTitle()
                    .Fill(x => x.PersonaId, () => { return new Random().Next(1, 100); });

            var lista = A.ListOf<Persona>(30);
            lista[0].PersonaId = int.MinValue;

            return lista;
        }

        private IEnumerable<PersonaDetalle> ObtenerDataPruebaDetalle()
        {
            A.Configure<PersonaDetalle>()
                    .Fill(x => x.Direccion).AsFirstName()
                     .Fill(x => x.Email).AsLastName()
                     .Fill(x => x.Telefono).AsPhoneNumber()
                    .Fill(x => x.PersonaId, () => { return new Random().Next(1, 100); });

            var lista = A.ListOf<PersonaDetalle>(30);
            lista[0].PersonaId = int.MinValue;

            return lista;
        }


        private Mock<ContextoPersona> CrearContexto()
        {

            #region dataPruebaPersona

            var dataPrueba = ObtenerDataPrueba().AsQueryable();

            var dbSet = new Mock<DbSet<Persona>>();
            dbSet.As<IQueryable<Persona>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<Persona>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<Persona>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<Persona>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            dbSet.As<IAsyncEnumerable<Persona>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
            .Returns(new AsyncEnumerator<Persona>(dataPrueba.GetEnumerator()));
            dbSet.As<IQueryable<Persona>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<Persona>(dataPrueba.Provider));


            var contexto = new Mock<ContextoPersona>();
            contexto.Setup(x => x.Persona).Returns(dbSet.Object);

            #endregion


            #region dataPruebaPersona

            var dataPruebaPersonaDetalle = ObtenerDataPruebaDetalle().AsQueryable();

            var dbSetPersonaDetalle = new Mock<DbSet<PersonaDetalle>>();
            dbSetPersonaDetalle.As<IQueryable<PersonaDetalle>>().Setup(x => x.Provider).Returns(dataPruebaPersonaDetalle.Provider);
            dbSetPersonaDetalle.As<IQueryable<PersonaDetalle>>().Setup(x => x.Expression).Returns(dataPruebaPersonaDetalle.Expression);
            dbSetPersonaDetalle.As<IQueryable<PersonaDetalle>>().Setup(x => x.ElementType).Returns(dataPruebaPersonaDetalle.ElementType);
            dbSetPersonaDetalle.As<IQueryable<PersonaDetalle>>().Setup(x => x.GetEnumerator()).Returns(dataPruebaPersonaDetalle.GetEnumerator());

            dbSetPersonaDetalle.As<IAsyncEnumerable<PersonaDetalle>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
            .Returns(new AsyncEnumerator<PersonaDetalle>(dataPruebaPersonaDetalle.GetEnumerator()));
            dbSetPersonaDetalle.As<IQueryable<PersonaDetalle>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<PersonaDetalle>(dataPruebaPersonaDetalle.Provider));

            contexto.Setup(x => x.PersonaDetalle).Returns(dbSetPersonaDetalle.Object);

            #endregion

            return contexto;
        }


        [Fact]
        public async void GetPersonaPorId()
        {
            System.Diagnostics.Debugger.Launch();
            var mockContexto = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();

            var request = new ConsultaFiltro.PersonaUnico();
            request.PersonaId = int.MinValue;

            var manejador = new ConsultaFiltro.Manejador(mockContexto.Object, mapper);

            var Persona = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.NotNull(Persona);
            Assert.True(Persona.PersonaId == int.MinValue);
            Assert.True(Persona.PersonaDetalleDtoLista.FirstOrDefault().PersonaId == int.MinValue);

        }


        [Fact]
        public async void GetPersonas()
        {
            var mockContexto = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();
        
            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mapper);

            Consulta.Ejecuta request = new Consulta.Ejecuta();

            var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(lista.Any());
        }


        [Fact]
        public async void GuardarPersona()
        {
            System.Diagnostics.Debugger.Launch();

            var options = new DbContextOptionsBuilder<ContextoPersona>()
                .UseInMemoryDatabase(databaseName: "BaseDatosPersona")
                .Options;

            var contexto = new ContextoPersona(options);

            var request = new Nuevo.Ejecuta();
            request.Nombre = "Hector";
            request.Apellido = "Lazo";
            request.Documento = "123456789";
            request.TipoDocumento = "DNI";
            request.FecNacimiento = DateTime.Now;
            PersonaDetalle item = new PersonaDetalle() { };
            request.PersonaDetalleLista = new List<PersonaDetalle>();
            request.PersonaDetalleLista.Add(new PersonaDetalle()
            {
                Direccion = "Direccion1",
                Email = "Email1",
                Telefono = "Telefono1"
            }); 

            var manejador = new Nuevo.Manejador(contexto);

            var Persona = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(Persona != null);
        }


    }
}
