using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class Nuevo
    {
        //Clase que recibe los parametros que esta enviando los controles.
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime? FechaNacimiento { get; set; }
        }

        //Con esta clase vamos a ser las reglas de validación.
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
            }
        }

        //Esta clase se implementará la lógica de la inserción en la base de datos.
        //De eso se trata el patrón CQRS de dividir las responsabilidades.
        //IRequestHandler pertenece a la libreria MediaTR
        public class Manejador : IRequestHandler<Ejecuta>
        {
            public readonly ContextoAutor _contexto;

            //Aqui lo inyectamos: Inyectar significa instanciar crear un objeto.A traves de un constructor.
            public Manejador(ContextoAutor contexto)
            {
                _contexto = contexto;
            }

            //Task<Unit> lo que espera devolver es 1 si fue existoso o cero si hubo errores
            //request contiene Nombre,Apellido,FechaNacimiento. 
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                try
                {
                    var autorLibro = new AutorLibro
                    {
                        Nombre = request.Nombre,
                        Apellido = request.Apellido,
                        FechaNacimiento = request.FechaNacimiento,
                        AutorLibroGuid = Convert.ToString(Guid.NewGuid())
                    };
                    _contexto.AutorLibro.Add(autorLibro);
                    var valor = await _contexto.SaveChangesAsync();

                    if (valor > 0)//Si valor es mayor que cero la inserción fue correcta
                    {
                        return Unit.Value;
                    }
                    else//En caso contrario devuelve una excepción.
                    {
                        throw new Exception("No se puede insertar el Autor del libro");
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

    }
}
