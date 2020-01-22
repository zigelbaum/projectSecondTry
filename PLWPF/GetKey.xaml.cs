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
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for GetKey.xaml
    /// </summary>
    public partial class GetKey : Window
    {
        string entity;
        public bool retriveSuccess;
        public Int32 numVal=0;
        public GetKey(string entity)
        {
            this.entity = entity;
            retriveSuccess = false;
            InitializeComponent();
            switch(entity)
            {
                case "HostingUnit":
                    idLabel.Content = "Please enter hosting unit key:";
                    break;
                case "Host":
                    idLabel.Content = "Please enter your ID:";
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                numVal = Int32.Parse(tbID.Text);
                switch (this.entity)
                {
                    case "GuestRequest":
                        GuestRequest guestRequest = MainWindow.myBL.FindGuestRequest(numVal);
                        if (guestRequest == null)
                        {
                            numVal = 0;
                            throw new Exception("The guest request you looked for doestn exists in the system");
                        }
                        else
                            retriveSuccess = true;
                        Close();
                        break;
                    case "HostingUnit":
                        HostingUnit hostingUnit = MainWindow.myBL.FindUnit(numVal);
                        if (hostingUnit == null)
                        {
                            numVal = 0;
                            throw new Exception("The hosting unit you looked for doestn exists in the system");
                        }
                        else
                            retriveSuccess = true;
                        Close();
                        break;                    
                    case "Host":
                        List<HostingUnit> hostList = MainWindow.myBL.getHostingUnits(h => h.Owner.Id == numVal);
                        if (hostList.Count() == 0)
                        {
                            numVal = 0;
                            throw new Exception("The ID you looked for doestn exists in the system");
                        }
                        else
                            retriveSuccess = true;
                        Close();
                        break;
                }
            }
            catch(FormatException ex)
            {
                MessageBox.Show("please enter the guest request's key number. \n Please try again.", "incorrect input", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            catch(Exception exe)
            {
                MessageBox.Show(exe.Message, "internal error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                Close();
            }
            

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
