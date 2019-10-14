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
    public partial class Main : Form
    {
        //Company Details;;;;
        
      public static String  companyName = "Araia Investments";
      public static String companyAddress = "14 Birmingham Road, Willowton, Pietermaritzburg 3201";
      public  static String companyCell = "073 774 0879";
        /// <summary>
        /// /////////////////////////////////////////
        /// </summary>
        public static string staffName;
        public static string staffNumber;
        public static string staffTye;
        public static string staffUsername;
        public static string staffPassword;
        private string executionPath = null; 
        public Main()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            menuStrip1.BackColor = Color.PaleTurquoise;
            executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            executionPath = executionPath.Substring(6);       
        }
        private void loggedOut() {
            sqlCommandAB logout = new sqlCommandAB();
            int id = logout.getSingleValue("SELECT loginID FROM loginLog ORDER BY loginID DESC");
            logout.updateDB("UPDATE loginLog set loginEnd = getDate() WHERE loginID = '" + id + "'");
        
        }
        private void Main2_Load()
        {
            afterLogin();
        }
        public void beforeLogin()
        {
            menuStrip1.Enabled = false;
            POSBTN.Enabled = false;
            CustomerBTN.Enabled = false;

            EmployeeBTN.Enabled = false;
            oder1.Visible = false;
            OrdersBTN.Enabled = false;
            itemBTN.Enabled = false;
            StockINBTN.Enabled = false;
            ReportsBTN.Enabled = false;
            //button1.Enabled = false;
            //button1.Enabled = false;
        }
        public void afterLogin()
        {
            menuStrip1.Enabled = true;
            POSBTN.Enabled = true;
            CustomerBTN.Enabled = true;

            EmployeeBTN.Enabled = true;
          //  oder1.Visible = true;
            OrdersBTN.Enabled = true;
            itemBTN.Enabled = true;
            StockINBTN.Enabled = true;
            ReportsBTN.Enabled = true;
            //button1.Enabled = false;
            //button1.Enabled = false;
        }
        //delegate void activate(afterLogin){
        public void userLogin()
        {
            menuStrip1.Enabled = true;
            POSBTN.Enabled = true;
            CustomerBTN.Enabled = true;

            EmployeeBTN.Enabled = false;
         //   oder1.Visible = false;
            OrdersBTN.Enabled = false;
            itemBTN.Enabled = true;
            StockINBTN.Enabled = true;
            ReportsBTN.Enabled = false;
            //button1.Enabled = false;
            //button1.Enabled = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            oder1.Visible = false;
            employeeUC1.Visible = false;
            customer1.Visible = false;
            categoryBrand1.Visible = false;
            pos1.Visible = false;
            itemEntry1.Visible = false;
            reports1.Visible = false;
            customer1.Visible = true;
            rep1.Visible = false;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            customer1.Visible = false;
            reports1.Visible = false;
            customer1.Visible = false;
            categoryBrand1.Visible = false;
            pos1.Visible = false;
            itemEntry1.Visible = false;
            employeeUC1.Visible = true;
           // reports2.Visible = false;
            oder1.Visible = false;
            rep1.Visible = false;
        }
        private void login1_Load_1(object sender, EventArgs e)
        {
            // beforeLogin();

        }
        private void button5_Click(object sender, EventArgs e)
        {
            loggedOut();
            LoginProgram loginForm = new LoginProgram();
            loginForm.Show();
            this.Hide();
        }

        private void companyDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(companyName + " " + companyAddress + "\n Contact Number " + companyCell);          
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            customer1.Visible = false;
            employeeUC1.Visible = false;
            categoryBrand1.Visible = false;
            pos1.Visible = false;
            itemEntry1.Visible = false;
            reports1.Visible = false;
           // reports2.Visible = false;
            oder1.Visible = false;
        }

        private void itemBTN_Click(object sender, EventArgs e)
        {

            oder1.Visible = false;
            customer1.Visible = false;
            employeeUC1.Visible = false;
            pos1.Visible = false;
            itemEntry1.Visible = false;
            reports1.Visible = false;
            categoryBrand1.Visible = true;
            rep1.Visible = false;
          //  reports2.Visible = false;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e) { loggedOut(); Application.Exit(); }

        private void button1_Click(object sender, EventArgs e)
        {
            oder1.Visible = false;
            customer1.Visible = false;
            employeeUC1.Visible = false;
            categoryBrand1.Visible = false;
            itemEntry1.Visible = false;
            pos1.Visible = false;
            reports1.Visible = true;
           // reports2.Visible = false;
            rep1.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            customer1.Visible = false;
            employeeUC1.Visible = false;
            categoryBrand1.Visible = false;
            itemEntry1.Visible = true;
            pos1.Visible = false;
            reports1.Visible = false;
           // reports2.Visible = false;
            oder1.Visible = false;
            rep1.Visible = false;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            using (LoginProgram login = new LoginProgram())
            {
                login.loggedIn += new EventHandler(login_loggedIn);
               staffName = login.getSName();
               staffNumber = login.getSNumber();
               staffPassword = login.getSPassword();
               staffTye = login.getSType();
               staffUsername = login.getSUsername();
                
            }
            LsName.Text = staffTye;
            label1.Text = "Name: " + staffName;
            label2.Text = "Stuff Number: " + staffNumber;
            try
            {
                userImage.Image = Image.FromFile(executionPath + @"\StaffImages\" + staffNumber + ".bmp");

            }
            catch (Exception)
            {
                userImage.Visible = false;
            }

           switch (staffTye) {
             case "Admin":
                   afterLogin();
                   break;
               case "NoBody":
                   beforeLogin();
                   MessageBox.Show("Dear " + staffName + " Please login again after the manager gives you privelages to use the system");
                   break;
               case "User":
                   userLogin();
                   break;
             default: 
                beforeLogin();
                MessageBox.Show("User cannot be identified, If you are trying to hack the System... \nSorry There isn't much you can do here");
                break;            
            }
        }

        void login_loggedIn(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OrdersBTN_Click(object sender, EventArgs e)
        {
            employeeUC1.Visible = false;
            customer1.Visible = false;
            categoryBrand1.Visible = false;
            pos1.Visible = false;
            itemEntry1.Visible = false;
            reports1.Visible = false;
            customer1.Visible = false;
         //   reports2.Visible = false;
            oder1.Visible = true;
            rep1.Visible = false;
        }

        private void ReportsBTN_Click(object sender, EventArgs e)
        {
            employeeUC1.Visible = false;
            customer1.Visible = false;
            categoryBrand1.Visible = false;
            pos1.Visible = false;
            itemEntry1.Visible = false;
            reports1.Visible = false;
            customer1.Visible = false;       
            oder1.Visible = false;
            rep1.Visible = true;
        }

        private void posForUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formforus = new PosForUs();
            try
            {
                formforus.Show();
                this.IsMdiContainer = true;
                formforus.Focus();
            }
            catch (Exception) {
                MessageBox.Show("Sorry System cannot Continue, Please Restart POS System");
            }
        }

        private void rep1_Load(object sender, EventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form form1 = new Form1();
            //form1.Show();
        }
    }
}