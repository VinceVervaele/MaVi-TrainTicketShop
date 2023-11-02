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
    public class TicketService : IService<Ticket>
    {
        private IDAO<Ticket> _ticketDAO;

        public TicketService(IDAO<Ticket> ticketDAO)
        {
            _ticketDAO = ticketDAO;
        }

        public async Task Add(Ticket entity)
        {
           await _ticketDAO.Add(entity);
        }

        public async Task Delete(Ticket entity)
        {
            await _ticketDAO.Delete(entity);
        }

        public  Task<Ticket> FindById(int orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Ticket>> GetAll()
        {
            return await _ticketDAO.GetAll();
        }

        public Task Update(Ticket entity)
        {
            throw new NotImplementedException();
        }
    }
}
