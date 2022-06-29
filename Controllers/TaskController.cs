using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Services;
using TaskManager.Models;
using TaskManager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using TaskManager.controllers;
using Microsoft.AspNetCore.Http;


namespace TaskManager.controllers
{
    [ApiController]
    [Route("Task")]
    public class TaskController : ControllerBase
    {
        private ITaskService TaskService;
        private int userId;
        private string userName;
        public TaskController(ITaskService taskService, IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext.User;
            userName = user.FindFirst("name")?.Value;
            userId = int.Parse(user.FindFirst("UserId")?.Value);
            this.TaskService = taskService;
        }
        [HttpGet]
        [Authorize(Policy = "User")]
        public ActionResult<List<Task>> Get()
        {
            return TaskService.GetAll(userId);
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "User")]

        public ActionResult<Task> Get(int id)
        {
            var Task = TaskService.Get(id);
            if (Task == null)
            {
                return NotFound();
            }
            return Task;
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        public ActionResult Post(Task task)
        {
            task.UserId = userId;
            TaskService.Add(task);
            return CreatedAtAction(nameof(Post), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult Put(int id, Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }
            task.UserId = userId;
            var existingTask = TaskService.Get(id);
            if (existingTask == null)
            {
                return NotFound();
            }
            TaskService.Update(task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult Delete(int id)
        {
            var task = TaskService.Get(id);
            if (task == null)
            {
                return NotFound();
            }
            TaskService.Delete(id);
            return Content(TaskService.Count.ToString());
        }
        [HttpDelete]
        [Authorize(Policy = "User")]
        public ActionResult DeleteTaskIsDoen()
        {
            TaskService.DeleteTaskIsDone();
            return Content(TaskService.Count.ToString());
        }
        [HttpGet]
        [Route("action")]
        public ActionResult getUserName()
        {
            return new OkObjectResult(userName);
        }   
    }
}