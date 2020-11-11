using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests
{
    public class LibrosServiceTest
    {

        private IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
        {
            //Función para almacenar data de prueba
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });


            var lista = A.ListOf<LibreriaMaterial>(30);//Le decimos cuantos datos vamos a meter en la lista de prueba
            lista[0].LibreriaMaterialId = Guid.Empty;

            return lista;
        }


        //Este método tenemos que crear un DbContext y el DbSet de prueba 
        private Mock<ContextoLibreria> CrearContexto()
        {

            var dataPrueba = ObtenerDataPrueba().AsQueryable();

            //Estas lineas le estamos indicando que la clase LibreriaMaterial tiene que ser una clase de tipo Entidad.
            //Para eso tenemos que levantarla, tenemos que darle la configuración de Setup y darle un provider, expression,
            //ElementType,GetEnumerator. Estas son las propiedades que debe tener toda clase de EntityFramework.
            //Esto por que al no estar trabajando con una persistencia de Sql, debemos darle esta configuración.
            var dbSet = new Mock<DbSet<LibreriaMaterial>>();
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            //Aqui hay un detalle importante, el método que yo utilizo dentro de la aplicación, para obtener la data es
            //método async.Esto significa que mi Entidad LibreriaMaterial también tenga esas propiedades asincronas.Para 
            //que esto suceda tenemos que agregar dos clases adicionales.Una llamada AsyncEnumerator, AsyncEnumerable.
            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
            .Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

            //Se agrega el provider para que sea posible hacer los filtros hacia al entidad libreria material. Se
            //necesita esto para hacer filtros por LibroId
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));


            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x => x.LibreriaMaterial).Returns(dbSet.Object);
            return contexto;
        }


        [Fact]
        public async void GetLibroPorId()
        {

            var mockContexto = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();

            var request = new ConsultaFiltro.LibroUnico
            {
                LibroId = Guid.Empty
            };

            var manejador = new ConsultaFiltro.Manejador(mockContexto.Object, mapper);

            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.NotNull(libro);
            Assert.True(libro.LibreriaMaterialId == Guid.Empty);//Se consulta el libro que tiene el indice 0

        }


        [Fact]
        public async void GetLibros()
        {

            // que metodo dentro de mi microservice libro se esta encargando 
            //de realizar la consulta de libros de la base de datos???

            //1. Emular a la instancia de entity framework core - ContextoLibreria
            // para emular la acciones y eventos de un objeto en un ambiente de unit test 
            //utilizamos objetos de tipo mock

            var mockContexto = CrearContexto();

            // 2 Emular al mapping IMapper

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();
            //3. Instanciar a la clase Manejador y pasarle como parametros los mocks que he creado

            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mapper);

            Consulta.Ejecuta request = new Consulta.Ejecuta();

            var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(lista.Any());
        }


        [Fact]
        public async void GuardarLibro()
        {
            System.Diagnostics.Debugger.Launch();

            //Instalar Nuget para crear un base de datos en memoria.Microsoft.EntityFrameworkCore.InMemory

            //Este objeto me representa la configuración que va a tener la base de datos en memoria.
            var options = new DbContextOptionsBuilder<ContextoLibreria>()
                .UseInMemoryDatabase(databaseName: "BaseDatosLibro")
                .Options;

            //Se crea el contexto
            var contexto = new ContextoLibreria(options);

            //Se instancia la clase Ejecuta. Se pasan los datos que se guardará en la BD.
            var request = new Nuevo.Ejecuta
            {
                Titulo = "Libro de Microservice",
                AutorLibro = Guid.Empty,
                FechaPublicacion = DateTime.Now
            };

            var manejador = new Nuevo.Manejador(contexto);

            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(libro != null);
        }
    }
}
 