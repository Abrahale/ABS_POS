using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AGK_POS
{
    class sqlCommandAB
    {      
        private SqlDataAdapter AbDA;
        private DataTable AbDT;
        private SqlCommand command;
        private SqlCommandBuilder CmdBuilder;
        private SqlConnection connection;
        String connectionString = "Server = 143.128.146.30; Database = group9; User= group9; Password = yrzqrq";
       // String connectionString = "Server = 41.148.227.158; Database = ABS_POS; User=Abrahale; Password = Abrahale";
       // String connectionString = "Server = localhost\\SQLExpress; Database = ABS_POS; User = Abrahale; Password=Abrahale";
        //String connectionString = "Server = 143.128.146.30; Database = group10; User= group10; Password = rezfva";
      public  static String message;
        public sqlCommandAB()
        {           
            connection = new SqlConnection(connectionString);            
        }        
        public DataTable QueryDT(String com)
        {
            command = new SqlCommand(com, connection);
            try
            {

                connection.Open();
                AbDA = new SqlDataAdapter(command);
                AbDT = new DataTable();
                AbDA.Fill(AbDT);
                connection.Close();
                return AbDT;
            }
            catch (SqlException c ) {
                message = "There is an Error connecting to the Database";
                return new DataTable();
            }      

        }

        public void UpDate(DataTable AbDT)
        {
            try
            {
                connection.Open();
                CmdBuilder = new SqlCommandBuilder(AbDA);
                AbDA.Update(AbDT);
                connection.Close();
            }
            catch (SqlException z)
            {
                message = "Database cannot be updated, due to System Error,\n  Contact your database Administrator";
            }


        }
        public int getSingleValue(string com) {
            command = new SqlCommand(com, connection);
            int value=0;
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                value = Convert.ToInt32(result);
            }
            catch (SqlException)
            {
                message = "There is an Error connecting to the Database";

            }
            finally {
                connection.Close();
            }
           return value;
        }
        public string getSingleString(string com)
        {
            command = new SqlCommand(com, connection);
            string value="";
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                value = result.ToString();
            }
            catch (SqlException)
            {
                message = "There is an Error connecting to the Database server";

            }
            catch (Exception) {
                message += "There is an error, Please seek tecknician";
            }
            finally
            {
                connection.Close();
            }
            return value;
        }

        public void updateDB(string com) {
            command = new SqlCommand(com, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                //I will make use of this later if I get time, to print error messages.
                message = "There is an Error connecting to the Database server";
            }
            finally {
                connection.Close();
            }
        }

    }
}
