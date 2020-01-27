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
        HostingUnit unit;
        Int32 my_request_key;
        List<GuestRequest> matchRequests;
        #endregion

        public CreateOrderWindow(Int32 hostID, HostingUnit my_unit)
        {
            InitializeComponent();

            unit = my_unit;

            getGuestRequestList();
        }

        private void getGuestRequestList()
        {
            IEnumerable<IGrouping<Host, HostingUnit>> my_units = myBl.GroupHostByHostingUnit();
            foreach (IGrouping<Host, HostingUnit> hostings in my_units)
            {
                foreach (HostingUnit hosting in hostings)
                {
                    if (hosting.HostingUnitKey == unit.HostingUnitKey)
                    {
                        matchRequests = myBl.RequestMatchToStipulation(myBl.BuildPredicate(hosting), hosting);
                    }
                }
            }
            requestView.ItemsSource = matchRequests;
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order ord = myBl.NewOrder(unit.HostingUnitKey, my_request_key);
                myBl.AddOrder(ord);
                MessageBox.Show("the order has been added successfully", "adding order", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
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

        private void MenuItem_Click_Info(object sender, RoutedEventArgs e)
        {
            if (requestView.SelectedItem != null)
            {
                GuestRequest request = ((GuestRequest)requestView.SelectedItem);
                my_request_key = request.GuestRequestKey;
                GuestPresentation presentation = new GuestPresentation(request, "View");
                presentation.ShowDialog();
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            OrderWindow orderWindow = new OrderWindow(unit.Owner.Id);
            orderWindow.ShowDialog();
        }
    }
}
