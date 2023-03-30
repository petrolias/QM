using AutoMapper;

namespace QM.DAL.Mapper
{
    public static class AutoMapperHelper<TTo>
    {
        private static IMapper _mapper;

        private static void CreateMapperIfNull()
        {
            if (_mapper != null) { return; }
            var config = new MapperConfiguration(cfg =>
            {
                //ADD any required configurations
            });
            _mapper = config.CreateMapper();
        }

        public static TTo GetMappingResult<TFrom>(TFrom obj)
        {
            CreateMapperIfNull();
            return _mapper.Map<TFrom, TTo>(obj);
        }

        public static IEnumerable<TTo> GetMappingResult<TFrom>(IEnumerable<TFrom>? obj)
        {
            if (obj == null)
            {
                return Array.Empty<TTo>();
            }
            CreateMapperIfNull();
            return _mapper.Map<IEnumerable<TTo>>(obj).ToList();
        }
    }
}
