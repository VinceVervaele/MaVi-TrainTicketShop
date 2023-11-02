using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories.Interfaces;
using Trains_FSD.Services.Interfaces;

namespace Trains_FSD.Services
{
    public class OrderService : IServiceOrder<Order>
    {

        private IDAOOrder<Order> _orderDAO;

        public OrderService(IDAOOrder<Order> orderDAO)
        {
            _orderDAO = orderDAO;
        }


        public async Task Add(Order entity)
        {
            await _orderDAO.Add(entity);
        }

        public async Task Delete(Order entity)
        {
            await _orderDAO.Delete(entity);
        }

        public async Task<Order> FindById(int id)
        {
            return await _orderDAO.FindById(id);
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _orderDAO.GetAll();
        }

        public async Task Update(Order entity)
        {
            await _orderDAO.Update(entity);
        }

        public async Task<IEnumerable<Order>> FindByCustomerId(string customerId)
        {
            return await _orderDAO.FindByCustomerId(customerId);
        }

    }
}
