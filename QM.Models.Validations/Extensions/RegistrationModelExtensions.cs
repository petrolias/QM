using QM.Models.Abstractions;

namespace QM.Models.Validations.Extensions
{
    public static class RegistrationModelExtensions
    {
        public class ValidationResponse { 
            public bool IsSuccess { get; set; }
            public Exception Exception { get; set; }
        }
        public static ValidationResponse GetValidation(this IRegistrationModel registrationModel)
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
                return new ValidationResponse() { IsSuccess = false, Exception = ex };
            }
            return new ValidationResponse() { IsSuccess = true };
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
