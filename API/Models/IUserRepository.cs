using System.Threading.Tasks;

namespace API.Models
{
    public interface IUserRepository
    {
        Task<Order> Authenticate(User user);
        Task<Order> Signup(User user);

    }
}
