using AutoMapper;
using TiendaServicios.Api.Autor.Modelo;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    //Dentro de esta clase voy a agregar todos los mapeos que necesito realizar entre una clase EF
    //y las clases Dto.

    //Los perfiles de mapeo le permiten agrupar configuraciones de mapeo. Una mejor manera de organizar 
    //su mapeo es utilizando Perfiles de mapeo. Para cada entidad / área, puede crear una clase de mapeo 
    //que herede del perfil.
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AutorLibro, AutorDto>();
        }

    }
}
