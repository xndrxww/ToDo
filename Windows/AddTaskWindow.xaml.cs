using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using ToDo.Controls;
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

            var group = MainWindow.GroupsList.Where(g => g.Name == MainWindow.CurrentPageName).FirstOrDefault();
            if (group != null)
            {
                if (group.Tasks?.Any() == true)
                    group.Tasks.Add(CreateTask(group));
                else
                    group.Tasks = new List<Task> { CreateTask(group) };
            }
            else
            {
                group = CreateGroup();
                MainWindow.GroupsList.Add(group);
            }

            MainWindow.RefreshTasksStackPanel(group.Tasks);

            Close();
        }

        private Group CreateGroup()
        {
            var group = new Group();
            group.Name = MainWindow.CurrentPageName;
            group.Tasks = new List<Task> { CreateTask(group) };
            group.Id = group.GetId();

            return group;
        }

        private Task CreateTask(Group group)
        {
            return new Task
            {
                Id = group.Tasks != null ? group.Tasks.Count + 1 : 1,
                Name = taskNameText.Text,
                Description = taskDescriptionText.Text,
                DeadLine = deadLineTime.SelectedDate,
                Files = FilesList.Count > 0 ? FilesList : null,
            };
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
                    MainWindow.FilesStackPanel.Children.Add(new FilesControl(fileModel));
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
