using API.Models;
using System.Threading.Tasks;

namespace API.Repository
{
    public class UserRepository : IUserRepository
    {
        public Task<Order> Authenticate(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<Order> Signup(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
