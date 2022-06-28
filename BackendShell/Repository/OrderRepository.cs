using API.Data;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class OrderRepository : IOrderRepository
    {

        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Order> Create(Order order)
        {
            _context.Orders.Add(order);
           await _context.SaveChangesAsync();
           return order;
        }

        public async Task Delete(int id)
        {
            var book = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(book);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Order>> Get()
        {
            return await _context.Orders.ToListAsync(); 
        }

        public async Task<Order> Get(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public Task Update(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
