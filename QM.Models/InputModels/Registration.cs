using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Models.InputModels
{
    internal class Registration : BaseRegistration
    {
        public int UserId { get; set; }
        public string UserName { get; set; }        
    }
}