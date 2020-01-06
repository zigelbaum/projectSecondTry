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
        HostingUnit host; // לעשות שהוא יבין לבד לאיזו יחידה לבדוק התאמה
        public List<Order> listOrders;
        public UploadOrderWindow()
        {            
            listOrders = myBl.getOrders(u => u.HostingUnitKey == host.HostingUnitKey);
            InitializeComponent();
        }
    }
}
