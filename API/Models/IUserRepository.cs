using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public interface IUserRepository
    {
        Task<Order> Authenticate(User user);
        Task<Order> Signup(User user);

    }
}
