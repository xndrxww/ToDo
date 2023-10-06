using System;
using System.Linq;
using ToDo.Models;

namespace ToDo.Helpers
{
    public class GroupHelper
    {
        public static Group GetGroup()
        {
            return MainWindow.GroupsList.Where(g => g.Id == MainWindow.CurrentGroupId).FirstOrDefault();
        }

        public static Group GetGroup(Guid id)
        {
            return MainWindow.GroupsList.Where(group => group.Id == id).FirstOrDefault();
        }
    }
}
