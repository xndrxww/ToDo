﻿using System;
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
        //public string TaskName;
        //public string TaskDescription;

        public TaskControl()
        {
            InitializeComponent();

            //taskNameText.Text = TaskName;
            //taskDesciptionText.Text = TaskDescription;
        }
    }
}
