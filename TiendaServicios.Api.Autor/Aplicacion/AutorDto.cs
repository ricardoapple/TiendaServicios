using System;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    //El objetivo de esta clase Dto es modelar la data que se va enviar al cliente mediante filtros o merge(Uniones)
    //Se utilizará solo para consultas.Se incluirán solo los campos que se necesita el cliente.
    public class AutorDto
    {
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string AutorLibroGuid { get; set; }
    }
}
