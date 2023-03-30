using QM.DAL.Models;
using QM.Models.Abstractions;
using QM.Models.InputModels;

namespace QM.DAL.Mapper
{
    public static class RegistrationModelDBExtensions
    {
        public static RegistrationModelDB GetDBModel(this IRegistrationModel registrationModel) {

            var registrationModelDB = AutoMapperHelper.CreateMapperIfNull().Map<RegistrationModelDB>(registrationModel);
            return registrationModelDB;
        }
    }
}