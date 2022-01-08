using BattleCards.Data;
using BattleCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext db;

        public UserService(ApplicationDbContext db)
        {
            this.db = db;   
        }

        public async Task AddUser(string username, string email, string password)
        {
           db.Users.Add(new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username, 
                Email = email,
                Password = HashPassword(password)
           });

            await db.SaveChangesAsync();
        }

        public string TryGetUserId(string username, string password)
        {
            var user = this.db.Users.FirstOrDefault(x => x.Username == username);
            if (user?.Password != HashPassword(password))
            {
                return null;
            }

            return user.Id;
        }

        private string HashPassword(string password)
        {
            using (var alg = SHA512.Create())
                return alg.ComputeHash(Encoding.UTF8.GetBytes(password))
                          .Aggregate(new StringBuilder(), (sb, x) => sb.Append(x.ToString("x2")))
                          .ToString();
        }
    }
}
