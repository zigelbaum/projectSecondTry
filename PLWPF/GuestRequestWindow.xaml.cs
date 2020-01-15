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
using System.Collections.ObjectModel;
using System.ComponentModel;
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for GuestRequestWindow.xaml
    /// </summary>
    public partial class GuestRequestWindow : Window
    {
        private ObservableCollection<GuestRequest> requestList;


        public GuestRequestWindow()
        {
            InitializeComponent();
        }

        private void addRequestButton_Click(object sender, RoutedEventArgs e)
        { 
            GuestPresentation guestPresentationWindow = new GuestPresentation();
            guestPresentationWindow.ShowDialog();
        }

        private void updateRequestButton_Click(object sender, RoutedEventArgs e)
        {
            GuestRequest request;
            GetKey getKey = new GetKey("GuestRequest");
            getKey.ShowDialog();
            if (getKey.numVal != 0)
            {
                request = MainWindow.myBL.FindGuestRequest(getKey.numVal);
                GuestPresentation presentation = new GuestPresentation(request, "Update");
                presentation.ShowDialog();
            }
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbKey_SearchFilterChanged(object sender, TextChangedEventArgs e)
        {           
                if (requestList != null)
                {
                    ObservableCollection<GuestRequest> it = new ObservableCollection<GuestRequest>((from item in requestList
                                                                                        where CheckIfStringsAreEqual(FirstName.Text, item.FirstName)
                                                                                        select item
                                                                                    into g
                                                                                        where CheckIfStringsAreEqual(LestName.Text, g.LastName)
                                                                                        select g
                                                                                    into j
                                                                                        where CheckIfStringsAreEqual(ID.Text, j.Id)
                                                                                        select j).ToList());
                    TestersList.ItemsSource = it;
                    numOfTesters.Text = it.Count.ToString();
                }            
        }
    }
}
