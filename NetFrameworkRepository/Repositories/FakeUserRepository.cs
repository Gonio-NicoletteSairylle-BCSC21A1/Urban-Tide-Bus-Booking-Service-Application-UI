using System.Collections.Generic;
using System.Linq;
using Urban_Tide_Bus_Booking_Service_Application.Models;

namespace Urban_Tide_Bus_Booking_Service_Application.Repositories
{
    public class FakeUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>()
        {
            new User() { Username = "admin", Password = "admin", Roles = new []{"admin" } }
        };


        public List<User> GetUsers()
        {
            return _users;
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public bool ValidateUser(string username, string password)
        {
            User user = _users.FirstOrDefault(u => u.Username == username);
            return user != null && user.Password == password;
        }
    }
}