﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorChadCalendar.Data.Services
{
    public class TaskService
    {
        ApplicationContext db = new ApplicationContext();
        public IEnumerable<Project> GetProjects(User user)
        {
            return db.Projects.Where(proj => proj.User == user);
        }
        public IEnumerable<Data.Task> GetTasks(User user)
        {
            return db.Tasks.Where(task => task.User == user);
        }
        public Data.Task GetPredecessor(int? id)
        {
            if (id != null)
                return db.Tasks.FirstOrDefault(t => t.Id == id);
            else
                return null;
        }
    }
}