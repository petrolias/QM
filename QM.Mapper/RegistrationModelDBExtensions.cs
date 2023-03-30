using QM.Mapper.Models;
using QM.Models.Abstractions;
using QM.Models.DomainModels;

namespace QM.Mapper
{
    public static class RegistrationModelDBExtensions
    {
        public static RegistrationModelDB GetDBModel(this IRegistrationModel registrationModel) {

            var registrationModelDB = AutoMapperHelper.CreateMapperIfNull().Map<RegistrationModelDB>(registrationModel);
            return registrationModelDB;
        }
    }
}