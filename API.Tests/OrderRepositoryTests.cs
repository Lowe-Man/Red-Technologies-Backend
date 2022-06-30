using API.Data;
using API.Enums;
using API.Models;
using API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Tests
{
    [TestFixture]
    public class OrderRepositoryTests
    {
        private IConfigurationRoot _configuration;
        private DbContextOptions<ApplicationDbContext> _options;
        private Order order;


        public OrderRepositoryTests()
        {
            var builder = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.Development.json");
            _configuration = builder.Build();
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
                .Options;


        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            order = new Order()
            {
                CreatedByUserName = "Alex",
                CreatedDate = DateTime.Now,
                OrderType = (OrderType)Enum.Parse(typeof(OrderType), "Standard", true),
                CustomerName = "Dollar Tree"
            };
        }

        [Test]
        public async Task Save_OrderAsync()
        {
            using (var _context = new ApplicationDbContext(_options))
            {
                var repository = new OrderRepository(_context);
                await repository.Create(order);
            }

            using (var _context = new ApplicationDbContext(_options))
            {
                var orderFromDb = await _context.Orders.FirstOrDefaultAsync(o => o.CustomerName.Equals("Dollar Tree"));
                Assert.IsNotNull(orderFromDb);
            }
        }

        [TearDown]
        public void Tear_Down()
        {
            using (var _context = new ApplicationDbContext(_options))
            {
                _context.Orders.Attach(order);
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

    }
}
