using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains_FSD.Repositories.Interfaces
{
    public interface IDAOTraject<T> : IDAO<T> where T : class
    {
        Task<T> FindByMultipleIds(int firstId, int secondId);
    }
}
