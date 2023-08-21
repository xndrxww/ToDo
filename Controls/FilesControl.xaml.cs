using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ToDo.Controls
{
    /// <summary>
    /// Interaction logic for FilesControl.xaml
    /// </summary>
    public partial class FilesControl : UserControl
    {
        private Models.File File;
        private List<Models.File> FilesList;
        private StackPanel FileStackPanel;

        public FilesControl(Models.File fileModel, List<Models.File> filesList = null, StackPanel fileStackPanel = null)
        {
            InitializeComponent();

            File = fileModel;
            FilesList = filesList;
            FileStackPanel = fileStackPanel;
            fileNameText.Text = File.Name;
        }

        private void deleteFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FilesList.Remove(File); //изменить логику
            FileStackPanel.Children.Clear();
            if (FilesList.Count > 0)
            {
                foreach (var file in FilesList)
                {
                    FileStackPanel.Children.Add(new FilesControl(file));
                }
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
                MessageBox.Show("При открытии файла произошла ошибка");
            }
        }
    }
}
