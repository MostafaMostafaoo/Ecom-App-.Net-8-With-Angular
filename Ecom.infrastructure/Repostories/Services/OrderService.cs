using AutoMapper;
using Ecom.core.DTO;
using Ecom.core.Entity.Order;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repostories.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbcontext _context;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, AppDbcontext context, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Orders> CreateOrdersAsync(OrderDTO orderDTO, string BuyerEmail)
        {
            var basket = await _unitOfWork.CustomerBasket.GetBasketAsync(orderDTO.basketId);

            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (var item in basket.basketItems)
            {
                var Product = await _unitOfWork.ProductRepositry.GetByIdAsync(item.Id);
                var orderItem = new OrderItem(Product.Id, item.Image, Product.Name, item.Price, item.Quntity);
                orderItems.Add(orderItem);


            }

            var deliverMethod = await _context.DeliveryMethods.FirstOrDefaultAsync(m => m.Id == orderDTO.deliveryMethodId);

            var subTotal = orderItems.Sum(m => m.Price * m.Quntity);

            var ship = _mapper.Map<ShippingAddress>(orderDTO.shipAddress);

            var order = new Orders(BuyerEmail, subTotal, ship, deliverMethod, orderItems);

            await _context.Orders.AddAsync(order);

            await _context.SaveChangesAsync();
            await _unitOfWork.CustomerBasket.DeleteBasketAsync(orderDTO.basketId);
            return order;
        }

    
       


        public async Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            var orders = await _context.Orders.Where(m => m.BuyerEmail == BuyerEmail)
                .Include(inc => inc.orderItems).Include(inc => inc.deliveryMethod)
                .ToListAsync();
            var result = _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);
            return result;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
       => await _context.DeliveryMethods.AsNoTracking().ToListAsync();


        public async Task<OrderToReturnDTO> GetOrderByIdAsync(int Id, string BuyerEmail)
        {
            var order = await _context.Orders.Where(m => m.Id == Id && m.BuyerEmail == BuyerEmail)
                 .Include(inc => inc.orderItems).Include(inc => inc.deliveryMethod)
                 .FirstOrDefaultAsync();
            var  result = _mapper.Map<OrderToReturnDTO>(order);
            return result;
        }

    
    }
}
