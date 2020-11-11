using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteService
{
    public class LibrosService : ILibrosService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<LibrosService> _logger;

        public LibrosService(IHttpClientFactory httpClient, ILogger<LibrosService> logger) 
        {
            //Injectamos a ambos objetos.Y con esto ya se puede invocar a la MicroServices Libro.
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<(bool resultado, LibroRemote Libro, string ErrorMessage)> GetLibro(Guid LibroId)
        {
            try
            {
                //Libros esta configurado en el Startup 
                var cliente = _httpClient.CreateClient("Libros");
                //Este GetAsync lo que hace es invocar al Endpoints que yo quiero.En este caso se trata
                //del EndPoint de la descripción del libro. Todos los campos del libro.Solo pasandole el
                //parámetro del Id
                var response = await cliente.GetAsync($"api/LibroMaterial/{LibroId}");
                if (response.IsSuccessStatusCode) //Si fue exitoso
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    //PropertyNameCaseInsensitive:true para comparar los nombres de propiedad mediante la
                    //comparación sin distinción entre mayúsculas y minúsculas; en caso contrario, false
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    //Resultado ya contiene toda la data del libro.
                    var resultado = JsonSerializer.Deserialize<LibroRemote>(contenido, options);
                    return (true, resultado, null);
                }

                //En caso de que hubiera error, estos son los parámetros que tiene devolver
                return (false, null, response.ReasonPhrase);

            }
            catch (Exception e) {
                _logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }
    }
}
