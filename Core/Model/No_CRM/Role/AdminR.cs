using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.No_CRM.Role
{
    public class AdminR : IRole
    {
        public string roleName { get; set; }
        public string nomeR()
        {
            return roleName = "Adim";
        }
    }
}
