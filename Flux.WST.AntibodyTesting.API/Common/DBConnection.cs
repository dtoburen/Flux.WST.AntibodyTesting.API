using System.Data;
using System.Data.SqlClient;

namespace Flux.WST.AntibodyTesting.API
{
    public class DBConnection
    {
        //This method expects the query in selectSQL to return only 1 row, but up to 10 columns.
        public string[] ExecuteSQL(string selectSQL)
        {
            string[] resultArray = new string[36];

            //SqlConnection connection = new SqlConnection(@sqlConnection);
            SqlConnection connection = new SqlConnection(@"Data Source=bmtfnq19db02;Initial Catalog=HCLL;User ID=sa;Password=medw.123456;");
            SqlDataAdapter adapter = new SqlDataAdapter(selectSQL, connection);
            DataTable dtbl = new DataTable();
            adapter.Fill(dtbl);
            foreach (DataRow row in dtbl.Rows)
            {
                for (int i = 0; i < dtbl.Columns.Count; i++)
                {
                    resultArray[i] = (row[i].ToString());
                    resultArray[i] = resultArray[i].Trim();
                }
            }
            connection.Close();
            return resultArray;
        }

        //This method expects the query in selectSQL to return multiple rows and multiple columns.
        public DataTable ExecuteSQLMultipleRows(string selectSQL)
        {
            //SqlConnection connection = new SqlConnection(@sqlConnection);
            SqlConnection connection = new SqlConnection(@"Data Source=bmtfnq19db02;Initial Catalog=HCLL;User ID=sa;Password=medw.123456;");
            SqlDataAdapter adapter = new SqlDataAdapter(selectSQL, connection);
            DataTable dtbl = new DataTable();
            adapter.Fill(dtbl);
            connection.Close();
            return dtbl;
        }
    }
}
