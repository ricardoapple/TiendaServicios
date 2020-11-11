using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Nuevo
    {
        //Seguimos trabajando con el patrón CQRS
        //Esta es la clase que se va a insertar en la base de datos .
        public class Ejecuta : IRequest { 
            public  string Titulo { get; set; }
            public  DateTime? FechaPublicacion { get; set; }
            public  Guid? AutorLibro { get; set; }

        }

        //Validamos que los campos no sean nulos.
        public class EjecutaValidacion : AbstractValidator<Ejecuta> {

            public EjecutaValidacion() {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.AutorLibro).NotEmpty();
            }
        }


        //La clase manejador realiza el proceso de inserción
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoLibreria _contexto;//Aqui lo inyectamos

            public Manejador(ContextoLibreria contexto) {
                _contexto = contexto;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                var libro = new LibreriaMaterial
                {
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro
                };

                _contexto.LibreriaMaterial.Add(libro);

                var value = await _contexto.SaveChangesAsync();

                if (value > 0) //Si es mayor que cero fue correcta la operación
                {
                    return Unit.Value;
                }

                //Si hubo un error devuelve la siguiente exception.
                throw new Exception("No se pudo guardar el libro");
                
            }
        }

    }
}
