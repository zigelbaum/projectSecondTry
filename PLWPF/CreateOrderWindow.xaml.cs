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
    /// Interaction logic for CreateOrderWindow.xaml
    /// </summary>
    public partial class CreateOrderWindow : Window
    {
        #region variable
        IBL myBl = BL.FactoryBL.getBL("XML");
        HostingUnit host;
        Int32 my_request_key;
        List<GuestRequest> matchRequests;
        Order myorder;
        #endregion

        public CreateOrderWindow()
        {
            InitializeComponent();

            #region visibility
            GuestRequestList.Visibility = Visibility.Hidden;

            tbhostID.Visibility = Visibility.Visible;
            hostID.Visibility = Visibility.Visible;

            GuestRequestKey.Visibility = Visibility.Hidden;
            GuestRequestKeyString.Visibility = Visibility.Hidden;

            OrderKey.Visibility = Visibility.Hidden;
            OrderKeyString.Visibility = Visibility.Hidden;
            #endregion

        }
        private void Enter_Click()
        {
            string unitName = tbhostID.Text;
            List<HostingUnit> hostList = myBl.getHostingUnits(h => h.HostingUnitName == unitName);
            host = hostList[0];
            if (host == null)
            {
                //זה אומר שהקלט לא תקין צריך לטפל
            }
            //אם הפלט תקין
            //אמור להביא את רשימת הדרישות המתאימות ליחידת אירוח
            IEnumerable<IGrouping<Host, HostingUnit>> my_units = myBl.GroupHostByHostingUnit();
            foreach (IGrouping<Host, HostingUnit> hosting in my_units)
            {
                foreach (HostingUnit unit in hosting)
                {
                    matchRequests = myBl.RequestMatchToStipulation(myBl.BuildPredicate(unit));
                }
            }
            //מראה את הדרישות המתאימות
            GuestRequestList.Visibility = Visibility.Visible;
        }

        public void cbRequestList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //למלאות את הפרטים כאשר לוחצים על דרישת אירוח
            Int32 index = GuestRequestList.SelectedIndex;

            GuestRequestKey.Visibility = Visibility.Hidden;
            GuestRequestKeyString.Visibility = Visibility.Hidden;

            my_request_key = matchRequests[index].GuestRequestKey;

            #region definition order
            myorder.GuestRequestKey = my_request_key;
            myorder.HostingUnitKey = host.HostingUnitKey;
            myorder.HostingUnitKey = host.HostingUnitKey;
            #endregion

            myBl.AddOrder(myorder);
        }

    }
}
