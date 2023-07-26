﻿using System;

namespace ToDo.Models
{
    public class Task
    {
        public Task()
        {}

        public int Id { get; set; }
        public string ControlName => $"taskControl{Id}";
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DeadLine { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsOverdue { get; set; }
    }
}
