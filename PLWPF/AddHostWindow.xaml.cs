using BE;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public Host host;       
        public bool added=false;
        public IEnumerable<IGrouping<int, BankBranch>> branches;
        #endregion

        public AddHostWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            host = new Host();
            this.DataContext = host;
            InitializeComponent();
            branches = myBl.GetBankBranchesGroup();
            initBank(true);
        }

        private void addHost_Click(object sender, RoutedEventArgs e)
        {
            bool premission = true;

            #region required fildes
            if (tbFirstName.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter name without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbFirstName.Background = Brushes.IndianRed;
                premission = false;
                return;
            }
            if (tbFirstName.Text == null)
            {
                MessageBox.Show("please enter name this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbFirstName.Background = Brushes.IndianRed;
                premission = false;
                return;
            }
            if (tbLastName.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter last name without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbLastName.Background = Brushes.IndianRed;
                premission = false;
                return;
            }
            if (tbLastName.Text == null)
            {
                MessageBox.Show("please enter last name this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbLastName.Background = Brushes.IndianRed;
                premission = false;
                return;
            }
            if (!tbPhon.Text.All(char.IsDigit))
            {
                MessageBox.Show("the phon input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbPhon.Background = Brushes.IndianRed;
                premission = false;
                return;
            }
            
            if (tbBrCity.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter branch banck city without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbBrCity.Background = Brushes.IndianRed;
                premission = false;
                return;
            }
            if (!tbAccountNumber.Text.All(char.IsDigit))
            {
                MessageBox.Show("the banck account number input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbAccountNumber.Background = Brushes.IndianRed;
                premission = false;
                return;
            }
            if (tbAccountNumber.Text == null)
            {
                MessageBox.Show("please enter banck account number this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbAccountNumber.Background = Brushes.IndianRed;
                premission = false;
                return;
            }
            if (!tbID.Text.All(char.IsDigit))
            {
                MessageBox.Show("the ID number input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbID.Background = Brushes.IndianRed;
                premission = false;
                return;
            }
            if (tbID.Text == null)
            {
                MessageBox.Show("please enter ID, this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbID.Background = Brushes.IndianRed;
                premission = false;
                return;
            }
            if (ckbcollectionClearance.IsChecked==false)
            {
                MessageBox.Show("You can not register if you don't have collection clearance", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                ckbcollectionClearance.Background = Brushes.IndianRed;
                premission = false;
                return;
            }
            if(tbMail.Text==null)
            {
                MessageBox.Show("please enter mailaddress, this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbID.Background = Brushes.IndianRed;
                premission = false;
                return;
            }
            try
            {
                myBl.IsValidEmail(tbMail.Text);
            }
            catch(Exception a)
            {

                MessageBox.Show(a.Message, "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbID.Background = Brushes.IndianRed;
                premission = false;
                return;
            }
         
            #endregion
            if (premission == true)
            {
                added = true;
                Close();
            }
               
        }

        private void cancelUnitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("are you sure you want to exit?\n any changes will not be saved.", "cancaling", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.OK)
            {
                added = false;
                Close();
            }
        }

        private void initBank(bool start = false)//binding between bank and bank source
        {
            int curBank = -1;

            if (start)
            {
                foreach (var bank in branches)
                {
                    cbBname.Items.Add(bank.First().BankName);
                    cbBnumber.Items.Add(bank.First().BankNumber.ToString());
                }

                cbBname.SelectedIndex = -1;
                cbBnumber.SelectedIndex = -1;
            }
            else
            {
                curBank = cbBnumber.SelectedIndex;
            }
            if (!start && curBank != -1)
            {
                foreach (var bank in branches.ElementAt(curBank))
                {
                    cbBrAdress.Items.Add(bank.BranchCity + " : " + bank.BranchAddress);//adds key of each group to list
                    cbBrNumber.Items.Add(bank.BranchNumber.ToString());

                }
                cbBrNumber.IsEnabled = true;
                cbBrAdress.IsEnabled = true;
            }
            cbBrAdress.SelectedIndex = -1;
            cbBrNumber.SelectedIndex = -1;//currently no index selected

        }
        #region bank number checks
        private void BankAcountNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (Regex.IsMatch(tbAccountNumber.Text, ("^[0-9]+$")))
            {
                tbAccountNumber.BorderBrush = Brushes.Gray;
                host.BankAccountNumber = Convert.ToInt32(tbAccountNumber.Text);

            }
            else
            {
                tbAccountNumber.BorderBrush = Brushes.Red;
                tbAccountNumber.Text = "";
            }
        }
        private void BankAcountNumberTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Regex.IsMatch(tbAccountNumber.Text, ("^[0-9]+$")))
                {
                    tbAccountNumber.BorderBrush = Brushes.Gray;
                    host.BankAccountNumber = Convert.ToInt32(tbAccountNumber.Text);
                }
                else
                {
                    tbAccountNumber.BorderBrush = Brushes.Red;
                    tbAccountNumber.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
        #region comboboxes
        private void cbBanckName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbBnumber.SelectedIndex = cbBnumber.SelectedIndex;
            initBank();

        }
        private void CbBanckNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbBname.SelectedIndex = cbBnumber.SelectedIndex;
        }

        #region branch
        private void Cb_branchAddr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbBrNumber.SelectedIndex = cbBrAdress.SelectedIndex;
        }

        private void Cb_branchNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbBrAdress.SelectedIndex = cbBnumber.SelectedIndex;
            setBank();

        }
        private void setBank()
        {
            var b = branches.ElementAt(cbBnumber.SelectedIndex).ElementAt(cbBrNumber.SelectedIndex);

            host.BankBranchDetails.BankName = b.BankName;
            host.BankBranchDetails.BankNumber = b.BankNumber;
            host.BankBranchDetails.BranchAddress = b.BranchAddress;
            host.BankBranchDetails.BranchCity = b.BranchCity;
            host.BankBranchDetails.BranchNumber = b.BranchNumber;

        }

        #endregion

        private void Cb_collectionClearance_Checked(object sender, RoutedEventArgs e)
        {
            if (ckbcollectionClearance.IsChecked == true)
                host.CollectionClearance = true;
            else
                host.CollectionClearance = false;

        }
        #endregion
    }
}
