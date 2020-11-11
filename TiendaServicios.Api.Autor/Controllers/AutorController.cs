using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Aplicacion;

namespace TiendaServicios.Api.Autor.Controllers
{
    //Recuerda que el web api, esta enviando la data a la capa de aplicación y para que pueda utilizar eso
    //tiene que utilizar la librería Media TR.Esto significa entonces que el web api necesita importar dentro
    //de controller leer el media T.R.
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AutorController(IMediator mediator)
        {
            _mediator = mediator; //Creamos la injección
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            try
            {
                //_mediator le enviará la data a la clase que tenemos a la clase Nuevo
                return await _mediator.Send(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Devuelve una lista AutorLibro Todos los campos
        //[HttpGet]
        //public async Task<ActionResult<List<AutorLibro>>> GetAutores()
        //{
        //    try
        //    {
        //        return await _mediator.Send(new Consulta.ListaAutor());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    } 
        //}

        //Esta consulta devuelve una ListaDto solo los campos que necesito
        [HttpGet]
        public async Task<ActionResult<List<AutorDto>>> GetAutores()
        {
            try
            {
                return await _mediator.Send(new Consulta.ListaAutor());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<AutorLibro>> GetAutorLibro(string id)
        //{
        //    try
        //    {
        //        return await _mediator.Send(new ConsultaFiltro.AutorUnico{AutoGuid = id });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<AutorDto>> GetAutorLibro(string id)
        {
            try
            {
                return await _mediator.Send(new ConsultaFiltro.AutorUnico { AutoGuid = id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
