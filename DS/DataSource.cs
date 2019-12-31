using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DS
{
    public class DataSource
    {
        public static List<HostingUnit> hostingUnitsCollection = new List<HostingUnit>()
        {
            new HostingUnit(){HostingUnitKey=10000001, HostingUnitName="Chani", Owner=My_Host.First(), Area=Enums.Area.Center, HostingUnitType=Enums.HostingUnitType.Hotel, SubArea = "Tel Aviv", Pool = true, Adults = 2, Kids = 6, Jaccuzi = true, Garden = false, ChildrenAttraction = true, Meals = true, Stars=5},
            new HostingUnit(){HostingUnitKey=10000002, HostingUnitName="Shira Tel", Owner=My_Host.First(), Area=Enums.Area.Jerusalem, HostingUnitType=Enums.HostingUnitType.Zimmer, SubArea = "Jerusalem", Pool = true, Adults = 10, Kids = 40, Jaccuzi = false, Garden = true, ChildrenAttraction = false, Meals = true, Stars=3},
            new HostingUnit(){HostingUnitKey=10000003, HostingUnitName="Roni", Owner=My_Host.Last(), Area=Enums.Area.North, HostingUnitType=Enums.HostingUnitType.Camping, SubArea = "Golan", Pool = true, Adults = 2, Kids = 10, Jaccuzi = false, Garden = true, ChildrenAttraction = false, Meals = false, Stars = 1}
        };
        public static List<Order> ordersCollection = new List<Order>()
        {
            new Order(){HostingUnitKey=10000001,GuestRequestKey=10000011,OrderKey=10000111,OrderStatus=Enums.OrderStatus.Active, CreateDate=new DateTime(2019, 10, 04), OrderDate=new DateTime(2019, 10, 04)},
            new Order(){HostingUnitKey=10000002,GuestRequestKey=10000012,OrderKey=10000211,OrderStatus=Enums.OrderStatus.NoAnswer, CreateDate=new DateTime(2005, 04, 16), OrderDate=new DateTime(2005, 04, 19)},
            new Order(){HostingUnitKey=10000003,GuestRequestKey=10000013,OrderKey=10000311,OrderStatus=Enums.OrderStatus.Closed, CreateDate=new DateTime(2019, 11, 29), OrderDate=new DateTime(2019, 12, 01)}
        };
        public static List<GuestRequest> guestRequestsCollection = new List<GuestRequest>();
      /*  {
            new GuestRequest(){GuestRequestKey=10000011, PrivateName="Avrohom", FamilyName="Diter", MailAddress="dit@gmail.com",Status=Enums.GuestRequestStatus.Active, RegistrationDate=new DateTime(2019, 10, 04), EnteryDate=new DateTime(2019, 10, 20), ReleaseDate=new DateTime(2019, 10, 28), Area=Enums.Area.Center, SubArea="Tel Aviv", Type=Enums.HostingUnitType.Hotel, Adults=2, Children=5, Pool=Enums.intrested.Necessary, Jacuzzi=Enums.intrested.Possible, Garden=Enums.intrested.Possible, ChildrenAttraction=Enums.intrested.Necessary, Meals = Enums.intrested.Necessary, Stars = 4},
            new GuestRequest(){GuestRequestKey=10000012, PrivateName="Yizchak", FamilyName="Molivt", MailAddress="Mol@gmail.com",Status=Enums.GuestRequestStatus.RequestExpired, RegistrationDate=new DateTime(2005, 04, 16), EnteryDate=new DateTime(2005, 08, 03), ReleaseDate=new DateTime(2005, 08, 10), Area=Enums.Area.All, SubArea=" ", Type=Enums.HostingUnitType.Zimmer, Adults=6, Children=14, Pool=Enums.intrested.Possible, Jacuzzi=Enums.intrested.Possible, Garden=Enums.intrested.Necessary, ChildrenAttraction=Enums.intrested.Possible, Meals = Enums.intrested.Possible, Stars = 3},
            new GuestRequest(){GuestRequestKey=10000013, PrivateName="Yaakov", FamilyName="Zigbel", MailAddress="yaZ@gmail.com",Status=Enums.GuestRequestStatus.ClosedOnTheWeb, RegistrationDate=new DateTime(2019, 11, 29), EnteryDate=new DateTime(2020, 01, 05), ReleaseDate=new DateTime(2020, 01, 20), Area=Enums.Area.North, SubArea=" ", Type=Enums.HostingUnitType.Camping, Adults=2, Children=6, Pool=Enums.intrested.NoThanks, Jacuzzi=Enums.intrested.NoThanks, Garden=Enums.intrested.Necessary, ChildrenAttraction=Enums.intrested.NoThanks, Meals = Enums.intrested.NoThanks, Stars = 0}
        };*/

        #region BankBranch
        static BankBranch BB1 = new BankBranch() { BankNumber = 1111, BankName = "yahav", BranchNumber = 987, BranchAddress = "Hagalil", BranchCity = "Bet Shemesh" };
        static BankBranch BB2 = new BankBranch() { BankNumber = 2222, BankName = "mizrahi", BranchNumber = 975, BranchAddress = "Yafo", BranchCity = "Tel Aviv" };
        static BankBranch BB3 = new BankBranch() { BankNumber = 3333, BankName = "pagi", BranchNumber = 234, BranchAddress = "King Daivid", BranchCity = "Jerusalem" };
        static BankBranch BB4 = new BankBranch() { BankNumber = 4444, BankName = "hapoalim", BranchNumber = 246, BranchAddress = "Amir", BranchCity = "Ramat Gan" };
        static BankBranch BB5 = new BankBranch() { BankNumber = 5555, BankName = "leumi", BranchNumber = 159, BranchAddress = "Bezalel", BranchCity = "Beer Sheva" };
        #endregion

        public static List<BankBranch> BankBranchesCollection = new List<BankBranch>() { BB1, BB2, BB3, BB4, BB5 };

        internal static List<Host> My_Host = new List<Host>
        {
            new Host(){HostKey=11111111, PrivateName="Yael", FamilyName="Shilo", PhoneNumber="0587100429", MailAddress="YSH1234@gmail.com", CollectionClearance=true, BankBranchDetails=BB1, BankAccountNumber=13579246},
            new Host(){HostKey=12121212, PrivateName="Abigail", FamilyName="Cohen", PhoneNumber="0508456123", MailAddress="abigail12@gmail.com", CollectionClearance=true, BankBranchDetails=BB2, BankAccountNumber=24680135},
            new Host(){HostKey=12222222, PrivateName="Rebeka", FamilyName="Levi", PhoneNumber="0503681400", MailAddress="Levi400@gmail.com", CollectionClearance=false, BankBranchDetails=BB3, BankAccountNumber=1000034}
        };

   
    }
}
