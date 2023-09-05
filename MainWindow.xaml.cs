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
        private string FileName = "tasks.xml";
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
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (TasksList != null)
            {
                if (File.Exists(FileName))
                    File.Delete(FileName);

                if (TasksList.Count > 0)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(TasksList.GetType());

                    using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
                    {
                        xmlSerializer.Serialize(fs, TasksList);
                    }
                }
            }
        }

        private void Deserialize()
        {
            if (File.Exists(FileName))
            {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(TasksList.GetType());

                    using (FileStream fs = new FileStream("tasks.xml", FileMode.OpenOrCreate))
                    {
                        TasksList = (List<Models.Task>)xmlSerializer.Deserialize(fs);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("При десериализации произошла ошибка");
                }
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
            if (task.Files !=  null && task.Files.Count > 0)
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
