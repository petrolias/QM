using AutoMapper;
using QA.External.Models;
using QM.DAL.Models;
using QM.Models.Abstractions;
using QM.Models.DomainModels;

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
                cfg.CreateMap<InputRegistrationModel, IRegistrationModel>();
                cfg.CreateMap<InputRegistrationModel, RegistrationModel>();
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
