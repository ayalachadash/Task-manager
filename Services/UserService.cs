using TaskManager.Models;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Interfaces;
using System.Text.Json;
using System.IO;
namespace TaskManager.Services
{
    public class UserService : IUserService
    {
        public List<User> Users { get; }
        static int counter = 1;

        private static string fileName = "User.json";
        public UserService(/*IWebHostEnvinronment webHost*/)
        {
            using (var jsonFile = File.OpenText(fileName))
            {
                Users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            if(Users.Count()>0){
                counter=Users[Users.Count()-1].UserId+1;
            }
        }
        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(Users));
        }
        public List<User> GetAll() => Users;
        public User Get(int userId) => Users.FirstOrDefault(u => u.UserId==userId);
        public void Add(User user)
        {
            user.UserId = Count;
            Users.Add(user);
            saveToFile();
        }
        public void Update(User user)
        {
            var index = Users.FindIndex(u => u.Password.Equals(user.Password));
            if (index == -1)
                return;
            Users[index] = user;
            saveToFile();
        }
        public void Delete(int userId)
        {
            var user = Get(userId);
            if (user is null)
                return;
            Users.Remove(user);
            saveToFile();
        }
        public bool isExist(string name, string password)
        {
            foreach (var user in Users)
            {
                if (user.UserName.Equals(name) && user.Password.Equals(password))
                {
                    return true;
                }
            }
            return false;
        }
        public int findId(string name, string password)
        {
            var idUser = Users.FirstOrDefault(u => u.UserName.Equals(name) && u.Password.Equals(password));
            return idUser.UserId;
        }
        public int Count => counter++;
        // public User currentUser = null;
    }
}
