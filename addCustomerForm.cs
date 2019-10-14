using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace AGK_POS
{
    public partial class addCustomerForm : Form
    {
        //Things I need to reconfigure!//
     
        private string customerNumber;
        private string customerFullName;
        private string cellCustomer;
        private string customerEmail;
        private string customerAccBal;        
        private string executionPath = null;
        public string getCustomerAccBal {
            get { return customerAccBal; }
        }
        public string getCustomerNumber
        {
            get { return customerNumber; }
        }
        public string getCustomerFullName
        {
            get { return customerFullName; }
        }
        public string getCustomerCell
        {
            get { return cellCustomer; }
        }
        public string getCustomerEmail
        {
            get { return customerEmail; }
        }
        public addCustomerForm()
        {
            InitializeComponent();
            executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            executionPath = executionPath.Substring(6);
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.RowTemplate.MinimumHeight = 80;
            this.MaximizeBox = false;
        }

        //Modifications to make to get customers from the database....
        public void getCustomers() {
            dataGridView1.Rows.Clear();
            sqlCommandAB addCustomer = new sqlCommandAB();
            DataTable DT = addCustomer.QueryDT("SELECT * FROM customer");
            String[] rows;
            for (int j = 0; j < DT.Rows.Count; j++)
            {
                rows = new string[] { (string)DT.Rows[j]["Customer_Number"], (string)DT.Rows[j]["Customer_Name"], (string)DT.Rows[j]["Customer_Surname"],DT.Rows[j]["Customer_IDNumber"].ToString(), "0" + DT.Rows[j]["Customer_Cell"].ToString(), DT.Rows[j]["Customer_Email"].ToString(),(string)DT.Rows[j]["Customer_Address"]  };
                dataGridView1.Rows.Add(rows);

            }
          
        }
        public void loadData()
        {
            getCustomers();
            int numRow = dataGridView1.Rows.Count;
            totalCustomers.Text = "Total Number Of Customers: " + numRow;
            for (int i = 0; i < numRow; i++)
            {
                try
                {
                    dataGridView1.Rows[i].Cells["Photo"].Value = Image.FromFile(executionPath + @"\CustomerImages\" + dataGridView1.Rows[i].Cells["cNumber"].Value.ToString() + ".bmp");
                }
                catch (Exception)
                {

                    dataGridView1.Rows[i].Cells["Photo"].Value = Properties.Resources.defaultImage.GetThumbnailImage(80,80,null,new IntPtr());

                }

            }
        }
        private void addCustomerForm_Load(object sender, EventArgs e)
        {            
            loadData();
        }



        public event EventHandler cellClicked;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

            customerNumber = (string)(selectedRow.Cells["cNumber"].Value);
            customerFullName = (string)selectedRow.Cells["cName"].Value +" " +(string)selectedRow.Cells["cSurname"].Value;
            cellCustomer = selectedRow.Cells["cCell"].Value.ToString();
            customerEmail = (string)selectedRow.Cells["cEmail"].Value;
           // customerAccBal = selectedRow.Cells["CreditBalance"].Value.ToString();
            if (cellClicked != null)
            {
                cellClicked(this, new EventArgs());
            }
            this.Hide();
        } 
        private void surnameSearchTB_TextChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonAllC_Click(object sender, EventArgs e)
        {   surnameSearchTB.Text = "";
            getCustomers();            
            loadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

