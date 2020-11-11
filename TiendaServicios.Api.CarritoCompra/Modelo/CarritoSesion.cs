using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.CarritoCompra.Modelo
{
    public class CarritoSesion
    {
        public int CarritoSesionId { get; set; }

        public DateTime? FechaCreacion { get; set; }

        //Contiene un grupo de productos que a futuro el usuario va a comprar.
        public ICollection<CarritoSesionDetalle> ListaDetalle { get; set; }
    }
}
