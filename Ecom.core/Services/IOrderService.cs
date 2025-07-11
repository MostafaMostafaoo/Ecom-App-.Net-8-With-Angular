using Ecom.core.DTO;
using Ecom.core.Entity.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Services
{
    public interface IOrderService
    {
        Task<Orders> CreateOrdersAsync(OrderDTO orderDTO , string BuyerEmail);
        Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail);

        Task<OrderToReturnDTO> GetOrderByIdAsync(int Id, string BuyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync(); 

    }
}
