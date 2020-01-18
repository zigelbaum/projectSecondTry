using BE;
using BL;
using System;
using System.Collections.ObjectModel;
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
        public static IBL myBl = BL.FactoryBL.getBL("XML");
        public List<Order> listOrders = new List<Order>();
        public Int32 hostID;
        private ObservableCollection<Order> ordersList = new ObservableCollection<Order>(myBl.GetOrdersList());
        #endregion

        public OrderWindow(Int32 IDhost)
        {
            InitializeComponent();
            UploadOrderButton.Visibility = Visibility.Hidden;
            CreateOrderButton.Visibility = Visibility.Hidden;            
            hostID = IDhost;
            //getOrderList();
            List<HostingUnit> tempHostingList = myBl.getHostingUnits(h => h.Owner.Id == IDhost);
            //List<HostingUnit> tempHostingList = myBl.getHostingUnits(u=>u.Owner.Id == IDhost);
            foreach (HostingUnit my_unit in tempHostingList)
            {
                List<Order> tempOrderList = myBl.getOrders(o => o.HostingUnitKey == my_unit.HostingUnitKey);
                foreach (Order ord in tempOrderList)
                {
                    listOrders.Add(ord);
                }
            }         
            OrderView.ItemsSource = listOrders;          
            UploadOrderButton.Visibility = Visibility.Visible;
            CreateOrderButton.Visibility = Visibility.Visible;
        }

        private void UploadOrderButton_Click(object sender, RoutedEventArgs e)
        {
            UploadOrderWindow upload_ord_Window = new UploadOrderWindow(hostID);
            upload_ord_Window.ShowDialog();
        }

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            CreateOrderWindow new_ord_Window;
            GetKey getKey = new GetKey("HostingUnit");
            getKey.ShowDialog();
            if (getKey.numVal != 0)
            {
                new_ord_Window = new CreateOrderWindow(hostID, myBl.FindUnit(getKey.numVal));
                new_ord_Window.ShowDialog();
            }                
        }

        /*private void getOrderList()
        {
            List<HostingUnit> hostList = myBl.getHostingUnits(h => h.Owner.ID == hostID);
            foreach(HostingUnit my_unit in hostList)
            {
                List<Order> tempOrderList = myBl.getOrders(o => o.HostingUnitKey == my_unit.HostingUnitKey);
                foreach(Order ord in tempOrderList)
                {
                    listOrders.Add(ord);
                }
            }*/
            /*unit = hostList[0];
            listOrders = myBl.getOrders(o => o.HostingUnitKey == unit.HostingUnitKey);*/
            /*cbOrderstList.Visibility = Visibility.Visible;
            UploadOrderButton.Visibility = Visibility.Visible;
            CreateOrderButton.Visibility = Visibility.Visible;
        }*/

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
