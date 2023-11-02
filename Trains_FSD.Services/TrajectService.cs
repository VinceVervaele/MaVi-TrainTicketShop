using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories.Interfaces;
using Trains_FSD.Services.Interfaces;

namespace Trains_FSD.Services
{
    public class TrajectService : IServiceTraject<Traject>
    {

        private IDAOTraject<Traject> _trajectDAO;

        public TrajectService(IDAOTraject<Traject> trajectDAO)
        {
            _trajectDAO = trajectDAO;
        }

        public Task Add(Traject entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Traject entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Traject> FindById(int id)
        {
            return await _trajectDAO.FindById(id);
        }

        public async Task<Traject> FindByMultipleIds(int departureCityId, int arrivalCityId)
        {
            return await _trajectDAO.FindByMultipleIds(departureCityId, arrivalCityId);
        }

        public Task<IEnumerable<Traject>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(Traject entity)
        {
            throw new NotImplementedException();
        }
    }
}
