using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Libro.Tests
{
    //Esta clase se hace una vez y se puede utilizar en todas las entidades  modelos que quieras testear en tu 
    //proyecto TiendaServicios.Api.Libro.Test
    public class AsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        public AsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        //Este método crea la instancia generica para hacer un Query
        public IQueryable CreateQuery(Expression expression)
        {
            return new AsyncEnumerable<TEntity>(expression);  
        }
        
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new AsyncEnumerable<TElement>(expression);
        }
 
        //Logica de la inserción que quiero realizar.La lógica de la operación puede ser un Insert, Delete, Update
        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

         
        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        //Esta clase el objetivo principal es tomar los parametros de entrada genericos. Ya que no sabemos que
        //parametros van a entrar puede ser un texto, puede ser un uno.Por eso se trabaja con valores genericos.
        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            var resultadoTipo = typeof(TResult).GetGenericArguments()[0]; 
            var ejecucionResultado = typeof(IQueryProvider)
                                        .GetMethod(
                                            name: nameof(IQueryProvider.Execute),
                                            genericParameterCount: 1,
                                            types: new[] { typeof(Expression) }
                                        )
                                        .MakeGenericMethod(resultadoTipo)
                                        .Invoke(this, new[] { expression });

            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))?
                    .MakeGenericMethod(resultadoTipo).Invoke(null, new[] { ejecucionResultado });

        }
    }
}
