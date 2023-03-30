using QA.External.Models;
using QM.Models.Abstractions;
using QM.Models.DomainModels;

namespace QM.Mapper
{
    public static class InputRegistrationModelExtensions
    {
        public static RegistrationModel GetDomainModel(this InputRegistrationModel  inputRegistrationModel) {

            var registrationModel = AutoMapperHelper.CreateMapperIfNull().Map<RegistrationModel>(inputRegistrationModel);
            return registrationModel;
        }
    }
}