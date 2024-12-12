using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Urban_Tide_Bus_Booking_Service_Application.Models;
using Urban_Tide_Bus_Booking_Service_Application.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace Urban_Tide_Bus_Booking_Service_Application.Manager
{
    // ApplicationManager Singleton
    public class UserManager
    {
        private static readonly UserManager _instance = new UserManager();
        private IUserRepository _userRepository;
        //private IBusRepository _busRepository;

        private UserManager()
        {
            // Use configuration to determine which implementation to use
            var repositoryType = GetRepositoryTypeFromConfig(); // Example: "Access" or "Fake"
            _userRepository = CreateUserRepository(repositoryType);
        }

        public static UserManager Instance => _instance;


        // Factory to create appropriate IUserRepository implementation
        private IUserRepository CreateUserRepository(string repositoryType)
        {

            switch (repositoryType)
            {
                case "Access":
                    string accessConnectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Path.Combine(Application.StartupPath, "UsersDB.msacc.accdb")}";

                    _userRepository = new AccessUserRepository(accessConnectionString);
                    break;
                case "Fake":
                    _userRepository = new FakeUserRepository();
                    break;
            }

            return _userRepository;
        }

        // Simulate fetching configuration value
        private string GetRepositoryTypeFromConfig()
        {

            try
            {
                // Read the appsettings.json file
                string jsonString = File.ReadAllText(@"C:\\data\\appsettings.json");

                // Parse the JSON using Newtonsoft.Json
                var config = JsonConvert.DeserializeObject<AppSettings>(jsonString);

                // Return the value of the "mode" field
                return config?.Mode ?? throw new Exception("Mode is not defined in appsettings.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading configuration: {ex.Message}");
                throw;
            }
        }


        // Public methods to interact with IUserRepository
        public List<User> GetAllUsers()
        {
            return _userRepository.GetUsers();
        }

        public void AddUser(User user)
        {
            _userRepository.AddUser(user);
        }

        public User AuthenticateUser(string username, string password)
        {
            var isValid = _userRepository.ValidateUser(username, password);
            return _userRepository.GetUsers().FirstOrDefault(i => i.Username == username && i.Password == password);
        }
    }

    public class AppSettings
    {
        public string Mode { get; set; }
    }
}