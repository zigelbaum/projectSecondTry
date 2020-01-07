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
        IBL myBl = BL.FactoryBL.getBL("XML");
        HostingUnit host; // לעשות שהוא יבין לבד לאיזו יחידה לבדוק התאמה
        public Enums.OrderStatus status;
        public List<GuestRequest> matchRequests;
        public Order myorder;
        public CreateOrderWindow()
        {
            IEnumerable<IGrouping<Host, HostingUnit>> my_units = myBl.GroupHostByHostingUnit();
            foreach (IGrouping<Host, HostingUnit> hosting in my_units)
            {
                foreach (HostingUnit unit in hosting)
                {
                    matchRequests = myBl.RequestMatchToStipulation(myBl.BuildPredicate(unit));
                }
            }
            InitializeComponent();
        }
        
        private void CreateOrder()
        {
            myorder.HostingUnitKey = host.HostingUnitKey;
        }
    }
}
