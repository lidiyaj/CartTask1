using CartApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly CartDbContext _context;

        public OrderController(CartDbContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetCarts()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetCart(int id)
        {
            var cart = await _context.Orders.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        // PUT: api/Carts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.OrderID)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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

        // POST: api/Carts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            try
            {
                cart.OrderStatus = nameof(OrderStatus.INITIATED);
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();

                Order order = new()
                {
                    CustomerID = cart.CustomerID,
                    CartID = cart.CartID
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var product in cart.Products)
                {
                    var orderItem = new OrderItem
                    {
                        OrderID = order.OrderID,
                        ProductID = product.ProductID,
                        Quantity = product.Quantity,
                        ProductPrice = product.ProductPrice,
                        Total = product.Total
                    };

                    _context.OrderItems.Add(orderItem);

                }
                await _context.SaveChangesAsync();

                var factory = new ConnectionFactory()
                {
                    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
                    Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RABBITMQ_PORT"))
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "orders", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    CartOrder cartOrder = new() { OrderID = cart.OrderID, CartID = cart.CartID, CustomerID = cart.CustomerID, Products = cart.Products };
                    String jsonified = JsonConvert.SerializeObject(cartOrder);
                    var body = Encoding.UTF8.GetBytes(jsonified);

                    channel.BasicPublish(exchange: "", routingKey: "orders", basicProperties: null, body: body);
                    Console.WriteLine(" [x] Sent {0}", cartOrder);
                }

                return CreatedAtAction("GetCart", new { id = cart.CartID }, cart);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartID == id);
        }
    }
}
