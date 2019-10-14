using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace AGK_POS
{
    class ConnectionDB
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;
        SqlCommandBuilder commandBuilder;

        public ConnectionDB() {
            string connectionString = "Server = DESKTOP-JDPH7S8\\SQLEXPRESS; Database = Araia_Investments; User= DESKTOP-JDPH7S8\\pc; Password = Abrahale";
            connection = new  SqlConnection(connectionString);
            command = new SqlCommand("SELECT * FROM users",connection);
        }
        public DataTable QueryDT(){
            connection.Open();
            dataAdapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;
        }
        public void Update(DataTable dataTable){
            connection.Open();
            commandBuilder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.Update(dataTable);
            connection.Close();
        }
    }
}
