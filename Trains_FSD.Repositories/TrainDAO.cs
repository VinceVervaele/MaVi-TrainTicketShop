using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Trains_FSD.Models.Data;
using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories.Interfaces;

namespace Trains_FSD.Repositories
{
    public class TrainDAO : IDAO<Train>
    {

        private readonly TrainDbContext _dbContext;

        public TrainDAO()
        {
            _dbContext = new TrainDbContext();
        }

        public Task Add(Train entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Train entity)
        {
            throw new NotImplementedException();
        }

        public Task<Train> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Train>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(Train entity)
        {
            throw new NotImplementedException();
        }
    }
}
