using System;

namespace ToDo.Models
{
    public class Task
    {
        public Task()
        {}

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DeadLine { get; set; }
    }
}
