using System.Collections.Generic;
using System.Linq;

namespace ToDo.Models
{
    public class Group
    {
        public Group()
        {}

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Models.Task> Tasks { get; set; }

        public int GetId()
        {
            if (MainWindow.GroupsList.Any())
                return MainWindow.GroupsList.Count + 1;
            else 
                return 1;
        }
    }
}
