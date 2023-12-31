﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        public static List<Group> GroupsList = new List<Group>();
        public static StackPanel TasksStackPanel;
        public static StackPanel FilesStackPanel;
        public static StackPanel GroupStackPanel;
        public static Guid CurrentGroupId;
        private string GroupsFileName = "groups.xml";

        public MainWindow()
        {
            InitializeComponent();

            Deserialize();
            Initialization();
            LoadGroups();
        }

        private void Deserialize()
        {
            try
            {
                if (System.IO.File.Exists(GroupsFileName))
                {
                    var xmlSerializer = new XmlSerializer(GroupsList.GetType());

                    using (FileStream fs = new FileStream(GroupsFileName, FileMode.OpenOrCreate))
                    {
                        GroupsList = (List<Group>)xmlSerializer.Deserialize(fs);
                    }
                }
                else
                {
                    GroupsList.Add(AddDefaultGroup());
                }
            }
            catch (Exception)
            {
                ErrorWindowHelper.ShowError("Десериализация была выполнена некорректно");
            }
        }

        private void Initialization()
        {
            MainFrameInstance = MainFrame;
            MainFrameInstance.Navigate(new TodayPage(GroupsList.First())); //Загрузка группы "Сегодня"
            GroupStackPanel = groupStackPanel;
        }

        public static void LoadGroups()
        {
            GroupStackPanel.Children.Clear();

            if (GroupsList?.Any() == true)
            {
                foreach (var group in GroupsList.Where(g => !g.IsDefault))
                {
                    GroupStackPanel.Children.Add(new GroupControl(group));
                }
            }
        }

        private void Serialize()
        {
            var xmlSerializer = new XmlSerializer(GroupsList.GetType());

            using (FileStream fs = new FileStream(GroupsFileName, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, GroupsList);
            }
        }

        public static void RefreshTasksStackPanel(List<Task> tasks = null, bool isCompleted = false)
        {
            if (TasksStackPanel != null)
            {
                TasksStackPanel.Children.Clear();

                if (isCompleted)
                {
                    var completedTasks = GetCompletedTasks();
                    foreach (var task in completedTasks)
                    {
                        TasksStackPanel.Children.Add(new TaskControl(task));
                    }
                }
                else
                {
                    if (tasks?.Any() == true)
                    {
                        foreach (var task in tasks.Where(task => !task.IsCompleted))
                        {
                            TasksStackPanel.Children.Add(new TaskControl(task));
                        }
                    }
                }
            }
        }

        public static List<Task> GetCompletedTasks()
        {
            var completedTasks = new List<Task>();
            foreach (var group in GroupsList)
            {
                if (group.Tasks?.Any() == true)
                {
                    completedTasks.AddRange(group.Tasks.Where(t => t.IsCompleted));
                }
            }

            return completedTasks;
        }

        private void todayMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrameInstance.Navigate(new TodayPage(GroupsList.First())); //Загрузка группы "Сегодня"
        }

        private void completedTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrameInstance.Navigate(new TodayPage(null, isCompleted: true));
        }

        private void addGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var group = new Group { Name = "Группа без названия" };

            GroupsList.Add(group);
            GroupStackPanel.Children.Add(new GroupControl(group));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (GroupsList != null)
            {
                if (System.IO.File.Exists(GroupsFileName))
                {
                    System.IO.File.Delete(GroupsFileName);
                }

                if (GroupsList.Any())
                {
                    Serialize();
                }
            }
        }

        public static void CheckTaskAndRefresh(Group group, Task task)
        {
            if (task.IsCompleted)
            {
                RefreshTasksStackPanel(isCompleted: true);
            }
            else
            {
                RefreshTasksStackPanel(group.Tasks);
            }
        }

        private Group AddDefaultGroup()
        {
            return new Group
            {
                Id = Guid.Empty,
                Name = "Сегодня",
                IsDefault = true
            };
        }
    }
}
