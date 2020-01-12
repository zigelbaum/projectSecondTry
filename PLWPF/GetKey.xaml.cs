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
        int numVal;
        public GetKey(string entity = "")
        {
            this.entity = entity;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Int32 numVal = Int32.Parse(tbID.Text);
                switch (this.entity)
                {
                    case "GuestRequest":                       
                            GuestRequest guestRequest = MainWindow.myBL.FindGuestRequest(numVal);
                        if (guestRequest == null)
                            throw new Exception("The guest request you looked for doestn exists in the system");
                    break;
                    case "HostingUnit":
                        HostingUnit hostingUnit = MainWindow.myBL.FindUnit(numVal);
                        if(hostingUnit==null)
                            throw new Exception("The hosting unit you looked for doestn exists in the system");
                        break;
                    case "Order":
                        Order order = MainWindow.myBL.FindOrder(numVal);
                        if (order == null)
                            throw new Exception("The order you looked for doestn exists in the system");
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
    }
}
