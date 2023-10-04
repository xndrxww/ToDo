using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ToDo.Controls;
using ToDo.Models;
using ToDo.Windows;

namespace ToDo.Pages
{
    public partial class TodayPage : Page
    {
        private Group Group;
        public bool IsCompleted;

        public TodayPage(Group group, bool isCompleted = false)
        {
            InitializeComponent();

            IsCompleted = isCompleted;

            Initialization(group);
        }

        private void Initialization(Group group)
        {
            MainWindow.TasksStackPanel = tasksStackPanel;

            if (IsCompleted)
            {
                addTaskMenu.Visibility = Visibility.Collapsed;
                sortTaskMenu.Visibility = Visibility.Collapsed;
                //TODO сделать поиск по выполненным задачам
                pageNameText.Text = "Выполненные задачи";
                LoadCompletedTasks();
            }
            else
            {
                pageNameText.Text = group.Name;
                Group = group;
                MainWindow.CurrentGroupId = Group.Id;

                if (Group.IsDefault && Group.Name == "Сегодня")
                    SetTodayDate();

                MainWindow.RefreshTasksStackPanel(Group.Tasks);
            }
        }

        private void LoadCompletedTasks()
        {
            var completedTasks = MainWindow.GetCompletedTasks();
            MainWindow.RefreshTasksStackPanel(completedTasks, isCompleted: true);
        }

        private void addTaskMenu_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow();
            addTaskWindow.ShowDialog();
        }

        private void SetTodayDate()
        {
            todayDateText.Visibility = Visibility.Visible;
            todayDateText.Text = DateTime.Now.ToShortDateString();
        }

        private void searhText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(searhText.Text))
            {
                string search = searhText.Text;

                tasksStackPanel.Children.Clear();

                if (IsCompleted)
                {
                    var completedTasks = MainWindow.GetCompletedTasks();
                    foreach (var task in completedTasks)
                    {
                        //TODO вынести в отдельный метод
                        if (task.Name.ToLower().StartsWith(search.Trim().ToLower()) || task.Description.ToLower().StartsWith(search.Trim().ToLower()))
                        {
                            if (task.DeadLine != null && task.DeadLine < DateTime.Now)
                                task.IsOverdue = true;

                            var taskControl = new TaskControl(task);
                            tasksStackPanel.Children.Add(taskControl);
                        }
                    }
                }
                else
                {
                    if (Group?.Tasks?.Any() == true)
                    {
                        foreach (var task in Group.Tasks.Where(t => !t.IsCompleted))
                        {
                            if (task.Name.ToLower().StartsWith(search.Trim().ToLower()) || task.Description.ToLower().StartsWith(search.Trim().ToLower()))
                            {
                                if (task.DeadLine != null && task.DeadLine < DateTime.Now)
                                    task.IsOverdue = true;

                                var taskControl = new TaskControl(task);
                                tasksStackPanel.Children.Add(taskControl);
                            }
                        }
                    }
                }
            }
            else
            {
                if (Group?.Tasks?.Any() == true)
                {
                    MainWindow.RefreshTasksStackPanel(Group.Tasks);
                }
                else
                {
                    MainWindow.RefreshTasksStackPanel(isCompleted: true);
                }
            }
        }

        private void sortTaskByDateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var tasks = Group.Tasks.OrderBy(t => t.DeadLine.HasValue).ThenBy(p => p.DeadLine).ToList();
            MainWindow.RefreshTasksStackPanel(tasks);
        }

        private void sortTaskByPriority_Click(object sender, RoutedEventArgs e)
        {
            var tasks = Group.Tasks.OrderByDescending(t => t.IsPriority).ToList();
            MainWindow.RefreshTasksStackPanel(tasks);
        }
    }
}
