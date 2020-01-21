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
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for orderQueryWindow.xaml
    /// </summary>
    public partial class orderQueryWindow : Window
    {
        public static IBL myBl = BL.FactoryBL.getBL("XML");
        public List<Order> listOrders = myBl.GetOrdersList();
        public orderQueryWindow()
        {
            InitializeComponent();
            OrderView.ItemsSource = listOrders;
        }
        private void tbUnitKey_SearchFilterChanged(object sender, TextChangedEventArgs e)
        {
            if (tbUnitKey != null)
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
            foreach (Order order in listOrders)
            {
                if (order.OrderStatus == (Enums.OrderStatus)(index + 1))
                    tempOrders.Add(order);
            }
            OrderView.ItemsSource = tempOrders;
        }
    }
}
