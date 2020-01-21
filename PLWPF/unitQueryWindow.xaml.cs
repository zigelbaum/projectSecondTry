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
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for unitQueryWindow.xaml
    /// </summary>
    public partial class unitQueryWindow : Window
    {
        enum filterRequest { everything, type, area, owner }
        private ObservableCollection<HostingUnit> mainUnitsList = new ObservableCollection<HostingUnit>(MainWindow.myBL.getHostingUnitsList());
        ObservableCollection<IGrouping<Host, HostingUnit>> groupedByOwner;
        ObservableCollection<IGrouping<Enums.Area, HostingUnit>> groupedByArea;
        ObservableCollection<IGrouping<Enums.HostingUnitType, HostingUnit>> groupedByType;
        private ObservableCollection<HostingUnit> listToFilter;

        public unitQueryWindow()
        {
            InitializeComponent();
            listToFilter = mainUnitsList;
            unitsView.ItemsSource = listToFilter;
            cbbGroupBy.ItemsSource = Enum.GetValues(typeof(filterRequest));
            cbbGroupBy.SelectedIndex = 0;
            cbbShowGroup.IsEnabled = false;
        }

        private void tbKey_SearchFilterChanged(object sender, TextChangedEventArgs e)
        {
            if (tbKey.Text != null)
            {
                string searchTxt = tbKey.Text;
                string upper = searchTxt.ToUpper();
                string lower = searchTxt.ToLower();

                var unit = from item in listToFilter
                           let key = item.HostingUnitKey.ToString()
                           where
                           key.StartsWith(lower)
                           || key.StartsWith(upper)
                           || key.Contains(searchTxt)
                           select item;
                unitsView.ItemsSource = unit;
            }
        }

        private void tbID_SearchFilterChanged(object sender, TextChangedEventArgs e)
        {
            if (tbID.Text != null)
            {
                string searchTxt = tbID.Text;
                string upper = searchTxt.ToUpper();
                string lower = searchTxt.ToLower();

                var unit = from item in listToFilter
                           let key = item.Owner.Id.ToString()
                           where
                           key.StartsWith(lower)
                           || key.StartsWith(upper)
                           || key.Contains(searchTxt)
                           select item;

                unitsView.ItemsSource = unit;
            }
        }

        private void tbLastName_SearchFilterChanged(object sender, TextChangedEventArgs e)
        {
            if (tbLastName.Text != null)
            {
                string searchTxt = tbLastName.Text;
                string upper = searchTxt.ToUpper();
                string lower = searchTxt.ToLower();

                var request = from item in listToFilter
                              let key = item.Owner.FamilyName
                              where
                              key.StartsWith(lower)
                              || key.StartsWith(upper)
                              || key.Contains(searchTxt)
                              select item;

                unitsView.ItemsSource = request;
            }
        }

        private void cbbGroupBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int group = (int)cbbGroupBy.SelectedItem;
            switch (group)
            {
                case 0:
                    cbbShowGroup.SelectedItem = null;
                    cbbShowGroup.IsEnabled = false;
                    mainUnitsList = new ObservableCollection<HostingUnit>(MainWindow.myBL.getHostingUnitsList());
                    unitsView.ItemsSource = mainUnitsList;
                    break;
                case 1:
                    groupedByType = new ObservableCollection<IGrouping<Enums.HostingUnitType, HostingUnit>>(MainWindow.myBL.GroupHostingUnitByType());
                    Enums.HostingUnitType[] keysOfType = (from item in groupedByType select item.Key).ToArray();
                    cbbShowGroup.ItemsSource = keysOfType;
                    cbbShowGroup.IsEnabled = true;
                    cbbShowGroup.SelectedItem = 0;
                    break;
                case 2:
                    groupedByArea = new ObservableCollection<IGrouping<Enums.Area, HostingUnit>>(MainWindow.myBL.GroupHostingUnitByArea());
                    Enums.Area[] keysOfArea = (from item in groupedByArea select item.Key).ToArray();
                    cbbShowGroup.ItemsSource = keysOfArea;
                    cbbShowGroup.IsEnabled = true;
                    cbbShowGroup.SelectedItem = 0;
                    break;
                case 3:

                    groupedByOwner = new ObservableCollection<IGrouping<Host, HostingUnit>>(MainWindow.myBL.GroupHostByHostingUnit());
                    Host[] keysOfstatus = (from item in groupedByOwner select item.Key).ToArray();
                    cbbShowGroup.ItemsSource = keysOfstatus;
                    cbbShowGroup.IsEnabled = true;
                    cbbShowGroup.SelectedItem = 0;
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
                    foreach (var item in groupedByType)
                        //
                        if (cbbShowGroup.SelectedItem != null && (int)item.Key == (int)cbbShowGroup.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<HostingUnit>(item.ToList());
                            break;
                        }
                    break;
                case 2:
                    foreach (var item in groupedByArea)
                        //
                        if (cbbShowGroup.SelectedItem != null && (int)item.Key == (int)cbbShowGroup.SelectedItem)
                        {
                            listToFilter = new ObservableCollection<HostingUnit>(item.ToList());
                            break;
                        }
                    break;
                case 3:
                    foreach (var item in groupedByOwner)
                        //
                        if (cbbShowGroup.SelectedItem != null)
                        {
                            listToFilter = new ObservableCollection<HostingUnit>(item.ToList());
                            break;
                        }
                    break;
            }
            unitsView.ItemsSource = listToFilter;
            if ((tbKey.Text.Length != 0 || tbID.Text.Length != 0 || tbLastName.Text.Length != 0) && listToFilter != null)
                tbKey_SearchFilterChanged(null, null);
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            listToFilter = mainUnitsList;
            unitsView.ItemsSource = listToFilter;
        }

        private void MenuItem_Click_Info(object sender, RoutedEventArgs e)
        {
            if (unitsView.SelectedItem != null)
            {
                UnitPresentationWindow presentation = new UnitPresentationWindow(((HostingUnit)unitsView.SelectedItem), "View");
                presentation.ShowDialog();
            }
        }
    }
}
