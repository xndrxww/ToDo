using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using ToDo.Controls;
using ToDo.Helpers;
using ToDo.Models;

namespace ToDo.Windows
{
    public partial class AddTaskWindow : Window
    {
        private const string DirectoryName = "Files";
        private List<Models.File> FilesList = new List<Models.File>();

        public AddTaskWindow()
        {
            InitializeComponent();

            MainWindow.FilesStackPanel = filesStackPanel;
        }

        private void addTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilesList.Count > 0)
                SaveFile();

            var group = GroupHelper.GetGroup();

            if (group.Tasks?.Any() == true)
                group.Tasks.Add(TaskHelper.CreateTask(taskNameText.Text, taskDescriptionText.Text, deadLineTime.SelectedDate, FilesList));
            else
                group.Tasks = new List<Task> { TaskHelper.CreateTask(taskNameText.Text, taskDescriptionText.Text, deadLineTime.SelectedDate, FilesList) };

            MainWindow.RefreshTasksStackPanel(group.Tasks);

            Close();
        }

        private void addFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Multiselect = true
            };

            if (fileDialog.ShowDialog() == true)
            {
                foreach (var file in fileDialog.FileNames)
                {
                    var fileModel = new Models.File
                    {
                        Name = Path.GetFileName(file),
                        UserPath = file
                    };

                    FilesList.Add(fileModel);
                    MainWindow.FilesStackPanel.Children.Add(new FilesControl(fileModel, new Task { Files = FilesList}));
                }

                CenterWindowOnScreen();
            }
        }

        private void deadLineMenuItem_Click(object sender, RoutedEventArgs e)
        {
            deadLineTime.IsDropDownOpen = true;
        }

        private void deadLineTime_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            deadLineMenuItem.Header = deadLineTime.SelectedDate.Value.ToString("dd.MM.yyyy");
        }

        public void SaveFile()
        {
            var files = new List<Models.File>(FilesList);
            FilesList.Clear();

            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);

            foreach (var file in files)
            {
                if (!System.IO.File.Exists($@"{DirectoryName}\{file.Name}"))
                    System.IO.File.Copy($@"{file.UserPath}", $@"{DirectoryName}\{file.Name}");

                var fileModel = new Models.File
                {
                    Name = file.Name,
                    UserPath = $@"{DirectoryName}\{file.Name}"
                };
                FilesList.Add(fileModel);
            }
        }

        private void CenterWindowOnScreen()
        {
            Height = 500;
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;
            Left = (screenWidth / 2) - (windowWidth / 2);
            Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}
