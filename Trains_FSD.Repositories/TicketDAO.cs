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
    public class TicketDAO : IDAO<Ticket>
    {
        private readonly TrainDbContext _dbContext;

        public TicketDAO()
        {
            _dbContext = new TrainDbContext();
        }

        public async Task Add(Ticket entity)
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

        public async Task Delete(Ticket entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;

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

        public Task<Ticket> FindById(int id)
        {
            throw new NotImplementedException();

        }

        public async Task<IEnumerable<Ticket>> GetAll()
        {
            try
            {
                return await _dbContext.Tickets.Include(a => a.TicketDetails).ToListAsync();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Debug.WriteLine("db not found: ", ex.ToString());
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("something went wrong in the db: ", ex.ToString());
                return null;
            }
        }

        public Task Update(Ticket entity)
        {
            throw new NotImplementedException();
        }
    }
}
