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
        public Int32 HostKey { get=> HostKey; set=> HostKey=value; }
        public String PrivateName { get=> PrivateName; set=> PrivateName=value; }
        public String FamilyName { get=> FamilyName; set=> FamilyName=value; }
        public string PhoneNumber { get=> PhoneNumber; set=> PhoneNumber=value; }
        public String MailAddress { get=> MailAddress; set=> MailAddress=value; }
        BankBranch bankBranchDetails;
        int bankAccountNumber;
        bool collectionClearance;
        public BankBranch BankBranchDetails { get => bankBranchDetails; set => bankBranchDetails = value; }
        public int BankAccountNumber { get => bankAccountNumber; set => bankAccountNumber = value; }
        public bool CollectionClearance { get => collectionClearance; set => collectionClearance = value; }


        #endregion

        #region functions
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion

    }
}
