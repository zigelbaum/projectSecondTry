using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
    /// Interaction logic for DirectorWindow.xaml
    /// </summary>
    public partial class DirectorWindow : Window
    {
        IBL myBl = BL.FactoryBL.getBL("XML");
        double all_fee ;

        public DirectorWindow()
        {
            InitializeComponent();
            Thread thread = new Thread(DailyUpdate);
            thread.Start();
            all_fee = myBl.Aggregate_fee();
            tbFee.Text = all_fee.ToString();
            tbFee.IsEnabled = false;
        }

        private void GuestQuery_Click(object sender, RoutedEventArgs e)
        {
            GuestRequestWindow guestRequest = new GuestRequestWindow("Director");
            guestRequest.ShowDialog();
        }

        private void HostingQuery_Click(object sender, RoutedEventArgs e)
        {
            HostingUnitWindow unitWindow = new HostingUnitWindow("Director");
            unitWindow.ShowDialog();
        }

        private void OrderQuery_Click(object sender, RoutedEventArgs e)
        {
            orderQueryWindow orderQwindow = new orderQueryWindow();
            orderQwindow.ShowDialog();
        }

        #region dailyUpdate
        private void DailyUpdate()
        {
            while (true)
            {
                DateTime _DateLastRun;
                _DateLastRun = DateTime.Now.Date;

                if (_DateLastRun < DateTime.Now.Date)
                {
                    OrderDailyMethod();
                    ReqDailyMethod();
                    _DateLastRun = DateTime.Now.Date;
                }
            }
        }

        private void ReqDailyMethod()
        {
            List<GuestRequest> listOfreq = myBl.DaysPassFromMail(31);
            List<GuestRequest> guestList = new List<GuestRequest>();
            foreach (GuestRequest g in listOfreq)
            {
                guestList.Add(g);
            }
            guestList.ForEach(element => element.Status = Enums.GuestRequestStatus.RequestExpired);
            guestList.ForEach(element => myBl.SetGuestRequest(element));
        }

        private void OrderDailyMethod()
        {
            List<Order> listOfOrder = myBl.DaysPassedOrders(31);
            List<Order> ord = new List<Order>();
            foreach (Order o in listOfOrder)
            {
                ord.Add(o);
            }
            ord.ForEach(element => element.OrderStatus = Enums.OrderStatus.Closed);
            ord.ForEach(element => myBl.setOrder(element));
        }
        #endregion
    }
}
