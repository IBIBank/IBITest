using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IBITest.Layers.DAL
{
    public class BankerDAL
    {
        public DataTable GetNewAccountRequests()
        {
            BankDBTableAdapters.NewAccountRequestTableAdapter NewAccReqAdptr = new BankDBTableAdapters.NewAccountRequestTableAdapter();
            BankDB.NewAccountRequestDataTable newaccrequests = NewAccReqAdptr.GetData();

            return newaccrequests;
        }
    }
}