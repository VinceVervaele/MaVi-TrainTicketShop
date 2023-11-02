using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Trains_FSD.Models.Data;
using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories.Interfaces;

namespace Trains_FSD.Repositories
{
    public class VacationDAO : IDAO<Vacation>
    {
        private readonly TrainDbContext _dbContext;
        
        public VacationDAO()
        {
            _dbContext = new TrainDbContext();
        }
        
        public Task Add(Vacation entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Vacation entity)
        {
            throw new NotImplementedException();
        }

        public Task<Vacation> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Vacation>> GetAll()
        {
            try
            {
                return await _dbContext.Vacations.ToListAsync();
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

        public Task Update(Vacation entity)
        {
            throw new NotImplementedException();
        }
    }
}
