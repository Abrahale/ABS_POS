using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AGK_POS
{
  
    public partial class Staff : UserControl
    {
        private string name;
        private string surname;
        private string gender;
        private string email;
        private long IDnumber;
        private int cell;
        private float salary;
        private float wage;
        private string username;
        private string password;
        private string type;
        private string staffNumber;
        private string remarks;

        private OpenFileDialog loader = new OpenFileDialog();
        Image img;
        private string executionPath = null;
        public Staff()
        {
            InitializeComponent();
            executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            executionPath = executionPath.Substring(6);
            Staff[] staff;
            dataGridView1.RowTemplate.Height = 80;
            getCatData(out staff);
            sNumberTB.Text = makeStaffNumber();
        }
           public Staff(string name,string num, string surname, long IDnumber,
            string email, int cell, string remarks,string gender,string type) {
                
                this.name = name;
                this.surname = surname;
                this.email = email;
                this.type = type;
                this.cell = cell;                
                this.IDnumber = IDnumber;
                this.gender = gender;
                this.username = makeUsername(name, surname);
                this.password = makePassword(name, surname);
                this.salary = 0;
                this.wage = 0;
                this.staffNumber = num;
                this.remarks = remarks;
            
            
        }
           public string makeStaffNumber()
           {

               Random random = new Random();
               int username = random.Next(1111, 999999999);
               return username.ToString();

           }

           public void getCatData(out Staff[] staff_Array)
           {
               sqlCommandAB brandSql = new sqlCommandAB();
               DataTable DT = brandSql.QueryDT("SELECT * FROM staff");
               int rowCount = DT.Rows.Count;
               staff_Array = new Staff[rowCount];
               string[] row;
               for (int j = 0; j < rowCount; j++)
               {
                   row = new string[] { (string)DT.Rows[j]["staff_Number"], (string)DT.Rows[j]["staff_Name"], (string)DT.Rows[j]["staff_Surname"], DT.Rows[j]["staff_Cell"].ToString(), (string)DT.Rows[j]["staff_Type"] };
                   dataGridView1.Rows.Add(row);
                   loadData();
               }
           }
           public void Clear() {
               groupBox1.Visible = false;
               passwordTB.Text = usernameTB.Text = "";
               sNumberTB.Text = makeStaffNumber();
               img = null;
               staffIMG.Image = Properties.Resources.defaultImage;
               username = makeStaffNumber();
               nameTB.Text = surnameTB.Text = idNumberTB.Text = emailTB.Text = cellTB.Text = remarksTB.Text = "";
               genderCB.SelectedIndex = -1;
               userTypeCB.SelectedIndex = -1;
               saveBTN.Enabled = true;
               UpdateBTN.Enabled = false;
               deleteBTN.Enabled = false;
            
           }
           public string makeUsername(string name, string surname)
           {
               string username = "";
               username += name[0] + surname[0];

               return username;

           }
           public string makePassword(string name, string surname)
           {
               string password = "";
               password += name[0] + surname[0] + name[1] + surname[1] + "member";
               return password;


           }
        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {//Please Bonga if you see I didn't implement the validation, Please validate it... I don't have time right now.
            //I have thought of haveing a method that validates, and retruns a value....
            if (validateStaff()) { }
            else
            {
                if (img != null)
                {
                    Image imgfinal = ResizeImage(img, new Size(80, 80));
                    // MessageBox.Show(executionPath);
                    imgfinal.Save(executionPath + @"\StaffImages\" + sNumberTB.Text.ToString() + ".bmp");
                }
                Staff newCustomer = new Staff(nameTB.Text.ToString(), sNumberTB.Text.ToString(), surnameTB.Text.ToString(), Convert.ToInt64(idNumberTB.Text), emailTB.Text.ToString(), Convert.ToInt32(cellTB.Text), remarksTB.Text.ToString(), genderCB.SelectedItem.ToString(), userTypeCB.SelectedItem.ToString());
                // Customer newCustomer = new Customer("Abrahale", "Kiros", 9412116143262, "Male", 0848398778, 0848398778, "Eritrea", "PMB");
                InserNewCustomer(newCustomer);
                DialogResult dr = MessageBox.Show("New Staff Member  \n" + newCustomer.name + " \n has been added to the Database");
                if (dr == DialogResult.OK)
                {
                    dataGridView1.Rows.Clear();
                    Staff[] staff;
                    getCatData(out staff);
                    Clear();
                }
            }
        }
        public Boolean validateStaff() { 
           if(nameTB.Text == "" ){
               MessageBox.Show("Please Enter Staff name");
               nameTB.Focus();
               return true;
           }
           else if (surnameTB.Text == "") {
               MessageBox.Show("Please Enter Staff Surname");
               surnameTB.Focus();
               return true;
           }
           else if (!idNumberTB.Text.All(char.IsDigit) || idNumberTB.Text.Length != 13) {
               MessageBox.Show("ID Number is Invalid, Please Enter South African ID number");
               idNumberTB.Text = "";
               idNumberTB.Focus();
               return true;
           }
           else if (idNumberTB.Text == "") {
               MessageBox.Show("Please Enter South African ID number");
               idNumberTB.Focus();
               return true;
           }

           else if (!cellTB.Text.All(char.IsDigit) || cellTB.Text.Length != 10) {
               MessageBox.Show("Please Enter valid Cellphone Number");
               cellTB.Text = "";
               cellTB.Focus();
               return true;
           }
           else if (genderCB.Text == "") {
               MessageBox.Show("Please Select a Gender");
               genderCB.Focus();
               return true;
           }
           else if (userTypeCB.Text == "")
           {
               MessageBox.Show("Please Select Staff Type");
               userTypeCB.Focus();
               return true;
           }
           else {
               return false;
           }
        
        }
        public void  InserNewCustomer(Staff customer)
        {
            sqlCommandAB brandSql = new sqlCommandAB();
            DataTable dt = brandSql.QueryDT("SELECT * FROM Staff");
            DataRow dr = dt.NewRow();
            dr["staff_Number"] = customer.staffNumber;
            dr["staff_Name"] = customer.name;
            dr["staff_Surname"] = customer.surname;
            dr["staff_IDNumber"] = customer.IDnumber;
            dr["staff_Gender"] = customer.gender;
            dr["staff_Cell"] = customer.cell;            
            dr["staff_Email"] = customer.email;  
            dr["staff_Type"] = customer.type; 
            dr["staff_Username"] = customer.username;
            dr["staff_Password"] = customer.password;
            dr["staff_Remarks"] = customer.remarks;         
           
            dt.Rows.Add(dr);
            brandSql.UpDate(dt);

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Clear();
            groupBox2.Visible = false;
            enable();
            
        }
        private DataTable getUserLoginDetails(string a){
            sqlCommandAB customerSql = new sqlCommandAB();
            DataTable DT = customerSql.QueryDT("SELECT staff_Number, staff_Username, staff_Password FROM Staff where staff_Number ='" + a + "' ");
            return DT;
        
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            disabled();
            groupBox1.Visible = true;
            Clear();

            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

            string a = Convert.ToString(selectedRow.Cells["staff_Number"].Value);
            sqlCommandAB customerSql = new sqlCommandAB();
            DataTable DT = customerSql.QueryDT("SELECT * FROM Staff where staff_Number ='" + a + "' ");
            populate(DT);
            populateLoginDetails(getUserLoginDetails(a));
            groupBox1.Visible = true;
            deleteBTN.Enabled = true;
        }
        public void populateLoginDetails(DataTable me) {
            usernameTB.Text = (string)me.Rows[0]["staff_Username"];
            passwordTB.Text = (string)me.Rows[0]["staff_Password"];
            confirmpasswordTB.Visible = false;
            confimPasswordLabel.Visible = false;
            updateLoginDetails.Visible = false;
        }
        public void populate(DataTable me)
        {
          
            sNumberTB.Text = (string)me.Rows[0]["staff_Number"];
            nameTB.Text = (string)me.Rows[0]["staff_Name"];
            surnameTB.Text = (string)me.Rows[0]["staff_Surname"];
            idNumberTB.Text = me.Rows[0]["staff_IDNumber"].ToString();
            emailTB.Text = (string)me.Rows[0]["staff_Email"];
            cellTB.Text ="0"+ me.Rows[0]["staff_Cell"].ToString();
            remarksTB.Text = (string)me.Rows[0]["staff_Remarks"];
            genderCB.SelectedItem = (string)me.Rows[0]["staff_Gender"];
            userTypeCB.SelectedItem = (string)me.Rows[0]["staff_Type"];
            saveBTN.Enabled = false;
            UpdateBTN.Enabled = true;
            try
            {
                staffIMG.Image = Image.FromFile(executionPath + @"\StaffImages\" + me.Rows[0]["staff_Number"].ToString() + ".bmp");
            }
            catch (Exception)
            {
               
            }
        }

        private void deleteBTN_Click(object sender, EventArgs e)
        {

            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

            string a = Convert.ToString(selectedRow.Cells["staff_Number"].Value);
            sqlCommandAB customerSql = new sqlCommandAB();
            DialogResult dr = MessageBox.Show("Are you sure you want to Delete " + nameTB.Text.ToString() + "Permanently?", "Confim Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dr == DialogResult.Yes)
            {
                DataTable DT = customerSql.QueryDT("DELETE  FROM Staff where staff_Number ='" + a + "' ");
                dataGridView1.Rows.Clear();
                Staff[] staff;
                getCatData(out staff);
                Clear();
            }

        }

        private void browseBTN_Click(object sender, EventArgs e)
        {

            try
            {
                loader.Filter = "Choose Image(AllFiles)|*.jpg;*.png;*.gif ";
                if (loader.ShowDialog() == DialogResult.OK)
                {
                    img = Image.FromFile(loader.FileName);
                    staffIMG.Image = img;
                }
            }
            catch
            {

                MessageBox.Show("Error! Please check if the image is in a correct format and if it exist.");

            }
        }
        private Image ResizeImage(Image img, Size newSize)
        {
            Image newImg = new Bitmap(newSize.Width, newSize.Width);
            using (Graphics art = Graphics.FromImage((Bitmap)newImg))
            {

                art.DrawImage(img, new Rectangle(Point.Empty, newSize));
            }

            return newImg;
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
                    dataGridView1.Rows[j].Cells["customerPhoto"].Value = Image.FromFile(executionPath + @"\StaffImages\" + dataGridView1.Rows[j].Cells["staff_Number"].Value.ToString() + ".bmp");
                }
                catch (Exception)
                {
                    dataGridView1.Rows[j].Cells["customerPhoto"].Value = Properties.Resources.defaultImage.GetThumbnailImage(80, 80, null, new IntPtr());
                }
            }
        }
   
        private void UpdateBTN_Click(object sender, EventArgs e)
        {
            if (validateStaff()) { }
            else
            {
                Staff newCustomer = new Staff(nameTB.Text.ToString(), sNumberTB.Text.ToString(), surnameTB.Text.ToString(), Convert.ToInt64(idNumberTB.Text), emailTB.Text.ToString(), Convert.ToInt32(cellTB.Text), remarksTB.Text.ToString(), genderCB.SelectedItem.ToString(), userTypeCB.SelectedItem.ToString());
                UpdateStaff(newCustomer);
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
        public void UpdateStaff(Staff customer)
        {
            sqlCommandAB brandSql = new sqlCommandAB();
            brandSql.updateDB("UPDATE Staff set staff_Cell = '" + customer.cell + "',staff_Remarks='" + customer.remarks + "',staff_Email ='" + customer.email + "', staff_Type='" + customer.type + "' WHERE (staff_Number = '" + customer.staffNumber + "')");
            

        }
        public void disabled() { 
            
            nameTB.Enabled = false; surnameTB.Enabled = false; 
            idNumberTB.Enabled = false;
            genderCB.Enabled = false;
           

        }
        public void enable() {
            //After Update
            nameTB.Enabled = true; surnameTB.Enabled = true;
            idNumberTB.Enabled = true;
            genderCB.Enabled = true;
        }


        public void getStaffData(out Staff[] staff_Array)
        {
            dataGridView1.Rows.Clear();
            string allQ = (string)textBox1.Text;
            sqlCommandAB brandSql = new sqlCommandAB();
            DataTable DT = brandSql.QueryDT("SELECT * FROM staff WHERE ((staff_IDNumber LIKE '" + allQ + "%') OR (staff_Number LIKE '%" + allQ + "%')) OR (staff_Name LIKE '%" + allQ + "%')");
            int rowCount = DT.Rows.Count;
            staff_Array = new Staff[rowCount];
            string[] row;
            for (int j = 0; j < rowCount; j++)
            {
                row = new string[] { (string)DT.Rows[j]["staff_Number"], (string)DT.Rows[j]["staff_Name"], (string)DT.Rows[j]["staff_Surname"], DT.Rows[j]["staff_Cell"].ToString(), (string)DT.Rows[j]["staff_Type"] };
                dataGridView1.Rows.Add(row);
                loadData();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Staff[] staff_Array;
            getStaffData(out  staff_Array);
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
        }    

    }
}
