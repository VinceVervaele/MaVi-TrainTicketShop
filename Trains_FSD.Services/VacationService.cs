using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories.Interfaces;
using Trains_FSD.Services.Interfaces;

namespace Trains_FSD.Services
{
    public class VacationService : IService<Vacation>
    {

        private IDAO<Vacation> _vacationDAO;

        public VacationService(IDAO<Vacation> vacationDAO)
        {
            _vacationDAO = vacationDAO;
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
            return await _vacationDAO.GetAll();
        }

        public Task Update(Vacation entity)
        {
            throw new NotImplementedException();
        }
    }
}
