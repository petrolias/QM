using AutoMapper;
using QM.DAL.Models;
using QM.Models.Abstractions;

namespace QM.DAL.Mapper
{
    public static class AutoMapperHelper
    {
        public static IMapper _mapper;
        
        public static void InitializeAutomapper()
        {            
            var config = new MapperConfiguration(cfg =>
            {                
                cfg.CreateMap<IRegistrationModel, RegistrationModelDB>();
                //Any Other Mapping Configuration ....
            });
            //Create an Instance of Mapper and return that Instance
            var mapper = new AutoMapper.Mapper(config);
            _mapper = mapper;            
        }
        public static IMapper CreateMapperIfNull()
        {
            if (_mapper == null) {
                InitializeAutomapper();
            }
            return _mapper;
        }       

    }
}
