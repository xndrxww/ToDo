using System;
using System.Collections.Generic;

namespace ToDo.Models
{
    public class Group
    {
        public Group()
        { }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string MenuItemName => $"menuItem{Id}".Replace("-", string.Empty);
        public List<Task> Tasks { get; set; }
        public bool IsDefault { get; set; }
    }
}
