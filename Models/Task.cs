using System;
using System.Collections.Generic;

namespace ToDo.Models
{
    public class Task
    {
        public Task()
        {}

        public Guid Id { get; set; }
        public string ControlName => $"taskControl{Id}".Replace("-", string.Empty);
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DeadLine { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsPriority { get; set; }
        public List<File> Files { get; set; }
    }
}
