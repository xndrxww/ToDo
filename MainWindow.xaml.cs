using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using ToDo.Controls;
using ToDo.Pages;

namespace ToDo
{
    public partial class MainWindow : Window
    {
        public static Frame MainFrameInstance;
        public static List<Models.Task> TasksList = new List<Models.Task>();
        public static List<Models.Group> GroupsList = new List<Models.Group>();
        public static List<Models.File> FilesList = new List<Models.File>();
        private string TasksFileName = "tasks.xml";
        private string GroupsFileName = "groups.xml";
        public static StackPanel TasksStackPanel;
        public static StackPanel CompletedTasksStackPanel;
        public static StackPanel FilesStackPanel;
        public static StackPanel GroupStackPanel;

        public MainWindow()
        {
            InitializeComponent();
            Deserialize();

            MainFrameInstance = MainFrame;
            MainFrameInstance.Navigate(new TodayPage());
            GroupStackPanel = groupStackPanel;
            LoadGroups();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (TasksList.Any())
            {
                if (File.Exists(TasksFileName))
                    File.Delete(TasksFileName);

                XmlSerializer xmlSerializer = new XmlSerializer(TasksList.GetType());

                using (FileStream fs = new FileStream(TasksFileName, FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, TasksList);
                }
            }

            if (GroupsList.Any())
            {
                if (File.Exists(GroupsFileName))
                    File.Delete(GroupsFileName);

                XmlSerializer xmlSerializer = new XmlSerializer(GroupsList.GetType());

                using (FileStream fs = new FileStream(GroupsFileName, FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, GroupsList);
                }
            }
        }

        private void Deserialize()
        {
            try
            {
                if (File.Exists(TasksFileName))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(TasksList.GetType());

                    using (FileStream fs = new FileStream(TasksFileName, FileMode.OpenOrCreate))
                    {
                        TasksList = (List<Models.Task>)xmlSerializer.Deserialize(fs);
                    }
                }

                if (File.Exists(GroupsFileName))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(GroupsList.GetType());

                    using (FileStream fs = new FileStream(GroupsFileName, FileMode.OpenOrCreate))
                    {
                        GroupsList = (List<Models.Group>)xmlSerializer.Deserialize(fs);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("При десериализации произошла ошибка");
            }
        }

        public static void LoadTasks(List<Models.Task> tasks)
        {
            var tasksList = new List<Models.Task>();

            if (tasks != null)
                tasksList = tasks;
            else
                tasksList = TasksList;

            if (tasksList != null)
            {
                if (CompletedTasksStackPanel != null)
                {
                    CompletedTasksStackPanel.Children.Clear();
                    foreach (var task in tasksList.Where(task => task.IsCompleted))
                    {
                        CompletedTasksStackPanel.Children.Add(new TaskControl(task));
                    }
                }
                else
                {
                    TasksStackPanel.Children.Clear();
                    foreach (var task in tasksList.Where(task => !task.IsCompleted))
                    {
                        TasksStackPanel.Children.Add(new TaskControl(task));
                    }
                }
            }
        }

        public static void LoadFiles(Models.Task task)
        {
            if (task.Files.Any())
            {
                FilesList = new List<Models.File>();
                foreach (var file in task.Files)
                {
                    FilesList.Add(file);
                }
            }
            else
            {
                FilesList = new List<Models.File>();
            }
        }

        public static void LoadGroups()
        {
            GroupStackPanel.Children.Clear();
            if (GroupsList.Any())
            {
                foreach (var group in GroupsList)
                {
                    GroupStackPanel.Children.Add(new GroupControl(group));
                }
            }
            else
            {
                GroupsList = new List<Models.Group>();
            }
        }

        private void todayMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CompletedTasksStackPanel = null;
            MainFrameInstance.Navigate(new TodayPage());
        }

        private void completedTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrameInstance.Navigate(new CompletedTasksPage());
        }

        private void addGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var group = new Models.Group
            {
                Name = "Группа без названия"
            };
            group.Id = group.GetId();

            GroupsList.Add(group);
            GroupStackPanel.Children.Add(new GroupControl(group));
        }
    }
}
