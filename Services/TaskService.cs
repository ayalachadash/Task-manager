using TaskManager.Models;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Interfaces;
using System.Text.Json;
using System.IO;
namespace TaskManager.Services
{
    public class TaskService : ITaskService
    {
        List<Task> Tasks { get; }
        static int counter = 0;
        private static string fileName = "Task.json";
        public TaskService(/*IWebHostEnvinronment webHost*/)
        {
            using (var jsonFile = File.OpenText(fileName))
            {
                Tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            if (Tasks.Count() > 0)
            {
                counter = Tasks[Tasks.Count() - 1].Id + 1;
            }
        }
        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(Tasks));
        }
        public List<Task> GetAll(int id) => Tasks.Where(t => t.UserId == id)?.ToList();
        public Task Get(int id) => Tasks.FirstOrDefault(t => t.Id == id);
        public void Add(Task task)
        {
            task.Id = Count;
            Tasks.Add(task);
            saveToFile();
        }
        public void Update(Task task)
        {
            var index = Tasks.FindIndex(t => t.Id == task.Id);
            if (index == -1)
                return;
            Tasks[index] = task;
            saveToFile();
        }
        public void Delete(int id)
        {
            var task = Get(id);
            if (task is null)
                return;
            Tasks.Remove(task);
            saveToFile();
        }

        public void DeleteTaskIsDone()
        {
            var q = Tasks.Where(t => t.IsDone).ToList();
            if (q is null)
            {
                return;
            }
            foreach (var task in q)
            {
                Tasks.Remove(task);
                saveToFile();

            }
        }

        public int Count => counter++;
    }
}
