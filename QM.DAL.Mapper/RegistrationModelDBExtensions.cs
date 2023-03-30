using QM.DAL.Models;
using QM.Models.InputModels;

namespace QM.DAL.Mapper
{
    public static class RegistrationModelDBExtensions
    {
        public static RegistrationModelDB GetDBModel(this RegistrationModel registrationModel) {
            var registrationModelDB = AutoMapperHelper<RegistrationModelDB>.GetMappingResult(registrationModel);
            return registrationModelDB;
        }
    }
}