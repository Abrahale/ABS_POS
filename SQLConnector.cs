using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AGK_POS
{
    class SQLConnector
    {


        private SqlConnection connector;
        private SqlCommand commander;
        private SqlDataAdapter adapter;
        private DataTable dataTable;
        private SqlCommandBuilder commBuilder;
        public bool insertChecker = false;
        public string message = "";
        public SQLConnector(string query)
        {
            //string connString = "Server = 143.128.146.30; Database = group9; User= group9; Password = yrzqrq";
           // String connString = "Server = 41.148.227.158; Database = ABS_POS; User=Abrahale; Password = Abrahale";
            String connString = "Server = localhost\\SQLExpress; Database = ABS_POS; User = Abrahale; Password=Abrahale";
            connector = new SqlConnection(connString);
            commander = new SqlCommand(query, connector);
        }
        public void updateQuery(string query)
        {

            commander.CommandText = query;
        }

        public DataTable getData()
        {

            try
            {
                connector.Open();
                adapter = new SqlDataAdapter(commander);
                dataTable = new DataTable();
                adapter.Fill(dataTable);
                connector.Close();
            }
            catch (Exception)
            {
                
              
            }
            return dataTable;
        }

        public void insertData(DataTable UpdaterTable)
        {
            
            try
            {
                connector.Open();
                commBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(UpdaterTable);
                insertChecker = true;
                connector.Close();
            }
            catch (Exception err )
            {
                message = err.Message;
                connector.Close();
            }
        }

    }
}
