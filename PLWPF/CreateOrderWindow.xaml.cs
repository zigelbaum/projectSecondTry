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
            //אמור להביא את רשימת הדרישות המתאימות ליחידת אירוח
            IEnumerable<IGrouping<Host, HostingUnit>> my_units = myBl.GroupHostByHostingUnit();
            foreach (IGrouping<Host, HostingUnit> hostings in my_units)
            {
                foreach (HostingUnit hosting in hostings)
                {
                    if (hosting == unit)
                    {
                        matchRequests = myBl.RequestMatchToStipulation(myBl.BuildPredicate(hosting));
                    }
                }
            }
        }

        public void cbRequestList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //למלאות את הפרטים כאשר לוחצים על דרישת אירוח
            Int32 index = GuestRequestList.SelectedIndex;

            my_request_key = matchRequests[index].GuestRequestKey;
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order ord = myBl.NewOrder(unit.HostingUnitKey, my_request_key);
                myBl.AddOrder(ord);
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
