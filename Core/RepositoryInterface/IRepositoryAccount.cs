using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RepositoryInterface
{
    public interface IRepositoryAccount : IRepository<Account>
    {
        string CercaAccount(string titolo);
        List<Account> GetAllAccount();
    }
}
