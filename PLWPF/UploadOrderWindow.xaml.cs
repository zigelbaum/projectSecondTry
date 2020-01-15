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
            OrderstList.Visibility = Visibility.Hidden;
            StatusOrder.Visibility = Visibility.Hidden;
            StatusOrderString.Visibility = Visibility.Hidden;
            CreateDate.Visibility = Visibility.Hidden;
            CreateDateString.Visibility = Visibility.Hidden;
            OrderDate.Visibility = Visibility.Hidden;
            OrderDateString.Visibility = Visibility.Hidden;
            #endregion

            getOrderList(hostID);
        }

        private void getOrderList(Int32 hostID)
        {
            //לעשות רשימה שתחזיר את כל ההזמנות של מארח מסוים
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
            OrderstList.Visibility = Visibility.Visible;

           /* IEnumerable<IGrouping<Host, HostingUnit>> my_units = myBl.GroupHostByHostingUnit();
            foreach (IGrouping<Host, HostingUnit> hosting in my_units)
            {
                foreach (HostingUnit unit in hosting)//הקוד לא יעיל להבין איך עובד הגרופינג ואז לתקן
                {
                    if(unit.Owner.HostKey == hostKey)
                    {
                        List<Order> temp = myBl.getOrders(o => o.HostingUnitKey == unit.HostingUnitKey);
                        int num = temp.Count();
                    }
                }
            }

                List<HostingUnit> hostList = myBl.getHostingUnits(h => h.Owner.HostKey == hostKey);
            host = hostList[0];
            //מראה את ההזמנות המתאימות
            OrderstList.Visibility = Visibility.Visible;*/
        }

        private void cbOrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Int32 index = OrderstList.SelectedIndex;
            myorder = listOrders[index];

            //למלאות את הפרטים כאשר לוחצים על הזמנה מסוימת
            StatusOrder.Visibility = Visibility;
            StatusOrderString.Visibility = Visibility;

            CreateDate.Visibility = Visibility;
            CreateDateString.Visibility = Visibility;           

            if (myorder.OrderDate != null)
            {
                OrderDate.Visibility = Visibility;
                OrderDateString.Visibility = Visibility;
            }
        }

        private void cbOrderStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //לעדכן סטטוס כשבוחרים סטטוס חדש
                Int32 index = StatusOrder.SelectedIndex;
                Enums.OrderStatus myStatus = (Enums.OrderStatus)index;
                Int32 orderKey = myorder.OrderKey;
                Order ord = myBl.getOrders(o => o.OrderKey == orderKey)[0];
                ord.OrderStatus = myStatus;
                myBl.setOrder(ord);
                //יראה את ההזמנה המעודדכנת
                myorder = myBl.FindOrder(myorder.OrderKey);
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
