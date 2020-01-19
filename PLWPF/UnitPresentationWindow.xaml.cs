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
        public bool addedSuccessfully;
        string operation = "Add";
        public HostingUnit my_unit;
        #endregion

        public UnitPresentationWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            my_unit = new HostingUnit();        
            InitializeComponent();
            cbArea.ItemsSource = Enum.GetValues(typeof(Enums.Area));
            cbUnitType.ItemsSource = Enum.GetValues(typeof(Enums.HostingUnitType));
            cbArea.SelectedItem = null;
            cbUnitType.SelectedItem = null;
            lblUnitKey.Visibility = Visibility.Hidden;
            tbUnitKey.Visibility = Visibility.Hidden;
            tbOnwer.Visibility = Visibility.Hidden;
            lblOwner.Visibility = Visibility.Hidden;
        }

        public UnitPresentationWindow(HostingUnit hostingUnit, string oper)
        {
            operation = oper;
            my_unit = hostingUnit;
            this.DataContext = my_unit;

        InitializeComponent();
            cbArea.ItemsSource = Enum.GetValues(typeof(Enums.Area));
            cbUnitType.ItemsSource = Enum.GetValues(typeof(Enums.HostingUnitType));
            switch (oper)
            {
                case "Update":
                    cbArea.IsEnabled = false;
                    cbUnitType.IsEnabled = false;
                    tbOnwer.IsReadOnly = true;
                    tbUnitKey.IsReadOnly = true;
                    tbSubArea.IsReadOnly = true;
                    addUnitButton.Content = "save changes";
                    break;
                case "View":
                    cbArea.IsEnabled = false;
                    cbUnitType.IsEnabled = false;
                    tbOnwer.IsReadOnly = true;
                    tbUnitKey.IsReadOnly = true;
                    tbSubArea.IsReadOnly = true;
                    tbUnitName.IsReadOnly = true;
                    tbStars.IsReadOnly = true;
                    tbAdults.IsReadOnly = true;
                    tbKids.IsReadOnly = true;
                    ckbPool.IsEnabled = false;
                    cbkJacuzzi.IsEnabled = false;
                    ckbGarden.IsEnabled = false;
                    ckbAttractions.IsEnabled = false;
                    ckbMeals.IsEnabled = false;
                    addUnitButton.Visibility = Visibility.Hidden;
                    cancelUnitButton.Content = "Close";
                    break;
            }
        }
        public void addUnitButton_Click(object sender, RoutedEventArgs e)
        {
            bool premission = true;

            #region required filleds
            if (tbUnitName.IsLoaded == false)
            {
                MessageBox.Show("please enter youe hosting unit name.", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                tbUnitName.BorderBrush = Brushes.Red;
                return;
            }
            if (tbSubArea.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Please enter sub area without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbSubArea.BorderBrush = Brushes.Red;
                premission = false;
                return;
            }
            if (tbAdults.Text == null)
            {
                MessageBox.Show("Please fill the adults filed", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbAdults.BorderBrush = Brushes.Red;
                premission = false;
                return;
            }
            else
            {
                if (!tbAdults.Text.All(char.IsDigit))
                {
                    MessageBox.Show("The adults input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    tbAdults.BorderBrush = Brushes.Red;
                    premission = false;
                    return;
                }
            }            
            if (!tbKids.Text.All(char.IsDigit))
            {
                MessageBox.Show("The kids input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbKids.BorderBrush = Brushes.Red;
                premission = false;
                return;
            }
            if (tbStars.Text == null)
            {
                MessageBox.Show("please enter number strars this is a required field", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbStars.BorderBrush = Brushes.Red;
                premission = false;
                return;
            }
            else
              if (!tbStars.Text.All(char.IsDigit))
            {
                MessageBox.Show("the stars input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbStars.BorderBrush = Brushes.Red;
                premission = false;
                return;
            }
            if (cbArea.SelectedItem == null)
            {

                MessageBox.Show("The area filed must be filled", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);               
                premission = false;
                return;
            }
            if (cbUnitType.SelectedItem == null)
            {

                MessageBox.Show("The type filed must be filled", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                premission = false;
                return;
            }
            #endregion
            switch (operation)
            {
                case "Add":
                    if (premission == true)
                    {
                        var result = MessageBox.Show("Are you allready regisred?", "Adding unit", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        if (result == MessageBoxResult.Yes)
                        {
                            GetKey key = new GetKey("Host");
                            key.ShowDialog();
                            if (key.retriveSuccess == true)
                            {
                                List<HostingUnit> units = myBl.getHostingUnits(x => x.Owner.Id.ToString() == key.tbID.Text);
                                my_unit.Owner = units.First().Owner;
                            }
                        }
                        else
                        {

                            AddHostWindow addHostWindow = new AddHostWindow(my_unit/*, unitKey*/);
                            addHostWindow.ShowDialog();
                            my_unit.Owner = addHostWindow.host;
                        }
                        HostingUnit unit = my_unit;
                        try
                        {
                            myBl.addHostingUnit(unit);
                        }
                        catch (Exception a)
                        {
                            var result2 = MessageBox.Show(a.Message + ". whould you like to retry?", "registration action failed", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                            if (MessageBoxResult.No == result2)
                            {
                                Close();
                                return;
                            }
                            else
                                return;
                        }
                        addedSuccessfully = true;
                        Close();
                    }
                    break;
                case "Update":
                    try
                    {
                        MainWindow.myBL.SetHostingUnit(my_unit);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "internal error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        return;
                    }
                    Close();

                    MessageBox.Show("the request details have been changed successfully", "update", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);

                    break;
            }
            
        }

        private void cancelUnitButton_Click(object sender, RoutedEventArgs e)
        {
            switch (operation)
            {
                case "Add":
                    var result = MessageBox.Show("are you sure you want to exit?\n any changes will not be saved.", "cancaling", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    if (result == MessageBoxResult.OK)
                        Close();
                    break;
                case "Update":
                    var result2 = MessageBox.Show("are you sure you want to exit?\n any changes will not be saved.", "cancaling", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    if (result2 == MessageBoxResult.OK)
                        Close();
                    break;
                case "View":
                    var result3 = MessageBox.Show("are you sure you want to exit?", "exit", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    if (result3 == MessageBoxResult.OK)
                        Close();
                    break;
            }
        }

    }
}
