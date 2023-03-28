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
        public int Userid { get; set; }
        public string Username { get; set; }
    }
}