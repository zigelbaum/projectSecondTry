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
        HostingUnit hosting;
        Int32 hostingKey;
        #endregion

        public AddHostWindow(HostingUnit unit, Int32 unitKey)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            hostingKey = unitKey;
            hosting = unit;
            switch (hostingKey)
            {
                case 0:
                    break;
                default:                    
                    tbID.IsEnabled = false;
                    break;
            }
        }

        public void addUnitButton_Click(object sender, RoutedEventArgs e)
        {
            if (tbFirstName.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter name without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (tbFirstName.Text == null)
            {
                MessageBox.Show("please enter name this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (tbLastName.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter last name without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (tbLastName.Text == null)
            {
                MessageBox.Show("please enter last name this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (!tbPhon.Text.All(char.IsDigit))
            {
                MessageBox.Show("the phon input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (tbBname.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter banck name without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (tbBname.Text == null)
            {
                MessageBox.Show("please enter banck name this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (tbBrCity.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter branch banck city without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (!tbBnumber.Text.All(char.IsDigit))
            {
                MessageBox.Show("the banck number input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if(tbBnumber.Text == null)
            {
                MessageBox.Show("please enter banck number this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (!tbBrNumber.Text.All(char.IsDigit))
            {
                MessageBox.Show("the branch banck number input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (tbBrNumber.Text == null)
            {
                MessageBox.Show("please enter branch banck number this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (!tbAccountNumber.Text.All(char.IsDigit))
            {
                MessageBox.Show("the banck account number input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (tbAccountNumber.Text == null)
            {
                MessageBox.Show("please enter banck account number this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (!tbID.Text.All(char.IsDigit))
            {
                MessageBox.Show("the ID number input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (tbAccountNumber.Text == null)
            {
                MessageBox.Show("please enter ID, this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            BankBranch branch = new BankBranch()
            { BankName = tbBname.Text, BankNumber = Int32.Parse(tbBnumber.Text), BranchAddress = tbBrAdress.Text, BranchCity = tbBrCity.Text, BranchNumber = Int32.Parse(tbBrNumber.Text) };
            Host host = new Host()
            { PrivateName=tbFirstName.Text, FamilyName=tbLastName.Text, Id=Int32.Parse(tbID.Text), PhoneNumber=tbPhon.Text, BankAccountNumber=Int32.Parse(tbAccountNumber.Text), CollectionClearance=ckbcollectionClearance.AllowDrop, MailAddress=tbMail.Text, BankBranchDetails=branch };
            hosting.Owner = host;
            switch (hostingKey)
            {
                case 0:
                    try
                    {
                        myBl.addHostingUnit(hosting);
                    }
                    catch (Exception a)
                    {
                        var result = MessageBox.Show(a.Message + ". whould you like to retry?", "registration action failed", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        if (MessageBoxResult.No == result)
                        {
                            Close();
                            return;
                        }
                        else
                            return;
                    }
                    Close();
                    MessageBox.Show("the hosting unit has been added successfully", "adding unit", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    break;
                default:
                    try
                    {
                        myBl.SetHostingUnit(hosting);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "internal error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        return;
                    }
                    Close();
                    MessageBox.Show("the hosting unit details have bee changed successfully", "update", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    break;
            }
        }

        public void cancelUnitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("are you sure you want to exit?\n any changes will not be saved.", "cancaling", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            if (result == MessageBoxResult.OK)
                Close();
        }
    }
}
