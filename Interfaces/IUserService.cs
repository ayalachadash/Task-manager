using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Services;
using TaskManager.Models;

namespace TaskManager.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();
        User Get(int userId);
        void Add(User user);
        void Update(User user);
        void Delete(int userId);
        int Count { get; }
        bool isExist(string name, string password);
        int findId(string name, string password);
    }
}