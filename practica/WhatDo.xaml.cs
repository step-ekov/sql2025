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
using System.Windows.Shapes;

namespace practica
{
    public partial class WhatDo : Window
    {
        public WhatDo()
        {
            InitializeComponent();
        }
        private void dellBtn_Click(object sender, RoutedEventArgs e)
        {
            UserData.isDell = "isdell";
            this.Close();
        }
        private void updtBtn_Click(object sender, RoutedEventArgs e)
        {
            UserData.AddOrUpdt = "updt";
            UserData.isUpdate = "isupdt";
            this.Close();
        }
    }
}
