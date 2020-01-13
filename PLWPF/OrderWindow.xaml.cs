using BE;
using BL;
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
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        #region variable
        IBL myBl = BL.FactoryBL.getBL("XML");
        public List<Order> listOrders;
        HostingUnit host;
        #endregion

        public OrderWindow()
        {
            InitializeComponent();
            cbOrderstList.Visibility = Visibility.Hidden;
            UploadOrderButton.Visibility = Visibility.Hidden;
            CreateOrderButton.Visibility = Visibility.Hidden;
        }

        private void UploadOrderButton_Click(object sender, RoutedEventArgs e)
        {
            UploadOrderWindow upload_ord_Window = new UploadOrderWindow();
            upload_ord_Window.ShowDialog();
        }

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            CreateOrderWindow new_ord_Window = new CreateOrderWindow();
            new_ord_Window.ShowDialog();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32 hostID = Int32.Parse(tbhostID.Text);
                List<HostingUnit> hostList = myBl.getHostingUnits(h => h.Owner.ID == hostID);
                host = hostList[0];
                listOrders = myBl.getOrders(o => o.HostingUnitKey == host.HostingUnitKey);
                cbOrderstList.Visibility = Visibility.Visible;
                UploadOrderButton.Visibility = Visibility.Visible;
                CreateOrderButton.Visibility = Visibility.Visible;
                ConButton.Visibility = Visibility.Hidden;
            }
            catch(Exception ex)
            {
                var result = MessageBox.Show(ex.Message + ". whould you like to retry?", "registration action failed", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                if (MessageBoxResult.No == result)
                {
                    Close();
                    return;
                }
                else
                    return;
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void cbOrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //לא קורה כלום זו סתם תצוגה או שנעשה שזה יעבור להצגת ההזמנה?
        }
    }
}
