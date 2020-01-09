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
        IBL myBl = BL.FactoryBL.getBL("XML");      
        HostingUnit host;
        List<Order> listOrders;
        Order myorder;

        public UploadOrderWindow()
        {
            string unitName = hostingUnitName.Text;
            List<HostingUnit> hostList = myBl.getHostingUnits(h => h.HostingUnitName == unitName);
            host = hostList[0];
            //מראה את ההזמנות המתאימות
            OrderstList.Visibility = Visibility;
       
            InitializeComponent();
        }

        public void cbOrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //למלאות את הפרטים כאשר לוחצים על הזמנה מסוימת
            StatusOrder.Visibility = Visibility;
            StatusOrderString.Visibility = Visibility;

            CreateDate.Visibility = Visibility;
            CreateDateString.Visibility = Visibility;

            Int32 index = OrderstList.SelectedIndex;
            myorder = listOrders[index];

            if(myorder.OrderDate != null)
            {
                OrderDate.Visibility = Visibility;
                OrderDateString.Visibility = Visibility;
            }
        }

        public void cbOrderStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //לעדכן סטטוס כשבוחרים סטטוס חדש
            myBl.setOrder();
        }
    }
}
