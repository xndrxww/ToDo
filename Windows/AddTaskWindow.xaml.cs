using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using ToDo.Controls;

namespace ToDo.Windows
{
    /// <summary>
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        private const string DirectoryName = "Files";

        public AddTaskWindow()
        {
            InitializeComponent();
            MainWindow.FilesStackPanel = filesStackPanel;
        }

        private void addTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var taskModel = new Models.Task
            {
                Id = MainWindow.TasksList.Count + 1,
                Name = taskNameText.Text,
                Description = taskDescriptionText.Text,
                DeadLine = deadLineTime.SelectedDate,
                Files = MainWindow.FilesList.Count > 0 ? MainWindow.FilesList : null
            };

            if (MainWindow.FilesList.Count > 0)
                SaveFile(MainWindow.FilesList);

            MainWindow.TasksList.Add(taskModel);
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

                    MainWindow.FilesList.Add(fileModel);
                    MainWindow.FilesStackPanel.Children.Add(new FilesControl(fileModel));
                }

                Height = 500;
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

        public void SaveFile(List<Models.File> files)
        {
            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);

            foreach (var file in files)
            {
                if (!File.Exists($@"{DirectoryName}\{file.Name}"))
                    File.Copy($@"{file.UserPath}", $@"{DirectoryName}\{file.Name}");
            }
        }
        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;
            Left = (screenWidth / 2) - (windowWidth / 2);
            Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}
