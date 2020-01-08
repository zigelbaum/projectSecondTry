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
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IBL myBL= FactoryBL.getBL("List");
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HostinGUnitButton_Click(object sender, RoutedEventArgs e)
        {
            HostingUnitWindow unitWindow = new HostingUnitWindow();
            unitWindow.ShowDialog();
        }

        private void GuestRequestButton_Click(object sender, RoutedEventArgs e)
        {
            GuestRequestWindow requestWindow = new GuestRequestWindow();
            requestWindow.ShowDialog();
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow();
            orderWindow.ShowDialog();
        }
    }
}
