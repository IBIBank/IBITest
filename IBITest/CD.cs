using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace IBITest
{
    public class CD
    {
        public void Getd()
        {
            SqlConnection cn = new SqlConnection("Data Source=(localdb)\\Projects;Initial Catalog=Database1;Integrated Security=True");//windows auth

            MessageBox.Show(cn.State.ToString());
        }

       
	    
	    
    }
}