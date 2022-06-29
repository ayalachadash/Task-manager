using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Services;
using TaskManager.Models;

namespace TaskManager.Interfaces{
    public interface ITaskService
    {
        List<Task> GetAll(int id);
        Task Get(int id);           
        void Add(Task task);
        void Update(Task task);
        void Delete(int id);
        void DeleteTaskIsDone();
        int Count{get;}
    }
}