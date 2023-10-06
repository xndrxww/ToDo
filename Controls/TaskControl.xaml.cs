using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ToDo.Helpers;
using ToDo.Models;
using ToDo.Windows;

namespace ToDo.Controls
{
    public partial class TaskControl : UserControl
    {
        private Models.Task Task;
        private Group CurrentGroup = GroupHelper.GetGroup();

        public TaskControl(Models.Task taskModel)
        {
            InitializeComponent();

            Initialization(taskModel);
        }

        private void Initialization(Models.Task taskModel)
        {
            Task = taskModel;
            DataContext = SetData();
        }

        private void completeTaskCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (CurrentGroup.Tasks != null)
            {
                var task = CurrentGroup.Tasks.Where(t => !t.IsCompleted && t.ControlName == taskControl.Name).FirstOrDefault();
                task.IsCompleted = true;
            }

            MainWindow.RefreshTasksStackPanel(CurrentGroup.Tasks);
        }

        private Models.Task SetData()
        {
            taskControl.Name = Task.ControlName;

            if (Task.DeadLine != null && Task.DeadLine < DateTime.Now)
            {
                Task.IsOverdue = true;
            }
            else
            {
                Task.IsOverdue = false;
            }

            if (Task.DeadLine != null)
            {
                taskDeadLineText.Text = Task.DeadLine.Value.ToShortDateString();
            }

            if (Task.IsOverdue)
            {
                taskDeadLineText.Foreground = new SolidColorBrush(Colors.Salmon);
            }

            if (Task.IsPriority)
            {
                priorityTaskImage.Source = new BitmapImage(new Uri(@"\Icons\filled_star.png", UriKind.Relative));
                priorityTaskImage.Opacity = 1;
            }
            else
            {
                priorityTaskImage.Source = new BitmapImage(new Uri(@"\Icons\star.png", UriKind.Relative));
                priorityTaskImage.Opacity = 0.3;
            }

            if (Task.IsCompleted)
            {
                completeTaskCheck.Visibility = Visibility.Collapsed;
                restoreTaskMenuItem.Visibility = Visibility.Visible;
                groupTaskMenuItem.Visibility = Visibility.Collapsed;
            }

            if (Task.Files != null && Task.Files.Count > 0)
            {
                attachedFilesStackPanel.Visibility = Visibility.Visible;
            }

            return Task;
        }

        private void deleteTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentGroup.Tasks != null)
            {
                CurrentGroup.Tasks.Remove(Task);
                MainWindow.CheckTaskAndRefresh(CurrentGroup, Task);
            }
        }

        private void EditTask()
        {
            if (!string.IsNullOrEmpty(taskDeadLineText.Text))
            {
                Task.DeadLine = DateTime.Parse(taskDeadLineText.Text);
            }
            else
            {
                Task.DeadLine = DateTime.Now;
            }

            EditTaskWindow editTaskWindow = new EditTaskWindow(Task);
            editTaskWindow.ShowDialog();
        }

        private void editTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditTask();
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditTask();
        }

        private void copyTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentGroup != null)
            {
                var copyTask = TaskHelper.CopyTask(Task.Name, Task.Description, Task.DeadLine, Task.IsCompleted, Task.IsOverdue, Task.IsPriority, Task.Files);
                CurrentGroup.Tasks.Add(copyTask);

                MainWindow.CheckTaskAndRefresh(CurrentGroup, copyTask);
            }
        }

        private void priorityTaskImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Task.IsPriority)
            {
                Task.IsPriority = false;
            }
            else
            {
                Task.IsPriority = true;
            }

            MainWindow.CheckTaskAndRefresh(CurrentGroup, Task);
        }

        private void restoreTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Task.IsCompleted = false;

            MainWindow.RefreshTasksStackPanel(isCompleted: true);
        }

        private void menu_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            LoadGroups();
        }

        private void LoadGroups()
        {
            groupTaskMenuItem.Items.Clear();
            if (MainWindow.GroupsList.Any())
            {
                foreach (var group in MainWindow.GroupsList.Where(g => !g.IsDefault))
                {
                    var menuItem = new MenuItem
                    {
                        Header = group.Name,
                        Name = group.MenuItemName
                    };
                    groupTaskMenuItem.Items.Add(menuItem);
                }
            }
        }

        private void groupTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = e.Source as MenuItem;

            if (Guid.TryParse(menuItem.Name.Replace("menuItem", string.Empty), out Guid groupId))
            {
                var swapGroup = GroupHelper.GetGroup(groupId);

                if (swapGroup != null)
                {
                    SetTaskToGroup(Task, swapGroup);
                }
            }
        }

        private void SetTaskToGroup(Models.Task task, Models.Group swapGroup)
        {
            CurrentGroup.Tasks.Remove(Task);

            if (swapGroup.Tasks != null)
            {
                swapGroup.Tasks.Add(task);
            }
            else
            {
                swapGroup.Tasks = new List<Models.Task> { task };
            }

            MainWindow.RefreshTasksStackPanel(CurrentGroup.Tasks);
        }
    }
}
