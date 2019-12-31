using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using BL;

namespace PL
{
    class Program
    {
        static void Main(string[] args)
        {
            Host host1 = new Host
            {
                HostKey = 10000023,
                PrivateName = "Dan",
                FamilyName = "Monko",
                PhoneNumber = "0587400342",
                MailAddress = "DanMo@gmail.com",
                BankBranchDetails = new BankBranch
                { BankNumber = 4444, BankName = "hapoalim", BranchNumber = 246, BranchAddress = "Amir", BranchCity = "Ramat Gan" },
                BankAccountNumber = 11111,
                CollectionClearance = true                                             
            };
            Host host2 = new Host
            {
                HostKey = 10000024,
                PrivateName = "Michal",
                FamilyName = "Cohen",
                PhoneNumber = "0503198404",
                MailAddress = "MiCohen404@gmail.com",
                BankBranchDetails = new BankBranch
                { BankNumber = 5555, BankName = "leumi", BranchNumber = 159, BranchAddress = "Bezalel", BranchCity = "Beer Sheva" },
                BankAccountNumber = 22222,
                CollectionClearance = false
            };

            IBL my_bl1 = FactoryBL.getBL("List");

            GuestRequest guest1 = new GuestRequest
            {
                PrivateName = "customer1",
                FamilyName = "Levi",
                MailAddress = "customer1@gmail.com",
                Status = Enums.GuestRequestStatus.Active,
                RegistrationDate = new DateTime(2019, 11, 01),
                EnteryDate = new DateTime(2019, 11, 28),
                ReleaseDate = new DateTime(2019, 11, 30),
                Area = Enums.Area.North,
                SubArea = "golan",
                Type = Enums.HostingUnitType.Camping,
                Adults = 2,
                Children = 7,
                Pool = Enums.intrested.Possible,
                Jacuzzi = Enums.intrested.NoThanks,
                Garden = Enums.intrested.Possible,
                ChildrenAttraction = Enums.intrested.Possible,
                Stars = 0,
                Meals = Enums.intrested.Possible
            };
            GuestRequest guest2 = new GuestRequest
            {
                PrivateName = "customer2",
                FamilyName = "Family",
                MailAddress = "Family2@gmail.com",
                Status = Enums.GuestRequestStatus.Active,
                RegistrationDate = DateTime.Now,
                EnteryDate = new DateTime(2020, 05, 01),
                ReleaseDate = new DateTime(2020, 05, 09),
                Area = Enums.Area.South,
                SubArea = " ",
                Type = Enums.HostingUnitType.Zimmer,
                Adults = 3,
                Children = 11,
                Pool = Enums.intrested.Necessary,
                Jacuzzi = Enums.intrested.Possible,
                Garden = Enums.intrested.Necessary,
                ChildrenAttraction = Enums.intrested.NoThanks,
                Stars = 0,
                Meals = Enums.intrested.Possible
            };
            GuestRequest guest3 = new GuestRequest
            {
                PrivateName = "customer3",
                FamilyName = "Nave",
                MailAddress = "NaveCustomers@gmail.com",
                Status = Enums.GuestRequestStatus.Active,
                RegistrationDate = new DateTime(2019, 12, 10),
                EnteryDate = new DateTime(2020, 10, 02),
                ReleaseDate = new DateTime(2019, 10, 06),
                Area = Enums.Area.Jerusalem,
                SubArea = "Jerusalem",
                Type = Enums.HostingUnitType.Hotel,
                Adults = 2,
                Children = 0,
                Pool = Enums.intrested.Possible,
                Jacuzzi = Enums.intrested.Possible,
                Garden = Enums.intrested.Possible,
                ChildrenAttraction = Enums.intrested.NoThanks,
                Stars = 4,
                Meals = Enums.intrested.Necessary
            };

            HostingUnit unit1 = new HostingUnit
            {
                Owner = host1,
                HostingUnitName = "aaaaa",
                HostingUnitType = Enums.HostingUnitType.Hotel,
                Area = Enums.Area.Jerusalem,
                Stars = 5,
                Meals = true
            };
            HostingUnit unit2 = new HostingUnit
            {
                Owner = host2,
                HostingUnitName = "bbbbb",
                HostingUnitType = Enums.HostingUnitType.Zimmer,
                Area = Enums.Area.North,
                Stars = 3,
                Meals = true
            };
            HostingUnit unit3 = new HostingUnit
            {
                Owner = host2,
                HostingUnitName = "ccccc",
                HostingUnitType = Enums.HostingUnitType.Zimmer,
                Area = Enums.Area.North,
                Stars = 3,
                Meals = true
            };

            try
            {
                my_bl1.addGuestRequest(guest1);
                my_bl1.addGuestRequest(guest2);
                //my_bl1.addGuestRequest(guest3);
                my_bl1.addHostingUnit(unit1);
                my_bl1.addHostingUnit(unit2);
                my_bl1.addHostingUnit(unit3);
            }
            catch (Exception a)
            {
                Console.WriteLine(a.Message);
            }

            List<GuestRequest> matchRequests;
            IEnumerable<IGrouping<Host, HostingUnit>> my_units = my_bl1.GroupHostByHostingUnit();
            foreach (IGrouping<Host, HostingUnit> hosting in my_units)
            {
                foreach (HostingUnit unit in hosting)
                {
                    matchRequests = my_bl1.RequestMatchToStipulation(my_bl1.BuildPredicate(unit)); 
                    foreach (GuestRequest guest in matchRequests)
                    {
                        my_bl1.AddOrder(my_bl1.NewOrder(unit1.HostingUnitKey, guest.GuestRequestKey));
                    }
                }
            }
        }  
    }
}
