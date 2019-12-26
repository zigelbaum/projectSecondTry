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
        String HostKey { get=> HostKey; set=> HostKey=value; }
        String PrivateName { get=> PrivateName; set=> PrivateName=value; }
        String FamilyName { get=> FamilyName; set=> FamilyName=value; }
        String PhoneNumber { get=> PhoneNumber; set=> PhoneNumber=value; }
        String MailAddress { get=> MailAddress; set=> MailAddress=value; }
        BankBranch bankBranchDetails {get=> bankBranchDetails; set=> bankBranchDetails=value;}
        int BankAccountNumber { get=> BankAccountNumber; set=> BankAccountNumber=value; }//might be confused
        bool CollectionClearance {get=> CollectionClearance; set=> CollectionClearance=value;}
        #endregion

        #region functions
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion

    }
}
