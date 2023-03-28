using QM.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Models.InputModels
{
    public class RegistrationModel : BaseRegistrationModel, IRegistrationModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}