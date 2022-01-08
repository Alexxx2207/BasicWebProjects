using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.Services
{
    public interface IUserService
    {
        Task AddUser(string username, string email, string password);

        string TryGetUserId(string username, string password);
    }
}
