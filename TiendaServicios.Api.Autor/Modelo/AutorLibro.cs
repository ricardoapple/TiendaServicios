using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Autor.Modelo
{
    public class AutorLibro
    {
         public int AutorLibroId { get; set; }//Llave Primaria

         public string Nombre { get; set; }

         public string Apellido { get; set; }

        //Un Autor va a tener una colección de grados académicos
        public DateTime? FechaNacimiento { get; set; }
              
         public ICollection<GradoAcademico> ListaGradoAcademico { get; set; }

        /*Este es un valor universal cuando yo quiera transmitir o darle seguimiento a un
         récord de autor libro desde otra macroservice*/
        //Guid: Significa Global Unique Identifier.Aunque sea string en PostgreSQL
        public string AutorLibroGuid { get; set; }

    }
}
