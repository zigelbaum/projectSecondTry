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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for DirectorWindow.xaml
    /// </summary>
    public partial class DirectorWindow : Window
    {
        public DirectorWindow()
        {
            InitializeComponent();
        }

        private void GuestQuery_Click(object sender, RoutedEventArgs e)
        {
            guestQueryWindow requestQueryWindow = new guestQueryWindow();
            requestQueryWindow.ShowDialog();
        }

        private void HostingQuery_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OrderQuery_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
