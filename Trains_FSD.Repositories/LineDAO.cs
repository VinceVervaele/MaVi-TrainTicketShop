using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains_FSD.Models.Data;
using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories.Interfaces;

namespace Trains_FSD.Repositories
{
    public class LineDAO : IDAO<Line>
    {
        private readonly TrainDbContext _dbContext;

        public LineDAO()
        {
            _dbContext = new TrainDbContext();
        }

        public Task Add(Line entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Line entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Line> FindById(int id)
        {
            try
            {
                return await _dbContext.Lines.Where(a => a.Id == id)
                    .Include(a => a.ArrivalCity)
                    .Include(a => a.DepartureCity)
                    .Include(a => a.Train)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            { throw new Exception("error DAO Line"); }
        }

        public Task<IEnumerable<Line>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(Line entity)
        {
            throw new NotImplementedException();
        }
    }
}
