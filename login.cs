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
    public partial class login : UserControl
    {
        public static Boolean AccessGranted = false;
        static String username = "Abrahale";
        static String password = "Abrahale";
        public login()
        {
            InitializeComponent();
            
 
        }

        private void pbPassowrdShow_Click(object sender, EventArgs e)
        {
            PasswordIP.UseSystemPasswordChar = false; 
            pbPasswordHide.Visible = true;
            pbPassowrdShow.Visible = false;

        }
        public void isLogged(String user, String pass)
        {
            if (username.Equals(user) && password.Equals(pass))
            {
                Form mainPage = new Main();
                AccessGranted = true;

                this.Hide();
               // mainPage.BeginInvoke(
                
                
                //This is working but you need to work further....
                //How to implement it to work on the other things....

            }

            else {
                LErrorMsg.Text = "Login Failed Try Again";

            }
         
        }
        private void pbPasswordHide_Click(object sender, EventArgs e)
        {
            PasswordIP.UseSystemPasswordChar = true; 
            pbPasswordHide.Visible = false;
            pbPassowrdShow.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserNameIP.Text = username;
            PasswordIP.Text = password;
            ClearBTN.Visible = true;
            DefaultBTN.Visible = false;
        }

        private void ClearBTN_Click(object sender, EventArgs e)
        {
            UserNameIP.Text = "";
            PasswordIP.Text = "";
            ClearBTN.Visible = false;
            DefaultBTN.Visible = true;
            PasswordIP.UseSystemPasswordChar = true; 
            pbPasswordHide.Visible = false;
            pbPassowrdShow.Visible = true;
        }

        private void changeDC(object sender, EventArgs e)
        {
            ClearBTN.Visible = true;
            DefaultBTN.Visible = false;
        }

        private void LoginBTN_Click(object sender, EventArgs e)
        {
            isLogged(UserNameIP.Text, PasswordIP.Text);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

 



    }
}
