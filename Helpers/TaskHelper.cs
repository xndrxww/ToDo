using System;
using System.Collections.Generic;
using System.Linq;
using ToDo.Models;

namespace ToDo.Helpers
{
    public class TaskHelper
    {
        public static Task GetTask(Group group, Guid id)
        {
            return group.Tasks.Where(t => t.Id == id).FirstOrDefault();
        }

        public static Task CopyTask(string name, string description, DateTime? deadLine, bool isCompleted, bool isOverdue, bool isPriority, List<File> files)
        {
            return new Task
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                DeadLine = deadLine,
                IsCompleted = isCompleted,
                IsOverdue = isOverdue,
                IsPriority = isPriority,
                Files = files
            };
        }

        public static Task CreateTask(string name, string description, DateTime? deadLine, List<File> files)
        {
            return new Task
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                DeadLine = deadLine ?? DateTime.Now,
                Files = files.Count > 0 ? files : null,
            };
        }
    }
}
