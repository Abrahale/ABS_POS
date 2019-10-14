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
    public partial class Customer : UserControl
    {
        private string name;
        private string surname;
        private string gender;
        private string email;
        private long IDnumber;
        private long cell;
        private long tel;
        private string address;
        private float creditLimit;
        private float creditBalance;
        private string username;
        private string password;
        private string customer_Number;
        //From Bonga's Code
        private OpenFileDialog loader = new OpenFileDialog();
        Image img;
        private string executionPath = null;

        public Customer()
        {
            InitializeComponent();
            dataGridView1.RowTemplate.Height = 80;
            cNumberTB.Text = makeCustomerNumber();
            
            Customer[] customers;
            getCatData(out customers);
            loadData();
            executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            executionPath = executionPath.Substring(6);


        }
        public Customer(string name,string cNum, string surname, long IDnumber,
            string email, long cell, long tel, string address, string gender) {
                this.name = name;
                this.surname = surname;
                this.email = email;
                this.address = address;
                this.cell = cell;
                this.tel = tel;
                this.IDnumber = IDnumber;
                this.gender = gender;
                this.username = cNum;
                this.password = makePassword(name, surname);
                this.creditBalance = 0;
                this.creditLimit = 0;
               
            
        }
        public void InserNewCustomer(Customer customer)
        {
            sqlCommandAB brandSql = new sqlCommandAB();
            DataTable dt = brandSql.QueryDT("SELECT * FROM Customer");
            DataRow dr = dt.NewRow();
            dr["Customer_Number"] = customer.username;
            dr["Customer_Name"] = customer.name ;
            dr["Customer_Surname"] = customer.surname;
            dr["Customer_IDNumber"] = customer.IDnumber;
            dr["Customer_Gender"] = customer.gender;
            dr["Customer_Cell"] =customer.cell ;
            dr["Customer_Tel"] = customer.tel;
            dr["Customer_Email"] = customer.email;
            dr["Customer_Address"] = customer.address;
            dr["Customer_CreditLimit"] =customer.creditLimit;
            dr["Customer_CreditBalance"] = customer.creditBalance ;            
            dr["Customer_Password"] = customer.password;
            dt.Rows.Add(dr);
            
            brandSql.UpDate(dt);
           
        }
       
        public string makeCustomerNumber() {
           
            Random random = new Random();
             int  username = random.Next(1111,9999999);      
            return username.ToString();

        }
        public string makePassword(string name, string surname) {
            string password = "";
            password +="cust"+name[0] + surname[0] + name[1] + surname[1];
            return password;


        }
        private void button7_Click(object sender, EventArgs e)
        {
           // Application.Restore();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            customerL.Text = "New Customer";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            customerL.Text = "Edit Customer Details";
        }

        public Boolean  validateCustomer() {

            if (nameTB.Text == "" || surnameTB.Text == "" || emailTB.Text == "" ||  countryTB.Text == "")
            {
                MessageBox.Show("Please add all Customer Details");
                return true;
            }
            else if (idNumberTB.Text.Length != 13 || !idNumberTB.Text.All(char.IsDigit))
            {
               
                MessageBox.Show("Please Enter ID number again.");
                idNumberTB.Text = "";
                idNumberTB.Focus();
                return true;
            }
            else if (cellTB.Text.Length !=10 || !cellTB.Text.All(char.IsDigit))
            {
            
                MessageBox.Show("Please Enter cellphone number without spaces");
                cellTB.Text = "";
                cellTB.Focus();
                return true;
            }
            else if (telTB.Text.Length != 10 || !telTB.Text.All(char.IsDigit))
            {
                
                MessageBox.Show("Please Enter tel-phone number without spaces");
                telTB.Text = "";
                telTB.Focus();
                return true;
            }
            else if (genderCB.Text.Equals("")) {
                MessageBox.Show("Please select gender");
                genderCB.Focus();
                return true;
            }

            else return false;
            
            

            
        }
        public void disable() {
            nameTB.Enabled = false;
            surnameTB.Enabled =false;
            idNumberTB.Enabled = false;
            genderCB.Enabled = false;
        }
        public void enable()
        {
            nameTB.Enabled = true;
            surnameTB.Enabled = true;
            idNumberTB.Enabled = true;
            genderCB.Enabled = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //WE Need to Validate the textBoxes
            if (validateCustomer()) { }
            else
            {
                //  public Customer(            string name,                string cNum,                 string surname,              long IDnumber,                  string email,               long cell,                  long tel,                  string address,                  string gender) {
                Customer newCustomer = new Customer(nameTB.Text.ToString(), cNumberTB.Text.ToString(), surnameTB.Text.ToString(), Convert.ToInt64(idNumberTB.Text), emailTB.Text.ToString(), Convert.ToInt64(cellTB.Text), Convert.ToInt64(telTB.Text), countryTB.Text.ToString(), genderCB.SelectedItem.ToString());
                if (img != null)
                {
                    Image imgfinal = ResizeImage(img, new Size(80, 80));                   
                    imgfinal.Save(executionPath + @"\CustomerImages\" + cNumberTB.Text.ToString() + ".bmp");
                }
               
                InserNewCustomer(newCustomer);
                if (sqlCommandAB.message != null)
                {
                   
                    MessageBox.Show(sqlCommandAB.message);
                }
                else
                {
                    DialogResult dr = MessageBox.Show("New Customer " + newCustomer.name + " has been added to the Database");
                    if (dr == DialogResult.OK)
                    {
                        dataGridView1.Rows.Clear();
                        Customer[] customer;
                        getCatData(out customer);
                        loadData();
                        Clear();
                    }
                }
            }
        }
        public void getCatData(out Customer[] Cast_Array)
        {   sqlCommandAB brandSql = new sqlCommandAB();
            DataTable DT = brandSql.QueryDT("SELECT * FROM Customer");
            Cast_Array = null;
            if (sqlCommandAB.message != null)
            {
                MessageBox.Show(sqlCommandAB.message);
            }
            else
            {
                int rowCount = DT.Rows.Count;
                Cast_Array = new Customer[rowCount];
                string[] row;
                for (int j = 0; j < rowCount; j++)
                {
                    row = new string[] { (string)DT.Rows[j]["Customer_Number"], (string)DT.Rows[j]["Customer_Name"], (string)DT.Rows[j]["Customer_Surname"], DT.Rows[j]["Customer_Cell"].ToString(), (string)DT.Rows[j]["Customer_Address"] };
                    dataGridView1.Rows.Add(row);
                    loadData();
                }
            }
        }
        public decimal  getCreditA(string a) {
            sqlCommandAB getCr = new sqlCommandAB();
            string cr = getCr.getSingleString("SELECT Customer_CreditLimit from Customer WHERE Customer_Number ='"+a+"'");
            decimal credit;
            try
            {
                Decimal.TryParse(cr, out credit);
            }
            catch (Exception) {
                return 0;
            }
            return credit;
        }
        public decimal getCreditB(string a)
        {
            sqlCommandAB getCr = new sqlCommandAB();
            string cr = getCr.getSingleString("SELECT Customer_CreditBalance from Customer WHERE Customer_Number ='" + a + "'");
            decimal credit;
            try
            {
                Decimal.TryParse(cr, out credit);
            }
            catch (Exception)
            {
                return 0;
            }
            return credit;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Clear();
            disable();
            creditControl.Visible = true;
            saveBTN.Enabled = false;
            
            
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

                string a = Convert.ToString(selectedRow.Cells["custNumber"].Value);
                customer_Number = a;
               textBox2.Text =  getCreditA(a).ToString();
               balanceTB.Text = getCreditB(a).ToString();
               
                sqlCommandAB customerSql = new sqlCommandAB();
                DataTable DT = customerSql.QueryDT("SELECT * FROM Customer where Customer_Number ='"+a+"' " );
                if (sqlCommandAB.message != null)
                {
                    MessageBox.Show(sqlCommandAB.message);
                }
                else
                {
                    populate(DT);
                    deleteBTN.Enabled = true;

                }
               // MessageBox.Show((string)DT.Rows[0]["Customer_Surname"]);
                
            
           
            
        }
        public void populate( DataTable me) {
            cNumberTB.Text = me.Rows[0]["Customer_Number"].ToString();
            nameTB.Text = (string)me.Rows[0]["Customer_Name"];
            surnameTB.Text = (string)me.Rows[0]["Customer_Surname"];
            idNumberTB.Text = me.Rows[0]["Customer_IDNumber"].ToString();
            emailTB.Text = (string)me.Rows[0]["Customer_Email"];
            cellTB.Text ="0"+me.Rows[0]["Customer_Cell"].ToString();
            telTB.Text ="0"+ me.Rows[0]["Customer_Tel"].ToString();
            countryTB.Text = (string)me.Rows[0]["Customer_Address"];
            genderCB.Text  = (string)me.Rows[0]["Customer_Gender"];
            UpdateBTN.Enabled = true;
            try {
                customerIMG.Image = Image.FromFile(executionPath + @"\CustomerImages\" + me.Rows[0]["Customer_Number"].ToString() + ".bmp");
            }
            catch (Exception) {
             
                
            }
        }
        public void Clear()
        {
            img = null;
            textBox3.Text = "";
            cNumberTB.Text = makeCustomerNumber();
            customerIMG.Image = Properties.Resources.defaultImage1;
            nameTB.Text=surnameTB.Text=idNumberTB.Text=emailTB.Text=cellTB.Text=telTB.Text=countryTB.Text = "";
            genderCB.SelectedIndex = -1;
            enable();
            groupBox2.Visible = false;
            creditControl.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Clear();
            UpdateBTN.Enabled = false;
            deleteBTN.Enabled = false;
            saveBTN.Enabled = true;
        }

        private void UpdateBTN_Click(object sender, EventArgs e)
        {
            if (validateCustomer()) { }
            else { 
                //Codes to update Customer
                Customer newCustomer = new Customer(nameTB.Text.ToString(), cNumberTB.Text.ToString(), surnameTB.Text.ToString(), Convert.ToInt64(idNumberTB.Text), emailTB.Text.ToString(), Convert.ToInt32(cellTB.Text), Convert.ToInt32(telTB.Text), countryTB.Text.ToString(), genderCB.SelectedItem.ToString());
                UpdateNewCustomer(newCustomer);
                if (sqlCommandAB.message != null)
                {
                    MessageBox.Show(sqlCommandAB.message);
                }
                else
                {
                    MessageBox.Show("Update Succefful");
                    Clear();
                    enable();
                }
            }
        }
        public void UpdateNewCustomer(Customer customer)
        {
            sqlCommandAB brandSql = new sqlCommandAB();
            brandSql.updateDB("UPDATE Customer set Customer_Cell = '" + customer.cell + "',Customer_Tel='" + customer.tel + "',Customer_Email ='" + customer.email + "', Customer_Address='"+ customer.address+"' WHERE (Customer_Number = '" + customer.username + "')");
            

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

            string a = Convert.ToString(selectedRow.Cells["custNumber"].Value);
            sqlCommandAB customerSql = new sqlCommandAB();
            DialogResult dr = MessageBox.Show("Are you sure you want to Delete " + nameTB.Text.ToString() + "Permanently?", "Confim Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dr == DialogResult.Yes)
            {
                if (sqlCommandAB.message != null)
                {
                    MessageBox.Show(sqlCommandAB.message);
                }
                else
                {
                    DataTable DT = customerSql.QueryDT("DELETE  FROM Customer where Customer_Number ='" + a + "' ");
                    dataGridView1.Rows.Clear();
                    Customer[] customer;
                    getCatData(out customer);
                    loadData();
                    Clear();
                }
            }
            
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            //this.Dispose();
        }

        private void browseBTN_Click(object sender, EventArgs e)
        {
            try
            {
                loader.Filter = "Choose Image(AllFiles)|*.jpg;*.png;*.gif ";
                if (loader.ShowDialog() == DialogResult.OK)
                {
                    img = Image.FromFile(loader.FileName);
                    customerIMG.Image = img;
                }
            }
            catch
            {

                MessageBox.Show("Error! Please check if the image is in a correct format and if it exist.");

            }
        }
  
       //Resizing the new Image
        private Image ResizeImage(Image img, Size newSize)
        {
            Image newImg = new Bitmap(newSize.Width, newSize.Width);
            using (Graphics art = Graphics.FromImage((Bitmap)newImg))
            {

                art.DrawImage(img, new Rectangle(Point.Empty, newSize));
            }

            return newImg;
        }

        private void Customer_Load(object sender, EventArgs e)
        {
            dataGridView1.RowTemplate.Height = 80;
            executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            executionPath = executionPath.Substring(6);
        }
        public void loadData()
        {
            executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            executionPath = executionPath.Substring(6);


            int numRow = dataGridView1.Rows.Count;
            for (int j = 0; j < numRow; j++)
            {
                try
                {
                    dataGridView1.Rows[j].Cells["customerPhoto"].Value = Image.FromFile(executionPath + @"\CustomerImages\" + dataGridView1.Rows[j].Cells["custNumber"].Value.ToString() + ".bmp");
                }
                catch (Exception)
                {

                    dataGridView1.Rows[j].Cells["customerPhoto"].Value = Properties.Resources.defaultImage.GetThumbnailImage(80, 80, null, new IntPtr());
                }
            }
        }

        private void searchBTN_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
        }
        public void getCustSearch(out Customer[] Cast_Array)
        {
            dataGridView1.Rows.Clear();
            string allQ = (string)textBox1.Text;
            sqlCommandAB brandSql = new sqlCommandAB();
            DataTable DT = brandSql.QueryDT("SELECT * FROM Customer WHERE ((Customer_IDNumber LIKE '" + allQ + "%') OR (Customer_Number LIKE '%" + allQ + "%')) OR (Customer_Name LIKE '%" + allQ + "%')");
            Cast_Array = null;
            if (sqlCommandAB.message != null)
            {
                MessageBox.Show(sqlCommandAB.message);
            }
            else
            {
                int rowCount = DT.Rows.Count;
                Cast_Array = new Customer[rowCount];
                string[] row;
                for (int j = 0; j < rowCount; j++)
                {
                    row = new string[] { (string)DT.Rows[j]["Customer_Number"], (string)DT.Rows[j]["Customer_Name"], (string)DT.Rows[j]["Customer_Surname"], DT.Rows[j]["Customer_Cell"].ToString(), (string)DT.Rows[j]["Customer_Address"] };
                    dataGridView1.Rows.Add(row);
                    loadData();
                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
           Customer[] customers;
           getCustSearch(out customers);

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           
        }
        public void setCreditControl(string a) {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Input a credit limit, Please try Again");
            }
            else if(!textBox3.Text.All(char.IsDigit)){
                MessageBox.Show("Invalid Credit limit, Please try Again");
            }
            else
            {
                double newCredit = Convert.ToDouble(textBox3.Text);
                if (Convert.ToDouble(balanceTB.Text) > Convert.ToDouble(textBox3.Text))
                {
                   double overCredit = Convert.ToDouble(balanceTB.Text);
                    DialogResult dr = MessageBox.Show("Please Confirm the new limit, " + overCredit, "Warning Customer credit limit is more than you are setting", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    if (dr.Equals(DialogResult.Yes))
                    {
                        updateCredit(overCredit,a);
                        
                    }
                    else if(dr.Equals(DialogResult.No)){

                        updateCredit(newCredit,a);
                    }
                    else
                    {
                        //Since Employee pressed cancel, they must go to where they were...
                    }
                }
                else {
                    updateCredit(newCredit, a);
                }
         
            }
        }
        private void updateCredit(double credit, string b) {
            sqlCommandAB ab = new sqlCommandAB();
            ab.updateDB("UPDATE Customer set Customer_CreditLimit ='" +credit+ "' WHERE Customer_Number = '" +b+ "'");
            if (sqlCommandAB.message != null)
            {
                MessageBox.Show("Credit Limit failed to update Customer");
            }
            else
            {
                MessageBox.Show("Credit limit set to " + credit);
                Clear();
            }
            
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            setCreditControl(customer_Number);
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }
   }
}
