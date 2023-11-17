using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Mappings
{
    /// <summary>
    ///     Registra todos os mapas configurados na aplicação.
    /// </summary>
    public class AutoMapperInitializer
    {
        private readonly IMapper _mapper;

        public AutoMapperInitializer()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.AddMaps(typeof(MappingProfile).Assembly)));
        }

        public IMapper GetMapper()
        {
            return _mapper;
        }
    }
}
