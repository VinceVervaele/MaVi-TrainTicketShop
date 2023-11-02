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
    public class TrajectLineDAO : IDAO<TrajectLine>
    {

        private readonly TrainDbContext _dbContext;

        public TrajectLineDAO()
        {
            _dbContext = new TrainDbContext();
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
            try
            {
                return await _dbContext.TrajectLines
                    .Where(a => a.TrajectId == id)
                    .Include(a => a.Line)
                    .ThenInclude(a => a.Train)
                    .Include(a => a.Line)
                    .ThenInclude(a => a.ArrivalCity)
                    .Include(a => a.Line)
                    .ThenInclude(a => a.DepartureCity)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("error DAO Traject");
            }
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
