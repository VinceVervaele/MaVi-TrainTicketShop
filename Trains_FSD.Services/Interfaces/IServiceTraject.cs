using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains_FSD.Services.Interfaces
{
    public interface IServiceTraject<T> : IService<T> where T : class
    {
        Task<T> FindByMultipleIds(int firstId, int secondId);
    }
}
