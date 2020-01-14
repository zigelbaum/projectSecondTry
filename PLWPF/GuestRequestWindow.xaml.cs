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
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for GuestRequestWindow.xaml
    /// </summary>
    public partial class GuestRequestWindow : Window
    {
        public GuestRequestWindow()
        {
            InitializeComponent();
        }

        private void addRequestButton_Click(object sender, RoutedEventArgs e)
        { 
            GuestPresentation guestPresentationWindow = new GuestPresentation();
            guestPresentationWindow.ShowDialog();
        }

        private void updateRequestButton_Click(object sender, RoutedEventArgs e)
        {
            GuestRequest request;
            GetKey getKey = new GetKey("GuestRequest");
            getKey.ShowDialog();
            if (getKey.numVal != 0)
            {
                request = MainWindow.myBL.FindGuestRequest(getKey.numVal);
                GuestPresentation presentation = new GuestPresentation(request, "Update");
            }
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
