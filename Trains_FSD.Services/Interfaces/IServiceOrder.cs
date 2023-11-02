using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains_FSD.Repositories.Interfaces;

namespace Trains_FSD.Services.Interfaces
{
    public interface IServiceOrder<T> : IService<T> where T : class
    {
        Task<IEnumerable<T>> FindByCustomerId(string id);
    }
}
