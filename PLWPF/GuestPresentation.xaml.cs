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
using Microsoft.Win32;
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for GuestPresentation.xaml
    /// </summary>

    public partial class GuestPresentation : Window
    {
        public GuestRequest guestRequest;

        public string operation = "Add";
        OpenFileDialog op;
        bool isImageChanged = false;

        ///<summery>
        ///default ctor for adding new request
        ///</summery>
        public GuestPresentation()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            guestRequest = new BE.GuestRequest();
            this.DataContext = guestRequest;
            InitializeComponent();
            requestDetails.Visibility = Visibility.Hidden;
            requestID.Visibility = Visibility.Hidden;
            tbRquestId.Visibility = Visibility.Hidden;
            requestStatus.Visibility = Visibility.Hidden;
            tbRequStatus.Visibility = Visibility.Hidden;
            regDate.Visibility = Visibility.Hidden;
            tbRegDate.Visibility = Visibility.Hidden;
            cbbArea.ItemsSource = Enum.GetValues(typeof(Enums.Area));
            cbbVacationType.ItemsSource = Enum.GetValues(typeof(Enums.HostingUnitType));
            cbbArea.SelectedItem = Enums.Area.All;
            cbbVacationType.SelectedItem = Enums.HostingUnitType.Zimmer;
        }

        ///<summery>
        ///param ctor, gets the request and the operaion
        ///<summery/>
        ///
        public GuestPresentation(GuestRequest request, string oper)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            operation = oper;
            guestRequest = request;
            this.DataContext = guestRequest;
            InitializeComponent();
            cbbArea.ItemsSource = Enum.GetValues(typeof(Enums.Area));
            cbbVacationType.ItemsSource = Enum.GetValues(typeof(Enums.HostingUnitType));
            cbbArea.SelectedItem = Enums.Area.All;
            cbbVacationType.SelectedItem = Enums.HostingUnitType.Zimmer;
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(System.IO.Path.GetFullPath(MainWindow.myBL.GetCostumerImagePath(guestRequest.GuestRequestKey)));
                bitmap.EndInit();
                CostumerImage.Source = bitmap;
            }
            catch { } //costumer doesnt have image

            switch (oper)
            {
                case "Update":
                    tbRquestId.IsEnabled = false;
                    tbRegDate.IsEnabled = false;
                    tbFirstName.IsEnabled = false;
                    tbLastName.IsEnabled = false;
                    tbRequStatus.IsEnabled = false;
                    costumerPro.Content = "change profile";
                    addReqButton.Content = "save changes";
                    break;
                case "View":
                    tbRquestId.IsEnabled = false;
                    tbRegDate.IsEnabled = false;
                    tbFirstName.IsEnabled = false;
                    tbLastName.IsEnabled = false;
                    tbRequStatus.IsEnabled = false;
                    tbMail.IsEnabled = false;
                    cbbArea.IsEnabled = false;
                    tbSubArea.IsEnabled = false;
                    cbbVacationType.IsEnabled = false;
                    tbAdults.IsEnabled = false;
                    tbKids.IsEnabled = false;
                    tbStars.IsEnabled = false;
                    ckbPool.IsEnabled = false;
                    cbkJacuzzi.IsEnabled = false;
                    ckbGarden.IsEnabled = false;
                    ckbAttractions.IsEnabled = false;
                    ckbMeals.IsEnabled = false;
                    addReqButton.Visibility = Visibility.Hidden;
                    cancelreqButton.Content = "close";
                    costumerPro.Visibility = Visibility.Hidden;
                    break;
            }
        }


        private void costumerPro_Click(object sender, RoutedEventArgs e)
        {
            op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                isImageChanged = true;
                CostumerImage.Source = new BitmapImage(new Uri(op.FileName));
            }
        }


        private void cancelreqButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addReqButton_Click(object sender, RoutedEventArgs e)
        {
            if (tbFirstName.Text.Any(char.IsDigit) || tbLastName.Text.Any(char.IsDigit))
            {
                MessageBox.Show("please enter name without numbers", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                return;
            }
            if (cbbArea.SelectedItem == null)
            {
                MessageBox.Show("please choose your vacation area", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            if (cbbVacationType.SelectedItem == null)
            {
                MessageBox.Show("please choose your vacation area", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            if (!tbAdults.Text.All(char.IsDigit))
            {
                MessageBox.Show("the adults input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            if (!tbKids.Text.All(char.IsDigit))
            {
                MessageBox.Show("the kids input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            if (!tbStars.Text.All(char.IsDigit))
            {
                MessageBox.Show("the stars input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            switch (operation)
            {
                case "Add":
                    GuestRequest guest = guestRequest;
                    try { MainWindow.myBL.addGuestRequest(guest); }
                    catch (Exception a)
                    {
                        MessageBox.Show(a.Message+". whould you like to retry?", "registration action failed", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        if (MessageBoxResult.No == result)
                        {
                            Close();
                            return;
                        }
                        else
                            return;
                    }
                    Close();
                    if (isImageChanged)
                    {
                        try
                        {
                            MainWindow.myBL.AddCostumerImage(testerToAdd.Id, op.FileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("המערכת לא הצליחה להוסיף את תמונת הבוחן. " + ex.Message, "סטטוס עידכון", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        }
                    }
                    MessageBox.Show("הבוחן התווסף בהצלחה למערכת!", "סטטוס הוספה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    break;
                    break;
            }
        }
    }


}
