using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class Consulta
    {
        //IRequest sirve para mapear los parámetros que envían los clientes, ademas sirve para identificar 
        //cuáles son los valores que vas a devolver el cliente.

        //Esta clase consulta se encarga de enviar de procesar y devolver toda la lista de autores de la BD.
        //public class ListaAutor : IRequest<List<AutorLibro>>
        public class ListaAutor : IRequest<List<AutorDto>>
        {

        }

        //IRequestHandler: Pertenece a la libreria MediaTR. Lo que va a ser recibe el modelo. ListaAutor,
        //Y devuelve AutorDto
        public class Manejador : IRequestHandler<ListaAutor, List<AutorDto>>
        //public class Manejador : IRequestHandler<ListaAutor, List<AutorLibro>>
        {
            private readonly ContextoAutor _contexto;
            private readonly IMapper _mappper;

            public Manejador(ContextoAutor contexto, IMapper mapper)
            {
                //Injectamos los campos.
                _contexto = contexto;
                _mappper = mapper;
            }

            //Esta consulta me devuelve todos los campos de la tabla.
            //public async Task<List<AutorLibro>> Handle(ListaAutor request, CancellationToken cancellationToken)
            //{
            //    try
            //    {
            //        var autores = await _contexto.AutorLibro.ToListAsync();
            //        return autores;
            //    }
            //    catch (System.Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            //Esta clase la mapeamos para que me devuelva solo los campos que necesito.
            public async Task<List<AutorDto>> Handle(ListaAutor request, CancellationToken cancellationToken)
            {
                try
                {
                    var autores = await _contexto.AutorLibro.ToListAsync();
                    //Ingresa Lista de AutorLibro y debe salir AutorDto
                    var autoresDto = _mappper.Map<List<AutorLibro>, List<AutorDto>>(autores);

                    return autoresDto;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
