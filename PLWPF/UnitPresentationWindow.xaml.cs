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
    /// Interaction logic for UnitPresentationWindow.xaml
    /// </summary>
    public partial class UnitPresentationWindow : Window
    {
        #region variable  
        IBL myBl = BL.FactoryBL.getBL("XML");
        Int32 unitKey;
        public HostingUnit my_unit;
        #endregion

        public UnitPresentationWindow(Int32 unitKeyCTOR)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;          
            InitializeComponent();
            cbArea.ItemsSource = Enum.GetValues(typeof(Enums.Area));
            cbUnitType.ItemsSource = Enum.GetValues(typeof(Enums.HostingUnitType));
            cbArea.SelectedItem = Enums.Area.All;
            cbUnitType.SelectedItem = Enums.HostingUnitType.Zimmer;
            unitKey = unitKeyCTOR;
            switch(unitKey)
            {
                case 0:
                    cbArea.IsEnabled = false;
                    cbUnitType.IsEnabled = false;
                    tbSubArea.IsEnabled = false;
                    break;              
            }
        }

        public void addUnitButton_Click(object sender, RoutedEventArgs e)
        {
            if (tbUnitName.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter name without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (tbUnitName.Text == null)
            {
                MessageBox.Show("please enter name this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (tbSubArea.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter sub area without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (!tbAdults.Text.All(char.IsDigit))
            {
                MessageBox.Show("the adults input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (!tbKids.Text.All(char.IsDigit))
            {
                MessageBox.Show("the kids input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (!tbStars.Text.All(char.IsDigit))
            {
                MessageBox.Show("the stars input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (tbStars.Text == null)
            {
                MessageBox.Show("please enter number strars this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            my_unit = new HostingUnit()
            { HostingUnitName = tbUnitName.Text, HostingUnitType = (Enums.HostingUnitType)cbUnitType.SelectedIndex, Area = (Enums.Area)cbArea.SelectedIndex, SubArea = tbSubArea.Text, Adults = Int32.Parse(tbAdults.Text), Kids = Int32.Parse(tbKids.Text), Stars = Int32.Parse(tbStars.Text), Pool = ckbPool.AllowDrop, Jaccuzi = cbkJacuzzi.AllowDrop, Garden = ckbGarden.AllowDrop, ChildrenAttraction = ckbAttractions.AllowDrop, Meals = ckbMeals.AllowDrop };
            AddHostWindow addHostWindow = new AddHostWindow(my_unit);
            addHostWindow.ShowDialog();            

            switch (unitKey)
            {
                case 0:
                   try
                    {
                        myBl.addHostingUnit(my_unit);
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
                        my_unit.Owner = myBl.FindUnit(unitKey).Owner; 
                        myBl.SetHostingUnit(my_unit);
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
