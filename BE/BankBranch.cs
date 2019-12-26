using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BankBranch
    {
        #region fildes
        /*private*/ Int32 bankNumber;
        /*private*/ string bankName;
        /*private*/ Int32 branchNumber;
        /*private*/ string branchAddress;
        /*private*/ string branchCity;
        /*private*/ long bankAccountNumber;
        #endregion

        #region properties
        public int BankNumber { get => bankNumber; set => bankNumber = value; }
        public string BankName { get => bankName; set => bankName = value; }
        public int BranchNumber { get => branchNumber; set => branchNumber = value; }
        public string BranchAddress { get => branchAddress; set => branchAddress = value; }
        public string BranchCity { get => branchCity; set => branchCity = value; }
        public long BankAccountNumber { get => bankAccountNumber; set => bankAccountNumber = value; }

        #endregion

        #region functions
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }
}
