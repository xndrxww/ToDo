using System.Windows;

namespace ToDo.Windows
{
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(string errorText)
        {
            InitializeComponent();

            errorTxt.Text = errorText;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
