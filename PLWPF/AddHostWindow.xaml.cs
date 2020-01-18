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
    /// Interaction logic for AddHostWindow.xaml
    /// </summary>
    public partial class AddHostWindow : Window
    {
        #region variable  
        IBL myBl = BL.FactoryBL.getBL("XML");        
        #endregion

        public AddHostWindow(HostingUnit unit)
        {
            InitializeComponent();
        }

        public void addButton_Click()
        {
            
        }

        private void addUnitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cancelUnitButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
