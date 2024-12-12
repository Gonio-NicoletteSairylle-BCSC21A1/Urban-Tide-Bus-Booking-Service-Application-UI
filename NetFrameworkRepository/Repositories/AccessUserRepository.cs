using System.Collections.Generic;
using System.Data.OleDb;
using Urban_Tide_Bus_Booking_Service_Application.Models;

namespace Urban_Tide_Bus_Booking_Service_Application.Repositories
{
    public class AccessUserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public AccessUserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();

            using (var connection = new OleDbConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Username, Password FROM Users";

                using (var command = new OleDbCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Username = reader.GetString(0),
                            Password = reader.GetString(1)
                        });
                    }
                }
            }

            return users;
        }

        public void AddUser(User user)
        {
            using (var connection = new OleDbConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";

                using (var command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool ValidateUser(string username, string password)
        {
            using (var connection = new OleDbConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";

                using (var command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
    }
}