namespace BE
{
    public static class Enums
    {
        public enum HostingUnitType
        {
            Zimmer=1,
            Hotel,
            Camping
        }

        public enum DataSourseType
        {
            List=1,
            XML
        } 

        public enum OrderStatus
        {
            SentEmail=1, NoAnswer, NotAddressedYet, Closed, Active,NotRelevent
        }

        public enum GuestRequestStatus
        { 
            Active=1, 
            ClosedOnTheWeb,
            RequestExpired
        }

        public enum Area
        { 
            All=1, North, South, Center, Jerusalem
        }

        public enum intrested
        { Necessary=1, Possible, NoThanks }
    }



}