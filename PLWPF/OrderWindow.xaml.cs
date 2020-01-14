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
        HostingUnit unit;
        public Int32 hostID;
        #endregion

        public OrderWindow()
        {
            InitializeComponent();
            UploadOrderButton.Visibility = Visibility.Hidden;
            CreateOrderButton.Visibility = Visibility.Hidden;
            //Host host;
            GetKey getKey = new GetKey("Host");
            getKey.ShowDialog();
            Int32 hostID = getKey.numVal;
            if (hostID != 0)         
               getOrderList();
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

        private void getOrderList()
        {
            List<HostingUnit> hostList = myBl.getHostingUnits(h => h.Owner.ID == hostID);
            unit = hostList[0];
            listOrders = myBl.getOrders(o => o.HostingUnitKey == unit.HostingUnitKey);
            cbOrderstList.Visibility = Visibility.Visible;
            UploadOrderButton.Visibility = Visibility.Visible;
            CreateOrderButton.Visibility = Visibility.Visible;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Close();  
        }

        private void cbOrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //לא קורה כלום זו סתם תצוגה או שנעשה שזה יעבור להצגת ההזמנה?
        }
    }
}
