﻿using Microsoft.AspNetCore.Mvc;
using ChadCalendar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using ChadCalendar.TaskHandlers;

namespace ChadCalendar.Controllers
{
    public class TaskController : Controller
    {
        ApplicationContext db = new ApplicationContext();
        ErrorsHandler errorsHandler = new ErrorsHandler();
        private IEnumerable<Project> getProjects(User user)
        {
            return db.Projects.Where(proj => proj.User == user);
        }
        private IEnumerable<Models.Task> getTasks(User user)
        {
            return db.Tasks.Where(task => task.User == user);
        }
        private Models.Task getPredecessor(int? id)
        {
            if (id != null)
                return db.Tasks.FirstOrDefault(t => t.Id == id);
            else
                return null;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            return View(await db.Tasks.Where(task => task.User == user).ToListAsync());
        }

        [Authorize]
        public IActionResult AddTask()
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            ViewBag.Projects = getProjects(user);
            ViewBag.TasksOfProject = getTasks(user);
            Models.Task task = new Models.Task();
            return View(task);
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddTask(Models.Task task)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            ViewBag.Projects = getProjects(user);
            task.User = user;
            task.Predecessor = getPredecessor(task.Predecessor.Id);
            task.Accessed = DateTime.Now;
            task.NRepetitions = 1;
            task.Project = db.Projects.FirstOrDefault(p => p.Id == task.Project.Id); // это странное выражение нужно потому что в модели передается только Id
            if (!task.IsCorrect())
            {
                ViewBag.Error = true;
                ViewBag.TasksOfProject = getTasks(user);
                return View(task);
            }
            db.Add(task);
            db.SaveChanges();
            return Redirect("~/");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            ViewBag.Projects = getProjects(user);
            ViewBag.TasksOfProject = getTasks(user); 
            if (id != null)
            {
                Models.Task task = await db.Tasks.Include(t => t.Project).Include(t => t.Predecessor).FirstOrDefaultAsync(p => p.Id == id);


                if (task != null && task.User == user)
                    return View(task);

            }
            return NotFound();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Models.Task task)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            task.Accessed = DateTime.Now;
            task.Project = db.Projects.FirstOrDefault(p => p.Id == task.Project.Id); // это странное выражение нужно потому что в модели передается только Id
            //Models.Task tempTask = task.Predecessor;
            task.Predecessor = getPredecessor(task.Predecessor.Id);
            //if (task.Predecessor == null)
            //{
            //    db.Tasks.Remove(tempTask);
            //}
            ViewBag.Projects = getProjects(user);
            if (!task.IsCorrect())
            {
                ViewBag.Error = true;
                ViewBag.TasksOfProject = getTasks(user);
                return View(task);
            }
            //task.Predecessor = new Models.Task { Name = "mkkm", TimeTakes = (new TimeSpan(500)), Accessed = DateTime.Now, Id = null };
            db.Tasks.Update(task);
            await db.SaveChangesAsync();
            return Redirect("~/");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeIsCompleted(int? id)
        {
            Models.Task task = db.Tasks.Include(t => t.Project).FirstOrDefault(t => t.Id == id);
            task.IsCompleted = !task.IsCompleted;
            db.Tasks.FirstOrDefault(t => t.Id == id);
            await db.SaveChangesAsync();
            return Redirect($"~/Home/Index/{task.Project.Id}");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Models.Task task = await db.Tasks.FirstOrDefaultAsync(p => p.Id == id);
                if (task != null)
                {
                    db.Tasks.Remove(task);
                    await db.SaveChangesAsync();
                    return Redirect("~/");
                }
            }
            return NotFound();
        }
        public async Task<IActionResult> Mutatuion(Models.Task task)
        {
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            task = db.Tasks.FirstOrDefault(t => task.Id == t.Id); // это странное выражение нужно потому что в модели передается только Id
            var del = db.Tasks.Where(t => t.Predecessor == task);
            foreach (var item in del)
            {
                db.Tasks.Remove(item.Predecessor);
            }
            DateTime dt = DateTime.Now;
            DateTime startsAt = dt.Date.AddHours(dt.Hour).AddMinutes(dt.Minute);
            Event _event = new Event(task, startsAt, 15);
            _event.User = user;
            db.Events.Add(_event);
            db.Tasks.Remove(task);
            await db.SaveChangesAsync();
            return Redirect("~/");
        }
    }
}
