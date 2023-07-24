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
        private string FileName = "tasks.xml";
        public static StackPanel TasksStackPanel;
        public static StackPanel CompletedTasksStackPanel;

        public MainWindow()
        {
            InitializeComponent();
            DeserializeTasks();

            MainFrameInstance = MainFrame;
            MainFrameInstance.Navigate(new TodayPage());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (TasksList != null)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(TasksList.GetType());

                using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, TasksList);
                }
            }
        }

        private void DeserializeTasks()
        {
            if (File.Exists(FileName))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(TasksList.GetType());

                using (FileStream fs = new FileStream("tasks.xml", FileMode.OpenOrCreate))
                {
                    TasksList = (List<Models.Task>)xmlSerializer.Deserialize(fs);
                }
            }
        }

        public static void LoadTasks()
        {
            if (TasksList != null)
            {
                if (CompletedTasksStackPanel != null)
                {
                    CompletedTasksStackPanel.Children.Clear();
                    foreach (var task in TasksList.Where(task => task.IsCompleted))
                    {
                        CompletedTasksStackPanel.Children.Add(new TaskControl(task));
                    }
                    CompletedTasksStackPanel = null;
                }
                else
                {
                    TasksStackPanel.Children.Clear();
                    foreach (var task in TasksList.Where(task => !task.IsCompleted))
                    {
                        TasksStackPanel.Children.Add(new TaskControl(task));
                    }
                }
            }
        }

        private void completedTaskButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrameInstance.Navigate(new CompletedTasksPage());
        }

        private void todayButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrameInstance.Navigate(new TodayPage());
        }
    }
}
