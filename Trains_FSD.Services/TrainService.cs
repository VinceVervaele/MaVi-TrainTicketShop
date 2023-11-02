using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories;
using Trains_FSD.Repositories.Interfaces;
using Trains_FSD.Services.Interfaces;

namespace Trains_FSD.Services
{
    public class TrainService : IService<Train>
    {

        private IDAO<Train> _trainDAO;

        public TrainService(IDAO<Train> trainDAO)
        {
            _trainDAO = trainDAO;
        }

        public Task Add(Train entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Train entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Train>> FindByCustomerId(string id)
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
