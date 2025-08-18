using Core.Model.No_CRM.Role;
using System.Collections.Generic;


namespace Core.Model.No_CRM
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public IRole Role { get; set; }

        public ICollection<Account> accounts { get; set; } = new List<Account>();

        public User() { }
        public string VisUser()
        {
            return $"{UserId} - {UserName} - {Password} - {Role.roleName}";

        }
    }
}
