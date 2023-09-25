using System.Collections.Generic;
using System;
using System.Linq;
using ToDo.Models;

namespace ToDo.Helpers
{
    public static class GroupHelper
    {
        public static Group GetGroup()
        {
            return MainWindow.GroupsList.Where(g => g.Id == MainWindow.CurrentGroupId).FirstOrDefault();
        }

        public static Group GetGroup(string groupName)
        {
            return MainWindow.GroupsList.Where(g => g.Name == groupName).FirstOrDefault();
        }

        public static Group GetGroup(Guid id)
        {
            return MainWindow.GroupsList.Where(group => group.Id == id).FirstOrDefault();
        }

        public static Group CreateGroup(Task task)
        {
            var group = new Group();
            group.Name = MainWindow.CurrentPageName;
            group.Tasks = new List<Task> { TaskHelper.CreateTask(task.Name, task.Description, task.DeadLine, task.Files) };

            if (MainWindow.CurrentPageName == "Сегодня")
                group.Id = Guid.Empty;

            return group;
        }
    }
}
