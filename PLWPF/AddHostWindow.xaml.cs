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
        IBL myBl = BL.FactoryBL.getBL("List");
        //HostingUnit hosting;
        public Host host;
        bool premission = true;
        #endregion

        public AddHostWindow(/*HostingUnit unit*/)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            host = new Host();
            this.DataContext = host;
            //hosting = unit;
            InitializeComponent();            
        }

        private void addHost_Click(object sender, RoutedEventArgs e)
        {

            #region required fildes
            if (tbFirstName.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter name without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (tbFirstName.Text == null)
            {
                MessageBox.Show("please enter name this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (tbLastName.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter last name without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (tbLastName.Text == null)
            {
                MessageBox.Show("please enter last name this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (!tbPhon.Text.All(char.IsDigit))
            {
                MessageBox.Show("the phon input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (tbBname.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter banck name without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (tbBname.Text == null)
            {
                MessageBox.Show("please enter banck name this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (tbBrCity.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter branch banck city without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (!tbBnumber.Text.All(char.IsDigit))
            {
                MessageBox.Show("the banck number input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (tbBnumber.Text == null)
            {
                MessageBox.Show("please enter banck number this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (!tbBrNumber.Text.All(char.IsDigit))
            {
                MessageBox.Show("the branch banck number input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (tbBrNumber.Text == null)
            {
                MessageBox.Show("please enter branch banck number this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (!tbAccountNumber.Text.All(char.IsDigit))
            {
                MessageBox.Show("the banck account number input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (tbAccountNumber.Text == null)
            {
                MessageBox.Show("please enter banck account number this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (!tbID.Text.All(char.IsDigit))
            {
                MessageBox.Show("the ID number input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (tbAccountNumber.Text == null)
            {
                MessageBox.Show("please enter ID, this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            if (ckbcollectionClearance.IsChecked==false)
            {
                MessageBox.Show("You can not register if you don't have collection clearance", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            } 
            #endregion
            if (premission == true)
            {                             
                Close();
            }

        
        }

        private void cancelUnitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("are you sure you want to exit?\n any changes will not be saved.", "cancaling", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.OK)
                Close();
        }
    }
}
