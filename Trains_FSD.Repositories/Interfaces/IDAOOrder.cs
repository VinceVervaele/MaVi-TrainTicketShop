using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains_FSD.Repositories.Interfaces
{
    public interface IDAOOrder<T> : IDAO<T> where T : class
    {
        Task<IEnumerable<T>> FindByCustomerId(string customerId);
    }
}
