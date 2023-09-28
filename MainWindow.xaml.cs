using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using ToDo.Controls;
using ToDo.Helpers;
using ToDo.Models;
using ToDo.Pages;

namespace ToDo
{
    public partial class MainWindow : Window
    {
        public static Frame MainFrameInstance;
        public static List<Group> GroupsList;
        private string GroupsFileName = "groups.xml";
        public static StackPanel TasksStackPanel;
        public static StackPanel CompletedTasksStackPanel;
        public static StackPanel FilesStackPanel;
        public static StackPanel GroupStackPanel;
        public static string CurrentPageName;
        public static Guid CurrentGroupId;

        public MainWindow()
        {
            InitializeComponent();

            Deserialize();
            Initialization();
            LoadGroups();
        }

        private void Initialization()
        {
            MainFrameInstance = MainFrame;
            MainFrameInstance.Navigate(new TodayPage());
            GroupStackPanel = groupStackPanel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (GroupsList != null)
            {
                if (System.IO.File.Exists(GroupsFileName))
                    System.IO.File.Delete(GroupsFileName);

                if (GroupsList.Any())
                    Serialize();
            }
        }

        private void Serialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(GroupsList.GetType());

            using (FileStream fs = new FileStream(GroupsFileName, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, GroupsList);
            }
        }

        private void Deserialize()
        {
            try
            {
                if (System.IO.File.Exists(GroupsFileName))
                {
                    GroupsList = new List<Group>();
                    XmlSerializer xmlSerializer = new XmlSerializer(GroupsList.GetType());

                    using (FileStream fs = new FileStream(GroupsFileName, FileMode.OpenOrCreate))
                    {
                        GroupsList = (List<Group>)xmlSerializer.Deserialize(fs);
                    }
                }
            }
            catch (Exception)
            {
                ErrorWindowHelper.ShowError("Десериализация была выполнена некорректно");
            }
        }

        public static void RefreshTasksStackPanel(List<Task> tasks)
        {
            if (TasksStackPanel != null)
            {
                TasksStackPanel.Children.Clear();
                if (tasks?.Any() == true)
                {
                    foreach (var task in tasks.Where(task => !task.IsCompleted))
                    {
                        TasksStackPanel.Children.Add(new TaskControl(task));
                    }
                }
            }
        }

        public static void RefreshCompletedTasksStackPanel()
        {
            if (CompletedTasksStackPanel != null)
            {
                CompletedTasksStackPanel.Children.Clear();
                foreach (var group in GroupsList)
                {
                    if (group.Tasks?.Any() == true)
                    {
                        foreach (var task in group.Tasks.Where(t => t.IsCompleted))
                        {
                            CompletedTasksStackPanel.Children.Add(new TaskControl(task));
                        }
                    }
                }
            }
        }

        public static void LoadGroups()
        {
            GroupStackPanel.Children.Clear();

            if (GroupsList?.Any() == true)
            {
                foreach (var group in GroupsList.Where(g => g.Name != "Сегодня"))
                {
                    GroupStackPanel.Children.Add(new GroupControl(group));
                }
            }
            else
            {
                GroupsList = new List<Group>();
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
            var group = new Group{ Name = "Группа без названия" };

            GroupsList.Add(group);
            GroupStackPanel.Children.Add(new GroupControl(group));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                Application.Current.MainWindow.DragMove();
        }
    }
}
