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
        public static List<HostingUnit> hostingUnitsCollection = new List<HostingUnit>();
        public static List<Order> orders = new List<Order>();
        public static List<GuestRequest> guestRequestsCollection = new List<GuestRequest>()
        {
            new GuestRequest(){PrivateName="Esther", FamilyName="burack", MailAddress="stburack@gmail.com",Status=(Enums.GuestRequestStatus)1,RegistrationDate=new DateTime(2020, 09, 09), EnteryDate=new DateTime(2020, 09, 25), ReleaseDate=new DateTime(2020, 09, 28), Area=Enums.Area.Jerusalem, SubArea="Old City", Type=VacationType.Hut, Adults=2, Children=3, Pool=Choices.Yes, Jacuzzi=Choices.No, Garden=Choices.DontCare, ChildrensAttractions=Choices.Yes, FitnessCenter=Choices.DontCare, Parking=Choices.Yes, GuestRequestKey=12345678, Pet=false, Stars=StarRating.four_star, WiFi=Choices.Yes},
            new GuestRequest(){PrivateName="Aviva", FamilyName="Nam", MailAddress="Man@gmail.com",Status=(Enums.GuestRequestStatus)0,RegistrationDate=new DateTime(2019, 08, 10), EnteryDate=new DateTime(2019, 08, 29), ReleaseDate=new DateTime(2019, 09, 03), Area=Enums.Area.North, SubArea="Ramot", Type=VacationType.LogCabin, Adults=4, Children=17, Pool=Choices.No, Jacuzzi=Choices.Yes, Garden=Choices.Yes, ChildrensAttractions=Choices.DontCare, WiFi=Choices.No, Stars=StarRating.five_star, FitnessCenter=Choices.Yes, GuestRequestKey=12365479, Parking=Choices.Yes, Pet=true},
            new GuestRequest(){PrivateName="Sara", FamilyName="Teig", MailAddress="Teig@gmail.com",Status=(Enums.GuestRequestStatus)2,RegistrationDate=new DateTime(2020, 03, 16), EnteryDate=new DateTime(2020, 04, 01), ReleaseDate=new DateTime(2020, 04, 04), Area=Enums.Area.South, SubArea="Eilat", Type=VacationType.Hotel, Adults=1, Children=8, Pool=Choices.Yes, Jacuzzi=Choices.Yes, Garden=Choices.No, ChildrensAttractions=Choices.Yes, Pet=false, Parking=Choices.DontCare, GuestRequestKey=98765432, FitnessCenter=Choices.DontCare, Stars=StarRating.unrated, WiFi=Choices.Yes }
        };



        #region data


        internal static List<GuestRequest> requestCollection = new List<GuestRequest>()
        {
            new GuestRequest(){/*GuestRequestKey=10000011,*/ PrivateName="Esther", FamilyName="burack", MailAddress="stburack@gmail.com",Status=Status.NotAddressedYet,RegistrationDate=new DateTime(2020, 09, 09), EntryDate=new DateTime(2020, 09, 25), ReleaseDate=new DateTime(2020, 09, 28), Area=VacationArea.Center, SubArea=VacationSubArea.BatYam, Type=VacationType.Hut, Adults=2, Children=3, Pool=Choices.Yes, Jacuzzi=Choices.No, Garden=Choices.DontCare, ChildrensAttractions=Choices.Yes, FitnessCenter=Choices.DontCare, Parking=Choices.Yes, GuestRequestKey=12345678, Pet=false, Stars=StarRating.four_star, WiFi=Choices.Yes},
            new GuestRequest(){PrivateName="Aviva", FamilyName="Nam", MailAddress="Man@gmail.com",Status=Status.Closed,RegistrationDate=new DateTime(2019, 08, 10), EntryDate=new DateTime(2019, 08, 29), ReleaseDate=new DateTime(2019, 09, 03), Area=VacationArea.East, SubArea=VacationSubArea.Arad, Type=VacationType.LogCabin, Adults=4, Children=17, Pool=Choices.No, Jacuzzi=Choices.Yes, Garden=Choices.Yes, ChildrensAttractions=Choices.DontCare, WiFi=Choices.No, Stars=StarRating.five_star, FitnessCenter=Choices.Yes, GuestRequestKey=12365479, Parking=Choices.Yes, Pet=true},
            new GuestRequest(){PrivateName="Sara", FamilyName="Teig", MailAddress="Teig@gmail.com",Status=Status.Active,RegistrationDate=new DateTime(2020, 03, 16), EntryDate=new DateTime(2020, 04, 01), ReleaseDate=new DateTime(2020, 04, 04), Area=VacationArea.South, SubArea=VacationSubArea.Netanya, Type=VacationType.Hotel, Adults=1, Children=8, Pool=Choices.Yes, Jacuzzi=Choices.Yes, Garden=Choices.No, ChildrensAttractions=Choices.Yes, Pet=false, Parking=Choices.DontCare, GuestRequestKey=98765432, FitnessCenter=Choices.DontCare, Stars=StarRating.unrated, WiFi=Choices.Yes }

        };

        internal static List<Host> My_Host = new List<Host>
        {
            new Host(){HostKey=11111111, PrivateName="Yael", FamilyName="Shilo", PhoneNumber=0587100429, MailAddress="YSH1234@gmail.com", CollectionClearance=true, BankBranchDetails=B1, BankAccountNumber=13579246},
            new Host(){HostKey=12121212, PrivateName="Abigail", FamilyName="Cohen", PhoneNumber=0508456123, MailAddress="tamarbuterman@gmail.com", CollectionClearance=true, BankBranchDetails=B2, BankAccountNumber=24680135},
            new Host(){HostKey=12222222, PrivateName="Rebeka", FamilyName="Levi", PhoneNumber=0503681400, MailAddress="Levi400@gmail.com", CollectionClearance=false, BankBranchDetails=B3, BankAccountNumber=1000034}
        };

        internal static List<HostingUnit> HostingUnitCollection = new List<HostingUnit>()
        {
            new HostingUnit(){HostingUnitKey=10000001, HostingUnitName="Chani", Owner=My_Host.Last(), /*WiFi=true, Stars=StarRating.three_star, FitnessCenter=false,*/ Area=Enums.Area.Center, /*Beds=6, ChildrensAttractions=true, Garden=true,Jacuzzi=true, Parking=true, Pet=false, SubArea=VacationSubArea.Jerusalem, Pool=true,*/ HostingUnitType=Enums.HostingUnitType.Hotel},
            new HostingUnit(){HostingUnitKey=10000002, HostingUnitName="Shira Tel", Owner=My_Host.Last(),/* WiFi=true, Stars=StarRating.five_star, FitnessCenter=true,*/ Area=Enums.Area.Jerusalem, /*Beds=5, ChildrensAttractions=true, Garden=true, Jacuzzi=true, Parking=true, Pet=true, SubArea=VacationSubArea.Netanya, Pool=true,*/ HostingUnitType=Enums.HostingUnitType.Hotel},
            new HostingUnit(){HostingUnitKey=10000003, HostingUnitName="Roni", Owner=My_Host.First(), /*WiFi=false, Stars=StarRating.three_star, FitnessCenter=false,*/ Area=Enums.Area.North, /*Beds=9, ChildrensAttractions=false, Garden=false, Jacuzzi=false, Parking=false, SubArea=VacationSubArea.TelAviv, Pool=false,*/ HostingUnitType=Enums.HostingUnitType.Zimmer/*, Pet=true*/}

        };

        internal static List<Order> OrderCollection = new List<Order>()
        {
            new Order(){HostingUnitKey=10000001,GuestRequestKey=10000011,OrderKey=10000111,Status=Status.NoAnswer, CreateDate=new DateTime(2019, 10, 04), OrderDate=new DateTime(2019, 10, 04), SentEmail=default(DateTime)},
            new Order(){HostingUnitKey=10000002,GuestRequestKey=10000012,OrderKey=10000211,Status=Status.Closed, CreateDate=new DateTime(2005, 04, 16), OrderDate=new DateTime(2005, 04, 19), SentEmail=default(DateTime)/*new DateTime(2019, 04, 25)*/},
            new Order(){HostingUnitKey=10000003,GuestRequestKey=10000013,OrderKey=10000311,Status=Status.Active, CreateDate=new DateTime(2019, 11, 29), OrderDate=new DateTime(2019, 12, 01), SentEmail=default(DateTime)}
        };

        #endregion
    }
}
