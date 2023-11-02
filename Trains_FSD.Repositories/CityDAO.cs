using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Trains_FSD.Models.Data;
using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories.Interfaces;

namespace Trains_FSD.Repositories
{
    public class CityDAO : IDAO<City>
    {
        private readonly TrainDbContext _dbContext;
        
        public CityDAO()
        {
            _dbContext = new TrainDbContext();
        }
        
        public Task Add(City entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(City entity)
        {
            throw new NotImplementedException();
        }

        public async Task<City> FindById(int id)
        {
            try
            {
                return await _dbContext.Cities.Where(b => b.Id == id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("error DAO orders");
            }
        }

        public async Task<IEnumerable<City>> GetAll()
        {
            try
            {
                return await _dbContext.Cities.ToListAsync();
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

        public Task Update(City entity)
        {
            throw new NotImplementedException();
        }
    }
}
