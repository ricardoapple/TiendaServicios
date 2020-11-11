using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;

namespace TiendaServicios.Api.Libro.Persistencia
{
    public class ContextoLibreria : DbContext
    {
        //Constructor por defecto 
        public ContextoLibreria() { }
     
        public ContextoLibreria(DbContextOptions<ContextoLibreria> options) : base(options) { }

        //La palabra virtual lo que permite es que se puede sobreescribir a futuro, que es lo que necesita nuestro
        //proyecto de servicios va a tomar a LibreriaMaterial la va a sobreescribir 
        public virtual DbSet<LibreriaMaterial> LibreriaMaterial { get; set; }

    }
}
