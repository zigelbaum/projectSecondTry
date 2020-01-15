﻿using System;
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
        enum filterRequest { everything , status, area, type, stars }
        private ObservableCollection<GuestRequest> requestsList = new ObservableCollection<GuestRequest>(MainWindow.myBL.GetGuestRequestsList());
        ObservableCollection<IGrouping<Enums.GuestRequestStatus, GuestRequest>> groupedByStatus;
        ObservableCollection<IGrouping<Enums.Area, GuestRequest>> groupedByArea;
        ObservableCollection<IGrouping<Enums.HostingUnitType, GuestRequest>> groupedByType;
        ObservableCollection<IGrouping<int, GuestRequest>> groupedByStars;
        private ObservableCollection<GuestRequest> listToFilter;
        public GuestRequestWindow()
        {
            InitializeComponent();
            requestView.ItemsSource = requestsList;
            cbbGroupBy.ItemsSource = Enum.GetValues(typeof(filterRequest));
            cbbGroupBy.SelectedIndex = 1;
            cbbShowGroup.IsEnabled = false;
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

        }

        private void cbbGroupBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int group = (int)cbbGroupBy.SelectedItem;
            switch (group)
            {
                case 0:
                    //dont know if works
                    cbbShowGroup.IsEnabled = false;
                    requestsList = new ObservableCollection<GuestRequest>(MainWindow.myBL.GetGuestRequestsList());
                    requestView.ItemsSource = requestsList;
                    break;
                case 1:
                    groupedByStatus = new ObservableCollection<IGrouping<Enums.GuestRequestStatus, GuestRequest>>(MainWindow.myBL.GroupGRByStatus());
                    Enums.GuestRequestStatus[] keysOfstatus = (from item in groupedByStatus select item.Key).ToArray();
                    cbbShowGroup.ItemsSource = keysOfstatus;
                    cbbShowGroup.IsEnabled = true;
                    break;
                case 2:
                    groupedByArea = new ObservableCollection<IGrouping<Enums.Area, GuestRequest>>(MainWindow.myBL.GroupGRByArea());
                    Enums.Area[] keysOfArea = (from item in groupedByArea select item.Key).ToArray();
                    cbbShowGroup.ItemsSource = keysOfArea;
                    cbbShowGroup.IsEnabled = true;
                    break;
                case 3:
                    groupedByType = new ObservableCollection<IGrouping<Enums.HostingUnitType, GuestRequest>>(MainWindow.myBL.GroupGRByType());
                    Enums.HostingUnitType[] keysOfType = (from item in groupedByType select item.Key).ToArray();
                    cbbShowGroup.ItemsSource = keysOfType;
                    cbbShowGroup.IsEnabled = true;
                    break;
                case 4:
                    groupedByStars = new ObservableCollection<IGrouping<int, GuestRequest>>(MainWindow.myBL.GroupGRByStars());
                    int[] keysOfStars = (from item in groupedByStars select item.Key).ToArray();
                    cbbShowGroup.ItemsSource = keysOfStars;
                    cbbShowGroup.IsEnabled = true;
                    break;
            }
        }

        private void cbbShowGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int group = (int)cbbGroupBy.SelectedItem;
            switch (group)
            {
                case 0:
                    break;
                case 1:
                    foreach (var item in groupedByStatus)
                        //
                        if (cbbShowGroup.SelectedItem != null && (int)item.Key ==(int)cbbShowGroup.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<GuestRequest>(item.ToList());
                            break;
                        }
                    break;
                case 2:
                    foreach (var item in groupedByArea)
                        //
                        if (cbbShowGroup.SelectedItem != null && (int)item.Key == (int)cbbShowGroup.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<GuestRequest>(item.ToList());
                            break;
                        }
                    break;
                case 3:
                    foreach (var item in groupedByType)
                        //
                        if (cbbShowGroup.SelectedItem != null && (int)item.Key == (int)cbbShowGroup.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<GuestRequest>(item.ToList());
                            break;
                        }
                    break;
                case 4:
                    foreach (var item in groupedByStars)
                        //
                        if (cbbShowGroup.SelectedItem != null && (int)item.Key == (int)cbbShowGroup.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<GuestRequest>(item.ToList());
                            break;
                        }
                    break;
            }
            requestView.ItemsSource = listToFilter;

        }
    }
}
