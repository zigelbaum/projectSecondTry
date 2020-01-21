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
        #region variable
        public GuestRequest guestRequest;
        public string operation = "Add";
        OpenFileDialog op;
        bool isImageChanged = false;
        public bool addedSuccessfuly = false;
        Brush yelf, yels, grayf, grays;
        #endregion

        ///<summery>
        ///default ctor for adding new request
        ///</summery>
        public GuestPresentation()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            guestRequest = new GuestRequest();
            this.DataContext = guestRequest;
            InitializeComponent();
            requestDetails.Visibility = Visibility.Hidden;
            requestID.Visibility = Visibility.Hidden;
            tbRquestId.Visibility = Visibility.Hidden;
            requestStatus.Visibility = Visibility.Hidden;
            cbbStatus.Visibility = Visibility.Hidden;
            regDate.Visibility = Visibility.Hidden;
            tbRegDate.Visibility = Visibility.Hidden;
            cbbArea.ItemsSource = Enum.GetValues(typeof(Enums.Area));
            cbbVacationType.ItemsSource = Enum.GetValues(typeof(Enums.HostingUnitType));
            cbbArea.SelectedItem = null;
            cbbVacationType.SelectedItem = null;
            dpEntryDate.SelectedDate = DateTime.Now;
            dpRealeseDate.SelectedDate = DateTime.Now.AddDays(1);
            tbStars.IsEnabled = false;

            #region stars
            yelf = pstar1.Fill;
            yels = pstar1.Stroke;

            grayf = pstar5.Fill;
            pstar1.Fill = grayf;
            pstar2.Fill = grayf;
            pstar3.Fill = grayf;
            pstar4.Fill = grayf;

            grays = pstar5.Stroke;
            pstar1.Stroke = grays;
            pstar2.Stroke = grays;
            pstar3.Stroke = grays;
            pstar4.Stroke = grays;
            #endregion
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

            #region star
            yelf = pstar1.Fill;
            yels = pstar1.Stroke;

            grayf = pstar5.Fill;
            pstar1.Fill = grayf;
            pstar2.Fill = grayf;
            pstar3.Fill = grayf;
            pstar4.Fill = grayf;

            grays = pstar5.Stroke;
            pstar1.Stroke = grays;
            pstar2.Stroke = grays;
            pstar3.Stroke = grays;
            pstar4.Stroke = grays;
            #endregion
            tbStars.IsEnabled = false;

            cbbStatus.ItemsSource = Enum.GetValues(typeof(Enums.GuestRequestStatus));
            cbbArea.ItemsSource = Enum.GetValues(typeof(Enums.Area));
            cbbVacationType.ItemsSource = Enum.GetValues(typeof(Enums.HostingUnitType));
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
                    cbbStatus.IsEnabled = false;                   
                    costumerPro.Content = "change profile";
                    addReqButton.Content = "save changes";
                    drawY(Int32.Parse(tbStars.Text));
                    break;
                case "View":
                    tbRquestId.IsReadOnly = true;
                    tbRegDate.IsReadOnly = true;
                    tbFirstName.IsReadOnly = true;
                    tbLastName.IsReadOnly = true;
                    cbbStatus.IsEnabled = false;
                    tbMail.IsReadOnly = true;
                    cbbArea.IsEnabled = false;
                    tbSubArea.IsReadOnly = true;
                    cbbVacationType.IsEnabled = false;
                    tbAdults.IsReadOnly = true;
                    tbKids.IsReadOnly = true;
                    tbStars.IsReadOnly = true;
                    ckbPool.IsEnabled = false;
                    cbkJacuzzi.IsEnabled = false;
                    ckbGarden.IsEnabled = false;
                    ckbAttractions.IsEnabled = false;
                    ckbMeals.IsEnabled = false;
                    bstar1.IsEnabled = false;
                    bstar2.IsEnabled = false;
                    bstar3.IsEnabled = false;
                    bstar4.IsEnabled = false;
                    bstar5.IsEnabled = false;
                    bstar1.Visibility = Visibility.Hidden;
                    bstar2.Visibility = Visibility.Hidden;
                    bstar3.Visibility = Visibility.Hidden;
                    bstar4.Visibility = Visibility.Hidden;
                    bstar5.Visibility = Visibility.Hidden;
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
                        Close();
                    break;
            }
        }

        private void addReqButton_Click(object sender, RoutedEventArgs e)
        {
            bool premitionToAdd = true;
            if (String.IsNullOrEmpty(tbFirstName.Text) || String.IsNullOrEmpty(tbLastName.Text) || tbFirstName.Text.Any(char.IsDigit) || tbLastName.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Please make sure to fill the name right", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbFirstName.Background = Brushes.IndianRed;
                tbLastName.Background = Brushes.IndianRed;
                premitionToAdd = false;
                return;
            }
            if (cbbArea.SelectedItem == null)
            {
                MessageBox.Show("please choose your vacation area", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                cbbArea.Background = Brushes.IndianRed;
                premitionToAdd = false;
                return;
            }
            if (cbbVacationType.SelectedItem == null)
            {
                MessageBox.Show("please choose your vacation type", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                cbbVacationType.Background = Brushes.IndianRed;
                premitionToAdd = false;
                return;
            }
            if (!tbAdults.Text.All(char.IsDigit))
            {
                MessageBox.Show("the adults input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbAdults.Background = Brushes.IndianRed;
                premitionToAdd = false;
                return;
            }
            if (!tbKids.Text.All(char.IsDigit))
            {
                MessageBox.Show("the kids input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbKids.Background = Brushes.IndianRed;
                premitionToAdd = false;
                return;
            }
            if (!tbStars.Text.All(char.IsDigit))
            {
                MessageBox.Show("the stars input has to be number", "registration action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                tbStars.Background = Brushes.IndianRed;
                premitionToAdd = false;
                return;
            }
            switch (operation)
            {
                case "Add":
                    if (String.IsNullOrEmpty(tbMail.Text))
                    {
                        premitionToAdd = false;
                        tbMail.Background = Brushes.IndianRed;
           
                    }
                    if (String.IsNullOrEmpty(tbAdults.Text))
                    {
                        premitionToAdd = false;
                        tbAdults.Background = Brushes.IndianRed;
  
                    }
                    else
                    {
                        if (int.Parse(tbAdults.Text) < 1)
                        {
                            tbAdults.Background = Brushes.IndianRed;
                            premitionToAdd = false;
                        }
                    }
                    if (String.IsNullOrEmpty(tbFirstName.Text))
                    {
                        premitionToAdd = false;
                        tbFirstName.Background = Brushes.IndianRed;
                    }
                    if (String.IsNullOrEmpty(tbLastName.Text))
                    {
                        premitionToAdd = false;
                        tbLastName.Background = Brushes.IndianRed;
                    }
                    if (premitionToAdd == true)
                    {
                        GuestRequest guest = guestRequest;//clone//יכול להיות שלא יעבוד כי אין בנאי העתקה
                        try { MainWindow.myBL.addGuestRequest(guest); }
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
                        //Close();
                        if (isImageChanged)
                        {
                            try
                            {
                                MainWindow.myBL.AddCostumerImage(guest.GuestRequestKey, op.FileName);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("failed uploading profile picture" + ex.Message, "action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                            }
                        }
                        Close();//might be in the wrong place
                        MessageBox.Show("the requet has been added successfully", "adding request", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        addedSuccessfuly = true;
                    }
                    else
                        MessageBox.Show("Some details are missing or uncorrect. Please try again.", "action failed", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                    break;
                case "Update":
                    if (premitionToAdd == true)
                        try
                        {
                            MainWindow.myBL.SetGuestRequest(guestRequest);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "internal error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                            return;
                        }
                    addedSuccessfuly = true;
                    Close();
                    CostumerImage.Source = null;
                    if (isImageChanged)
                    {
                        try
                        {
                            MainWindow.myBL.ChangeCostuerImage(guestRequest.GuestRequestKey, op.FileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("failed adding ptofile photo" + ex.Message, "update", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        }
                    }
                    MessageBox.Show("the request details have bee changed successfully", "update", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);

                    break;
            }           
        }

        #region draw - star
        private void drawY(int star)
        {
            if (star < 1)
                return;
            pstar1.Fill = yelf;
            pstar1.Stroke = yels;
            if (star > 1)
            {
                pstar2.Fill = yelf;
                pstar2.Stroke = yels;
                if (star > 2)
                {
                    pstar3.Fill = yelf;
                    pstar3.Stroke = yels;
                    if (star > 3)
                    {
                        pstar4.Fill = yelf;
                        pstar4.Stroke = yels;
                    }
                    if (star > 4)
                    {
                        pstar5.Fill = yelf;
                        pstar5.Stroke = yels;
                    }
                }
            }
        }
        private void drawG(int star)
        {
            pstar5.Fill = grayf;
            pstar5.Stroke = grays;
            if (star < 5)
            {
                pstar4.Fill = grayf;
                pstar4.Stroke = grays;
                if (star < 4)
                {
                    pstar3.Fill = grayf;
                    pstar3.Stroke = grays;
                    if (star < 3)
                    {
                        pstar2.Fill = grayf;
                        pstar2.Stroke = grays;
                        if (star < 2)
                        {
                            pstar1.Fill = grayf;
                            pstar1.Stroke = grays;
                        }
                    }
                }
            }
        }
        private void bstar1_click(object sender, RoutedEventArgs e)
        {
            if (pstar1.Fill == yelf)
            {
                drawG(1);
                tbStars.Text = "0";
            }
            else
            {
                drawY(1);
                tbStars.Text = "1";
            }
        }
        private void bstar2_click(object sender, RoutedEventArgs e)
        {
            if (pstar2.Fill == yelf)
            {
                drawG(2);
                tbStars.Text = "1";
            }
            else
            {
                drawY(2);
                tbStars.Text = "2";
            }
        }
        private void bstar3_click(object sender, RoutedEventArgs e)
        {
            if (pstar3.Fill == yelf)
            {
                drawG(3);
                tbStars.Text = "2";
            }
            else
            {
                drawY(3);
                tbStars.Text = "3";
            }
        }
        private void bstar4_click(object sender, RoutedEventArgs e)
        {
            if (pstar4.Fill == yelf)
            {
                drawG(4);
                tbStars.Text = "3";
            }
            else
            {
                drawY(4);
                tbStars.Text = "4";
            }
        }
        private void bstar5_click(object sender, RoutedEventArgs e)
        {
            if (pstar5.Fill == yelf)
            {
                drawG(5);
                tbStars.Text = "4";
            }
            else
            {
                drawY(5);
                tbStars.Text = "5";
            }
        }
        #endregion
    }
}
