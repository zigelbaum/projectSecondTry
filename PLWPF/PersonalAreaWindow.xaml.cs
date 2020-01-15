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
    /// Interaction logic for PersonalAreaWindow.xaml
    /// </summary>
    ///   
    public partial class PersonalAreaWindow : Window
    {
        #region variable
        IBL myBl = BL.FactoryBL.getBL("XML");
        public Int32 unitKey;
        #endregion
        public PersonalAreaWindow(Int32 hostingKey)
        {
            InitializeComponent();
            unitKey = hostingKey;
        }

        public void UploadUnitButton_Click(object sender, RoutedEventArgs e)
        {
            UnitPresentationWindow unitPresentationWindow = new UnitPresentationWindow(unitKey);
            unitPresentationWindow.ShowDialog();
        }

        public void DeleteUnitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("are you sure you want to delete?", "cancaling", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    HostingUnit unit = myBl.FindUnit(unitKey);
                    myBl.DeleteHostingUnit(unit);
                }
                catch (Exception ex)
                {
                    var results = MessageBox.Show(ex.Message + ". whould you like to retry?", "registration action failed", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    if (MessageBoxResult.No == results)
                    {
                        Close();
                        return;
                    }
                    else
                        return;
                }
            }
        }
        
        public void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
