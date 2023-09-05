using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class Group
    {
        public Group()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public int GetId()
        {
            if (MainWindow.GroupsList.Any())
                return MainWindow.GroupsList.Count + 1;
            else 
                return 1;
        }
    }
}
