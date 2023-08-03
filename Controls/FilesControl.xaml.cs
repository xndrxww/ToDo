using System.Windows.Controls;

namespace ToDo.Controls
{
    /// <summary>
    /// Interaction logic for FilesControl.xaml
    /// </summary>
    public partial class FilesControl : UserControl
    {
        public FilesControl(string fileName)
        {
            InitializeComponent();

            fileNameText.Text = fileName;
        }
    }
}
