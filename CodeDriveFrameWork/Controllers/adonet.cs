//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;



using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CodeDriveFrameWork.Controllers
{
    public class Class1
    {
        public DataSet sdfa()
        {
            string SQLString = "select rolename from Base_RolesInfo where delflag=0";
            using (SqlConnection connection = new SqlConnection("Data Source =  192.144.132.21;Initial Catalog = HRL;User Id = sa;Password = hrl*8123;"))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                    DataTable dataInfo = ds.Tables[0];
                    for (int i = 0; i < dataInfo.Rows.Count; i++)
                    {
                        if (dataInfo.Rows[i]["rolename"] != null && dataInfo.Rows[i]["rolename"].ToString() != "")
                        {
                            var rew = dataInfo.Rows[i]["rolename"].ToString();
                        }
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }

        }
    }
}