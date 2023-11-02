using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains_FSD.Models.Data;
using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories.Interfaces;

namespace Trains_FSD.Repositories
{
    public class TrajectDAO : IDAOTraject<Traject>
    {

        private readonly TrainDbContext _dbContext;

        public TrajectDAO()
        {
            _dbContext = new TrainDbContext();
        }

        public Task Add(Traject entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Traject entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Traject>> FindByCustomerId(string customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<Traject> FindById(int id)
        {
            try
            {
                return await _dbContext.Trajects
                    .Where(a => a.Id == id)
                    .Include(a => a.TrajectLines)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            { 
                throw new Exception("error DAO Traject"); 
            }
        }

        public async Task<Traject> FindByMultipleIds(int departureCityId, int arrivalCityId)
        { 
            return await _dbContext.Trajects
                .Where(t => t.DepartureCityId == departureCityId && t.ArrivalCityId == arrivalCityId)
                .Include(t => t.TrajectLines)
                    .ThenInclude(t => t.Line)
                    .ThenInclude(t => t.Train)
                .Include(t => t.TrajectLines)
                    .ThenInclude(t => t.Line)
                    .ThenInclude(t => t.ArrivalCity)
                .Include(t => t.TrajectLines)
                    .ThenInclude(t => t.Line)
                    .ThenInclude(t => t.DepartureCity)
                .FirstOrDefaultAsync();
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
