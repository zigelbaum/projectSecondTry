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
            UpdateUnitWindow updateUnitWindow = new UpdateUnitWindow(unitKey);
            updateUnitWindow.ShowDialog();
        }

        public void DeleteUnitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HostingUnit unit = myBl.FindUnit(unitKey);
                myBl.DeleteHostingUnit(unit);
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
        
        public void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
