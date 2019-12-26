using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Host
    {
        #region fileds and properties
        String HostKey { get; set; }
        String PrivateName { get; set; }
        String FamilyName { get; set; }
        String PhoneNumber { get; set; }
        String MailAddress { get; set; }
        BankBranch Bankaccount { get; set; }//might be confused
        String CollectionClearance { get; set; }
        #endregion

        #region functions
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion

        public Host() { }
    }
}
