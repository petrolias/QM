using QM.Models.Abstractions;

namespace QM.Models.Validations.Extensions
{
    public static class RegistrationModelExtensions
    {
        public static bool IsValid(this IRegistrationModel registrationModel)
        {
            try
            {
                registrationModel
                    .ValidateUsername()
                    .ValidateUserId();
            }
            catch (Exception ex)
            {
                //var message = ex.Message;
                //log exception or what ever
                return false;
            }
            return true;
        }

        private static IRegistrationModel ValidateUsername(this IRegistrationModel registrationModel)
        {
            if (string.IsNullOrEmpty(registrationModel.UserName))
            {
                throw new Exception($"username is null or empty string");
            }

            return registrationModel;
        }

        private static IRegistrationModel ValidateUserId(this IRegistrationModel registrationModel)
        {
            if (registrationModel.UserId <= 0)
            {
                throw new Exception($"user Id is negative number");
            }

            return registrationModel;
        }
    }
}
