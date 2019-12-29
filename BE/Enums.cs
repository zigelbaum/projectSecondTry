namespace BE
{
    public static class Enums
    {
        public enum HostingUnitType
        {
            Zimmer,
            Hotel,
            Camping
        }

        public enum DataSourseType
        {
            List,
            XML
        } 

        public enum OrderStatus
        {
            SentEmail, NoAnswer, NotAddressedYet, Closed, Active,NotRelevent
        }

        public enum GuestRequestStatus
        { 
            Active, 
            ClosedOnTheWeb,
            RequestExpired
        }

        public enum Area
        { 
            All, North, South, Center, Jerusalem
        }

        public enum intrested
        { Necessary, Possible, NoThanks }
    }



}