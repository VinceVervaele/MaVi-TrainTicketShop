using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories.Interfaces;
using Trains_FSD.Services.Interfaces;

namespace Trains_FSD.Services
{
    public class CityService : IService<City>
    {

        private IDAO<City> _cityDAO;

        public CityService(IDAO<City> cityDAO)
        {
            _cityDAO = cityDAO;
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
            return await _cityDAO.FindById(id);
        }


        public async Task<IEnumerable<City>> GetAll()
        {
            return await _cityDAO.GetAll();
        }

        public Task Update(City entity)
        {
            throw new NotImplementedException();
        }
    }
}
