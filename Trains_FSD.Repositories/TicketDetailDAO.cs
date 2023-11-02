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
    public class TicketDetailDAO : IDAOTicketDetails<TicketDetail>
    {
        private readonly TrainDbContext _dbContext;

        public TicketDetailDAO()
        {
            _dbContext = new TrainDbContext();
        }
        public async Task Add(TicketDetail entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public Task Delete(TicketDetail entity)
        {
            throw new NotImplementedException();
        }

        public Task<TicketDetail> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TicketDetail>> FindTicketsByDeparture(DateTime departureDate, int trajectId, string type)
        {
            return await _dbContext.TicketDetails
                .Where(td => td.DepartureDate.Date == departureDate.Date &&  td.TrajectId == trajectId && td.Type == type)
                .ToListAsync();
        }

        public Task<IEnumerable<TicketDetail>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(TicketDetail entity)
        {
            throw new NotImplementedException();
        }
    }
}
