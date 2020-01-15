using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Host: IComparable
    {
        #region fileds and properties
        public Int32 HostKey { get; set; }
        public String PrivateName { get; set; }
        public String FamilyName { get; set; }
        public string PhoneNumber { get; set; }
        public String MailAddress { get; set; }
        BankBranch bankBranchDetails;
        int bankAccountNumber;
        bool collectionClearance;
        Int32 id;
        public BankBranch BankBranchDetails { get => bankBranchDetails; set => bankBranchDetails = value; }
        public int BankAccountNumber { get => bankAccountNumber; set => bankAccountNumber = value; }
        public bool CollectionClearance { get => collectionClearance; set => collectionClearance = value; }
        public Int32 Id { get => id; set => id = value; }
        #endregion

        #region functions
        public override string ToString()
        {
            string host;
           host= "Host ID: " + HostKey + "@Name: " + PrivateName + " " + FamilyName + "@Phone number: " + PhoneNumber +
                "@Mail address: " + MailAddress + "@Bank Account: " + BankBranchDetails + "@Collection permission: " + CollectionClearance;
            host = host.Replace("@", System.Environment.NewLine);
            return host.ToString();
        }
        public Int32 CompareTo(object obj)
        {
            return this.HostKey.CompareTo((obj as Host).HostKey);
        }
        #endregion

    }
}
