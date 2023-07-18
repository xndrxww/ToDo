using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDo.Controls
{
    /// <summary>
    /// Interaction logic for TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {
        public TaskControl(Models.Task taskModel)
        {
            InitializeComponent();
            SetData(taskModel);
        }

        private void completeTaskCheck_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Выполнено");
        }

        private void SetData(Models.Task taskModel)
        {
            taskNameText.Text = taskModel.Name;
            taskDesciptionText.Text = taskModel.Description;
            
            if (taskModel.DeadLine != null)
                deadLineText.Text = taskModel.DeadLine.Value.ToShortDateString();

            if (taskModel.IsOverdue)
                deadLineText.Foreground = new SolidColorBrush(Colors.Salmon);
        }
    }
}
