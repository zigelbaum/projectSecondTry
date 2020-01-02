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
            #region new data
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
                EnteryDate = new DateTime(2019, 11, 28),
                ReleaseDate = new DateTime(2019, 11, 30),
                Area = Enums.Area.South,
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
                EnteryDate = new DateTime(2020, 05, 01),
                ReleaseDate = new DateTime(2020, 05, 09),
                Area = Enums.Area.North,
                SubArea = "Eilat",
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
            #endregion

            bool flag = false;

            do
            {
                try
                {
                    if (!flag)
                    {
                        #region add
                        #region add to guestRequest
                        my_bl1.addGuestRequest(guest1);
                        my_bl1.addGuestRequest(guest2);                       
                        #endregion
                        #region add to hostingUnit
                        my_bl1.addHostingUnit(unit1);
                        my_bl1.addHostingUnit(unit2);
                        my_bl1.addHostingUnit(unit3);
                        #endregion
                        #endregion 
                        #region delete hostingUnit   
                        IEnumerable<HostingUnit> bla = my_bl1.getHostingUnitsList();
                        foreach (var vvv in bla)
                        {
                            Console.WriteLine(vvv.ToString());
                            Console.WriteLine();
                        }
                        my_bl1.DeleteHostingUnit(unit3);
                        bla = my_bl1.getHostingUnitsList();
                        foreach (var vvv in bla)
                        {
                            Console.WriteLine(vvv.ToString());
                            Console.WriteLine();
                        }
                        #endregion
                        #region  use grouping
                        List<GuestRequest> matchRequests;
                        IEnumerable<IGrouping<Host, HostingUnit>> my_units = my_bl1.GroupHostByHostingUnit();
                        foreach (IGrouping<Host, HostingUnit> hosting in my_units)
                        {
                            foreach (HostingUnit unit in hosting)
                            {
                                matchRequests = my_bl1.RequestMatchToStipulation(my_bl1.BuildPredicate(unit));
                                foreach (GuestRequest guest in matchRequests)
                                {
                                    int orderKey;
                                    orderKey = my_bl1.AddOrder(my_bl1.NewOrder(unit1.HostingUnitKey, guest.GuestRequestKey));
                                    Console.WriteLine(my_bl1.getOrders(x => x.OrderKey == orderKey).Find(x => x.OrderKey == orderKey).ToString());
                                }
                            }
                        }
                        IEnumerable<IGrouping<Enums.Area, GuestRequest>> guests_by_area = my_bl1.GroupGRByArea();
                        foreach(IGrouping<Enums.Area, GuestRequest> requests in guests_by_area)
                        {
                            foreach(GuestRequest request in requests)
                            {
                                Console.WriteLine();
                                Console.WriteLine(request.ToString());
                            }
                        }
                        IEnumerable<IGrouping<int, GuestRequest>> num_vaca = my_bl1.GroupGRByVacationers();
                        foreach(IGrouping<int, GuestRequest> gu in num_vaca)
                        {
                            foreach(GuestRequest item in gu)
                            {
                                Console.WriteLine();
                                Console.WriteLine(item.ToString());
                            }
                        }
                        IEnumerable<IGrouping<Enums.Area, HostingUnit>> hosting_by_area = my_bl1.GroupHostingUnitByArea();
                        foreach(IGrouping < Enums.Area, HostingUnit> hostings in hosting_by_area)
                        {
                            foreach(HostingUnit unit in hostings)
                            {
                                Console.WriteLine();
                                Console.WriteLine(unit.ToString());
                            }
                        }
                        #endregion
                        #region upload
                        #region set order
                        my_bl1.setOrder(new Order() { HostingUnitKey = 10040002, GuestRequestKey = 10000012, OrderKey = 10000211, OrderStatus = Enums.OrderStatus.SentEmail, CreateDate = new DateTime(2005, 04, 16), OrderDate = new DateTime(2005, 04, 19) });
                        List<Order> orders = my_bl1.GetOrdersList();
                        foreach (Order ord in orders)
                        {
                            Console.WriteLine();
                            Console.WriteLine(ord.ToString());
                        }
                        #endregion
                        #region set guestRequest
                        List<GuestRequest> guestRequests = my_bl1.GetGuestRequestsList();
                        foreach (GuestRequest request in guestRequests)
                        {
                            Console.WriteLine();
                            Console.WriteLine(request.ToString());
                        }
                        my_bl1.SetGuestRequest(new GuestRequest
                        {
                            GuestRequestKey = 10000000,
                            PrivateName = "customer1",
                            FamilyName = "Levi",
                            MailAddress = "customer1@gmail.com",
                            EnteryDate = new DateTime(2019, 11, 28),
                            ReleaseDate = new DateTime(2019, 11, 30),
                            Area = Enums.Area.North,
                            SubArea = "golan",
                            Status = Enums.GuestRequestStatus.ClosedOnTheWeb,
                            Type = Enums.HostingUnitType.Camping,
                            Adults = 2,
                            Children = 7,
                            Pool = Enums.intrested.Possible,
                            Jacuzzi = Enums.intrested.NoThanks,
                            Garden = Enums.intrested.Possible,
                            ChildrenAttraction = Enums.intrested.Possible,
                            Stars = 0,
                            Meals = Enums.intrested.Possible
                        });
                        List<GuestRequest> lists = my_bl1.GetGuestRequestsList();
                        foreach (GuestRequest request in lists)
                        {
                            Console.WriteLine();
                            Console.WriteLine(request.ToString());
                        }
                        #endregion
                        #region set hostingUnit
                        List<HostingUnit> myUnits = my_bl1.getHostingUnitsList();
                        foreach (HostingUnit unit in myUnits)
                        {
                            Console.WriteLine();
                            Console.WriteLine(unit.ToString());
                        }
                        my_bl1.SetHostingUnit(new HostingUnit
                        {
                            HostingUnitKey = 10000001,
                            Owner = host2,
                            HostingUnitName = "new name",
                            HostingUnitType = Enums.HostingUnitType.Zimmer,
                            Area = Enums.Area.North,
                            Stars = 3,
                            Meals = true
                        });
                        List<HostingUnit> units = my_bl1.getHostingUnitsList();
                        foreach (HostingUnit unit in units)
                        {
                            Console.WriteLine();
                            Console.WriteLine(unit.ToString());
                        }
                        #endregion
                        #endregion
                        my_bl1.addGuestRequest(guest3);
                    }
                    else
                    {
                        Console.WriteLine("you need to entry new data");
                        flag = false;
                    }
                }
                catch (Exception a)
                {
                    Console.WriteLine(a.Message);
                    flag = true;
                }
            } while (flag);
        }  
    }
}
