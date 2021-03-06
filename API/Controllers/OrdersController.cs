using API.Data;
using API.Enums;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private List<Order> orders;
        private OrderType orderint;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all orders
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }
        /// <summary>
        /// Searches of an order.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="ordertype"></param>
        /// <returns>A list of search results</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/order/customer/order
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <response code="404">Returns 404 if </response>
        // GET: api/orders/customer/ordertype
        [HttpGet("{customer}/{ordertype}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Order>>> SearchOrders(string customer, string ordertype)
        {
            if (ordertype != "nul")
            {
                OrderType orderint = (OrderType)Enum.Parse(typeof(OrderType), ordertype, true);
            }

            customer = customer.ToLower();

            if (ordertype != "nul")
            {
                orders = await _context.Orders.Where(order => order.OrderType == orderint).ToListAsync();
            } else if (customer != "nul")
            {
                orders = await _context.Orders.Where(order => order.CustomerName.Contains(customer)).ToListAsync();
            }
             else
            {
                orders = await _context.Orders.ToListAsync();
            }

            if (orders == null)
            {
                return NotFound();
            }

            return orders;
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/orders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates an order.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A newly created order</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/order
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
