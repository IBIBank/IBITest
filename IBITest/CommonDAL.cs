using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace IBITest
{
    
    public class CommonDAL
    {
        SqlConnection cn = new SqlConnection(@"Data Source=(localdb)\Projects;Initial Catalog=Database1;Integrated Security=True");//windows auth
	    SqlCommand cmd = new SqlCommand("Select * from customers",cn);
	    cn.Open();
	    Console.WriteLine(cn.State);
    }
}