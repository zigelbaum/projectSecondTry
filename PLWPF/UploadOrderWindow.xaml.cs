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
    /// Interaction logic for UploadOrderWindow.xaml
    /// </summary>
    public partial class UploadOrderWindow : Window
    {
        #region variable
        IBL myBl = BL.FactoryBL.getBL("XML");
        List<Order> listOrders = new List<Order>();
        Order myorder;
        #endregion

        public UploadOrderWindow(Int32 hostID)
        {
            InitializeComponent();

            #region visibility
            StatusOrder.Visibility = Visibility.Hidden;
            StatusOrderString.Visibility = Visibility.Hidden;
            CreateDate.Visibility = Visibility.Hidden;
            CreateDateString.Visibility = Visibility.Hidden;
            OrderDate.Visibility = Visibility.Hidden;
            OrderDateString.Visibility = Visibility.Hidden;
            #endregion

            StatusOrder.ItemsSource = Enum.GetValues(typeof(Enums.OrderStatus));           
            cbbShowStatus.ItemsSource = Enum.GetValues(typeof(Enums.OrderStatus));
            //cbbShowStatus.SelectedIndex = 0;

            this.DataContext = myorder;//???????????????????????????????????????????????????????

            getOrderList(hostID);
        }

        private void getOrderList(Int32 hostID)
        {
            List<HostingUnit> unitsTemp = myBl.getHostingUnits(h => h.Owner.Id == hostID);
            foreach(HostingUnit unit in unitsTemp)
            {
                List<Order> tempOrder = myBl.getOrders(o => o.HostingUnitKey == unit.HostingUnitKey);
                foreach(Order ord in tempOrder)
                {
                    if(ord != null)
                    listOrders.Add(ord);
                }
            }
            orderView.ItemsSource = listOrders;     
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
                orderView.ItemsSource = order;
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
            orderView.ItemsSource = tempOrders;
        }

        private void MenuItem_Click_Info(object sender, RoutedEventArgs e)
        {
            if (orderView.SelectedItem != null)
            {
                myorder = ((Order)orderView.SelectedItem);

                StatusOrder.Visibility = Visibility;
                StatusOrderString.Visibility = Visibility;

                CreateDate.Visibility = Visibility;
                CreateDate.Text = myorder.CreateDate.ToString();
                CreateDateString.Visibility = Visibility;

                if (myorder.OrderDate != null)
                {
                    OrderDate.Visibility = Visibility;
                    OrderDate.Text = myorder.OrderDate.ToString();
                    OrderDateString.Visibility = Visibility;
                }

            }
        }

        private void setStatus_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32 index = StatusOrder.SelectedIndex + 1;
                Enums.OrderStatus myStatus = (Enums.OrderStatus)index;
                Order ord = myBl.getOrders(o => o.OrderKey == myorder.OrderKey)[0];
                ord.OrderStatus = myStatus;
                myBl.setOrder(ord);
                myorder = myBl.FindOrder(myorder.OrderKey);
                MessageBox.Show("The order has been closed successfully", "closing order", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            catch (Exception ex)
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

    }
}
