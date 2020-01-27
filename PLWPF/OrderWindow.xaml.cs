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
        #endregion

        public OrderWindow(Int32 IDhost)
        {
            InitializeComponent();            
            hostID = IDhost;

            List<HostingUnit> tempHostingList = myBl.getHostingUnits(h => h.Owner.Id == IDhost);            
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
            cbbShowStatus.Visibility = Visibility.Visible;

            cbbShowStatus.ItemsSource = Enum.GetValues(typeof(Enums.OrderStatus));
        }

        private void UploadOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
                Close();
                new_ord_Window = new CreateOrderWindow(hostID, myBl.FindUnit(getKey.numVal));
                new_ord_Window.ShowDialog();
            }
        }

        private void tbUnitKey_SearchFilterChanged(object sender, TextChangedEventArgs e)
        {
            if(tbUnitKey != null)
            {
                string searchTxt = tbUnitKey.Text;
                string upper = searchTxt.ToUpper();
                string lower = searchTxt.ToLower();

                var order = from item in listOrders
                            let key = item.HostingUnitKey.ToString()
                            where
                              key.StartsWith(lower)
                              || key.StartsWith(upper)
                              || key.Contains(searchTxt)
                            select item;
                OrderView.ItemsSource = order;
            }
        }

        private void cbbShowStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Int32 index = cbbShowStatus.SelectedIndex;
            List<Order> tempOrders = new List<Order>();
            foreach(Order order in listOrders)
            {
                if (order.OrderStatus == (Enums.OrderStatus)(index+1))
                    tempOrders.Add(order);
            }
            OrderView.ItemsSource = tempOrders;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Close();  
        }

    }
}
