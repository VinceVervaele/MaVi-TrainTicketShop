using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories;
using Trains_FSD.Repositories.Interfaces;
using Trains_FSD.Services.Interfaces;

namespace Trains_FSD.Services
{
    public class TicketDetailService : IServiceTicketDetails<TicketDetail>
    {
        private IDAOTicketDetails<TicketDetail> _ticketDetailDAO;

        public TicketDetailService(IDAOTicketDetails<TicketDetail> ticketDetailDAO)
        {
            _ticketDetailDAO = ticketDetailDAO;
        }
        public async Task Add(TicketDetail entity)
        {
            await _ticketDetailDAO.Add(entity);
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
            return await _ticketDetailDAO.FindTicketsByDeparture(departureDate, trajectId, type);
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
