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
    public class TrajectLineService : IService<TrajectLine>
    {

        private IDAO<TrajectLine> _trajectLineDAO;

        public TrajectLineService(IDAO<TrajectLine> trajectLineDAO)
        {
            _trajectLineDAO = trajectLineDAO;
        }

        public Task Add(TrajectLine entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(TrajectLine entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TrajectLine> FindById(int id)
        {
            return await _trajectLineDAO.FindById(id);
        }

        public Task<IEnumerable<TrajectLine>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(TrajectLine entity)
        {
            throw new NotImplementedException();
        }
    }
}
