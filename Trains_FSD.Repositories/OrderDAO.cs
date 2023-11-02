using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Trains_FSD.Models.Data;
using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories.Interfaces;

namespace Trains_FSD.Repositories
{
    public class OrderDAO : IDAOOrder<Order>
    {
        private readonly TrainDbContext _dbContext;

        public OrderDAO()
        {
            _dbContext = new TrainDbContext();
        }

        public async Task Add(Order entity)
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

        public async Task Delete(Order entity)
        {
            // Remove all tickets associated with the order
            //var tickets = _dbContext.Tickets.Where(t => t.OrderId == entity.Id);
            //_dbContext.Tickets.RemoveRange(tickets);

            // Remove the order entity
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

        public async Task<Order> FindById(int id)
        {
            try
            {
                return await _dbContext.Orders.Where(b => b.Id == id)
                    .Include(a => a.Tickets).
                    ThenInclude(e => e.TicketDetails)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("error DAO orders");
            }
        }

        public Task<Order> FindByMultipleIds(int firstId, int secondId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            try
            {
                return await _dbContext.Orders.Include(a => a.Tickets).ToListAsync();
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

        public async Task Update(Order entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
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

        public async Task<IEnumerable<Order>> FindByCustomerId(string customerId)
        {
            try
            {
                return await _dbContext.Orders.Where(b => b.CustomerId == customerId)
                    .Include(a => a.Tickets)
                    .ThenInclude(e => e.TicketDetails)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("error DAO orders");
            }
        }
    }
}
