using System;
using System.Windows;
using System.Windows.Controls;
using ToDo.Helpers;

namespace ToDo.Controls
{
    public partial class FilesControl : UserControl
    {
        private Models.File File;
        private Models.Task Task;

        public FilesControl(Models.File fileModel, Models.Task task)
        {
            InitializeComponent();

            Initialization(fileModel, task);
        }

        private void Initialization(Models.File fileModel, Models.Task task)
        {
            File = fileModel;
            Task = task;
            fileNameText.Text = File.Name;
        }

        private void deleteFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Task.Files.Remove(File);
            MainWindow.FilesStackPanel.Children.Clear();
            foreach (var file in Task.Files)
            {
                MainWindow.FilesStackPanel.Children.Add(new FilesControl(file, Task));
            }
        }

        private void openFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(File.UserPath);
            }
            catch (Exception)
            {
                ErrorWindowHelper.ShowError("При открытии файла произошла ошибка");
            }
        }
    }
}
