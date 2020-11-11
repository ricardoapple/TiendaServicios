using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Libro.Tests
{
    public class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        //Basicamente, lo que tiene que hacer esta clase es evaluar el arreglo que te va a devolver el EF
        //Es un métdo generico para otras microservice que necesite.
        private readonly IEnumerator<T> enumerator;

        public T Current => enumerator.Current;

        public AsyncEnumerator(IEnumerator<T> enumerator) => this.enumerator = enumerator ?? throw new ArgumentNullException();

        //Eliminar el método cuando se ha completado la tarea.
        public async ValueTask DisposeAsync()
        {
            await Task.CompletedTask;//Espera hasta que la tarea se complete.
        }
        //Devolverá los siguientes valores que existan dentro del arreglo.

        public async ValueTask<bool> MoveNextAsync()
        {
            return await Task.FromResult(enumerator.MoveNext());
        }
    }
}
