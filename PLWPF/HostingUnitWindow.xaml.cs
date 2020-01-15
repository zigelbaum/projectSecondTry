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
    /// Interaction logic for HostingUnitWindow.xaml
    /// </summary>
    public partial class HostingUnitWindow : Window
    {
        public HostingUnitWindow()
        {
            InitializeComponent();
        }

        public void PersonalAreaButton_Click(object sender, RoutedEventArgs e)
        {
            PersonalAreaWindow personalAreaWindow;
            GetKey getKey = new GetKey("HostingUnit");
            getKey.ShowDialog();
            if (getKey.numVal != 0)
            {
                personalAreaWindow = new PersonalAreaWindow(getKey.numVal);
                personalAreaWindow.ShowDialog();
            }              
        }

        public void CreateUnitButton_Click(object sender, RoutedEventArgs e)
        {
            UnitPresentationWindow unitPresentationWindow = new UnitPresentationWindow(0);
            unitPresentationWindow.ShowDialog();
        }

        public void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
