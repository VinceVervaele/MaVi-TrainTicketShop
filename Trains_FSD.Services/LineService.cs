using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories.Interfaces;
using Trains_FSD.Services.Interfaces;

namespace Trains_FSD.Services
{
    public class LineService : IService<Line>
    {
        private IDAO<Line> _lineDAO;

        public LineService(IDAO<Line> lineDAO)
        {
            _lineDAO = lineDAO;
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
            return await _lineDAO.FindById(id);
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
