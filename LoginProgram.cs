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

    public partial class LoginProgram : Form
    {
        static String Defaultusername = "SMG"; 
        static String Defaultpassword = "Dube";
        //This is the class TO create customer:    
        
        private static string sNumber;
        public string getSNumber() { return sNumber; }
        private static string sUsername;
        public string getSUsername() { return sUsername; }
        private static string sPassword;
        public string getSPassword() { return sPassword; }
        private static string sName;
        public string getSName() {
            return sName;
        }
        public static string sType;
        public string getSType() { return sType; }
       
        public LoginProgram()
        {
            InitializeComponent();
        }        
        private void ClearBTN_Click_1(object sender, EventArgs e)
        {
            UserNameIP.Text = "";
            PasswordIP.Text = "";
            ClearBTN.Visible = false;
            DefaultBTN.Visible = true;
            PasswordIP.UseSystemPasswordChar = true;
            pbPasswordHide.Visible = false;
            pbPassowrdShow.Visible = true;
        }
        private void DefaultBTN_Click(object sender, EventArgs e)
        {
            UserNameIP.Text = Defaultusername;
            PasswordIP.Text = Defaultpassword;
            ClearBTN.Visible = true;
            DefaultBTN.Visible = false;
        }
        private void pbPasswordHide_Click_1(object sender, EventArgs e)
        {
            PasswordIP.UseSystemPasswordChar = true;
            pbPasswordHide.Visible = false;
            pbPassowrdShow.Visible = true;
        }
        private void pbPassowrdShow_Click_1(object sender, EventArgs e)
        {
            PasswordIP.UseSystemPasswordChar = false;
            pbPasswordHide.Visible = true;
            pbPassowrdShow.Visible = false;
        }
        public event EventHandler loggedIn; 
        private void LoginBTN_Click(object sender, EventArgs e)
        {
            validateUser(UserNameIP.Text);
            if (sPassword == null) {
                LErrorMsg.Text = "User name not found in the database";
                UserNameIP.Text = "";
                PasswordIP.Text = "";
                UserNameIP.Focus();

            }
            else if (sPassword.Equals(PasswordIP.Text))
            {
                insertUser();
                Form mainPage = new Main();
                mainPage.Show();
                this.Hide();

            }
            else
            {
                LErrorMsg.Text = "Your Password is incorrect";
                PasswordIP.Text = "";
                PasswordIP.Focus();
            }
            if (loggedIn != null) { 
                loggedIn.Invoke(this,new EventArgs());
            }
                
        }
        public void insertUser() {
            sqlCommandAB iu = new sqlCommandAB();
            DataTable dt = iu.QueryDT("SELECT * FROM loginLog");
            DataRow dr = dt.NewRow();
                 dr["staff_Number"] = sNumber;
                 dr["staff_Name"] = sName;                
            dt.Rows.Add(dr);          
            iu.UpDate(dt);              
        }
        private void changeDC(object sender, EventArgs e)
        {
            if (PasswordIP.Text.Length > 0) {
                LErrorMsg.Text = ""; }
            else if(UserNameIP.Text == "")   
            {               
                ClearBTN.Visible = false;
                DefaultBTN.Visible = true;
                PasswordIP.Text = "";
                PasswordIP.UseSystemPasswordChar = true;
                pbPasswordHide.Visible = false;
                pbPassowrdShow.Visible = true;
            }

            else
            {
                ClearBTN.Visible = true;
                DefaultBTN.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void getStaff(object sender, EventArgs e)
        {
            
    
        }
        public void validateUser(string inUserName) {
            sqlCommandAB lg = new sqlCommandAB();
            DataTable dt = lg.QueryDT("SELECT staff_Number, staff_Type, staff_Username,staff_Password,staff_Name FROM Staff WHERE (staff_Number = '" + inUserName + "' OR staff_Username ='" + inUserName + "')"); 
                int count = dt.Rows.Count;
                if(count >=1){
                    sPassword = dt.Rows[0]["staff_Password"].ToString();
                    sNumber = dt.Rows[0]["staff_Number"].ToString();
                    sUsername = dt.Rows[0]["staff_Username"].ToString();
                    sName = dt.Rows[0]["staff_Name"].ToString();
                    sType = dt.Rows[0]["staff_Type"].ToString();                 
                 
               }
           }

        private void changeDC1(object sender, EventArgs e)
        {
             if (UserNameIP.Text.Length > 0) {
                LErrorMsg.Text = ""; }
            else if(UserNameIP.Text == "")   
            {               
                ClearBTN.Visible = false;
                DefaultBTN.Visible = true;
                PasswordIP.Text = "";
                PasswordIP.UseSystemPasswordChar = true;
                pbPasswordHide.Visible = false;
                pbPassowrdShow.Visible = true;
            }

            else
            {
                ClearBTN.Visible = true;
                DefaultBTN.Visible = false;
            }
        }
    }
}
