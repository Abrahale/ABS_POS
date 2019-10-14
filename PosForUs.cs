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
    public partial class PosForUs : Form
    {
      //  private static String branchName = "High Street No4 Shop";
      //  private static String Address = "Bergville High Street, 3350";
      //  private static String Contact = "0848398778";
      //  private static String businessType = "Retailers";
        private static string executionPath;
        private static List<System.Windows.Forms.Button> btns = new List<System.Windows.Forms.Button>();
        private static List<System.Windows.Forms.Button> catBtns = new List<System.Windows.Forms.Button>();
        private static List<System.Windows.Forms.Button> brandsBtns = new List<System.Windows.Forms.Button>();
        private static List<Cart> cart = new List<Cart>();
        public static String pName;
        public static String pCode;
        public static Decimal pSellingPrice;
        public static int pQty;
        public static int itemQty = 1;
        public static Decimal profit;
        public static Decimal pAmount;
        private static Decimal paidAmount;
        private static Decimal change;
        private static Decimal totalCost;
        private static Decimal costPrice;
        private static Decimal totalProfit;

         sqlCommandAB products = new sqlCommandAB();
       
        static int counter = 0;
        public PosForUs()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            executionPath = executionPath.Substring(6);

            productBtns();
            brandsBtnsView();
          //  AddButtons(listView1.Height, listView1.Width, dtCat);
           
        }
        public void brandsBtnsView() {
            DataTable dtBrand = products.QueryDT("SELECT * FROM brands");
            AddButtons(listView1.Height, listView1.Width, dtBrand);
        }
        public void productBtns() {
            DataTable DT = products.QueryDT("SELECT * FROM Product");
            //  DataTable dtCat = products.QueryDT("SELECT * FROM Categories");
            AddButtons(DT.Rows.Count, listView2.Height, listView2.Width, DT);
        }
        public void productBtns(string query)
        {
            DataTable DT = products.QueryDT(query);
            //  DataTable dtCat = products.QueryDT("SELECT * FROM Categories");
            AddButtons(DT.Rows.Count, listView2.Height, listView2.Width, DT);
        }
        private static void getProductDetails(String id) {
            sqlCommandAB getPro = new sqlCommandAB();
            DataTable DT = getPro.QueryDT("SELECT * FROM Product WHERE pCode = '"+id+"'");
            if (DT.Rows.Count > 0) {
                pName = (string)DT.Rows[0]["pName"];
                pCode = (string)DT.Rows[0]["pCode"];
                pSellingPrice = Convert.ToDecimal(DT.Rows[0]["pSellingPrice"]);
                pQty = Convert.ToInt32(DT.Rows[0]["pQty"]);
                costPrice = Convert.ToDecimal(DT.Rows[0]["pCostPrice"]);
                profit = (pSellingPrice-costPrice);
            }
           
        }
        private void PosForUs_Load(object sender, EventArgs e)
        {
            listBox1.Text = "";
        }
        private void createBtns(int width, int height, string code,int xP, int yP,string pName) {
            System.Windows.Forms.Button btn = new System.Windows.Forms.Button();
            btn.Tag = code;
            btn.Width = width;
            btn.Height = height;
            btn.Left = xP;
            btn.Top = yP;
            btn.Text = pName;
            btn.ImageAlign = ContentAlignment.TopCenter;
            btn.TextAlign = ContentAlignment.BottomCenter;
            btn.TextImageRelation = TextImageRelation.Overlay;
            btn.Click += new System.EventHandler(ClickButton);
            try
            {
                btn.Image = Image.FromFile(executionPath + @"\ItemImages\" + code + ".bmp");
            }
            catch (Exception)
            {

            }
            btns.Add(btn);
            
        }
        private void createBtns(int width, int height, string code,string pName)
        {
            System.Windows.Forms.Button btn = new System.Windows.Forms.Button();
            btn.Tag = code;
            btn.Width = width;
            btn.Height = height;
            btn.Text = pName;
            btn.ImageAlign = ContentAlignment.TopCenter;
            btn.TextAlign = ContentAlignment.BottomCenter;
            btn.TextImageRelation = TextImageRelation.Overlay;
            btn.Click += new System.EventHandler(ClickButtonCat);
            btn.Dock = DockStyle.Top;
            catBtns.Add(btn);
        }
        private void AddButtons(int w, int h, DataTable d)
        {
            int len = d.Rows.Count;

            
            for (int n = 0; n < len; n++)
            {
                createBtns(30, 30, d.Rows[n]["categoryName"].ToString(),d.Rows[n]["brandName"].ToString());
            }
            foreach (System.Windows.Forms.Button b in catBtns)
            {
                listView1.Controls.Add(b);
            }
        }
        private void AddButtons(int nn, int w, int h,DataTable p)
        {
            int xPos = 0;
            int yPos = 0;            
            System.Windows.Forms.Button[] btnArray = new System.Windows.Forms.Button[nn];
            System.Windows.Forms.Panel[] panelArray = new System.Windows.Forms.Panel[nn];
                
       
            for (int i = 0; i < nn; i++)
            { 
                btnArray[i] = new System.Windows.Forms.Button();                              
            }
            int n = 0;
            while (n < nn)
            {
                createBtns(80, 100, p.Rows[n]["pCode"].ToString(), xPos, yPos, p.Rows[n]["pName"].ToString());
                xPos = xPos + 81; // Left of next button
                n++;
                //btnArray[n].Tag = p.Rows[n]["pCode"].ToString(); // Tag of button 
                //btnArray[n].Width = 80; // Width of button 
                //btnArray[n].Height = 100; // Height of button 
                //btnArray[n].Left = xPos;
                //btnArray[n].Top = yPos;                
                //listView2.Controls.Add(btnArray[n]); // Let panel hold the Buttons                    
               //btnArray[n].Text = p.Rows[n]["pName"].ToString();
                //btnArray[n].ImageAlign = ContentAlignment.TopCenter;
                //btnArray[n].TextAlign = ContentAlignment.BottomCenter;
                //btnArray[n].TextImageRelation = TextImageRelation.Overlay;               
                //try
                //{
                //    btnArray[n].Image = Image.FromFile(executionPath + @"\ItemImages\" + p.Rows[n]["pCode"].ToString() + ".bmp");
                //}
                //catch (Exception)
                //{
                //}
               // btnArray[n].Click += new System.EventHandler(ClickButton);                
            }
            
            foreach(System.Windows.Forms.Button b in btns)
            {
                listView2.Controls.Add(b);
            }
            label1.Visible = true;            
        }
        public void ClickButton(Object sender, System.EventArgs e)
        { 
            Button btn = (Button)sender;
            getProductDetails(btn.Tag.ToString());
           // cart[counter] = new Cart(pName, pCode, pSellingPrice, pQty);                        
            cart.Add( new Cart(pName, pCode, pSellingPrice, pQty ,profit,costPrice,itemQty,(itemQty *pSellingPrice)));
            String item = "#" + (counter + 1) + " " + Cart.pName.PadRight(15 - Cart.pName.Length) + "\tR" + Cart.pSellingPrice.ToString().PadRight(2) + "\t"+Cart.qty.ToString()+"\t"+Cart.totalAmount.ToString();
            listBox1.Items.Add(item);
            pAmount = pAmount + Cart.pSellingPrice;
            totalProfit = totalProfit + Cart.profit;
            //listBox1.Items.Add(Cart.pName.PadRight(50 - Cart.pName.Length) + "R" + Cart.pSellingPrice.ToString());      
            totalAmountTB.Text = pAmount.ToString();
            counter++;            
        }
        public void ClickButtonCat(Object sender, System.EventArgs e)
        {
            Button btn = (Button)sender;
            getProductDetails(btn.Tag.ToString());
            productBtns("SELECT * FROM products WHERE categoryName'"+ btn.Tag.ToString()+"'");          
           
        }
        
        private void enterAmount(object sender, EventArgs e)
        {
            cashTB.Text = "";
        }
        private void insertSale() {
            sqlCommandAB saleQuery = new sqlCommandAB();
            DataTable dt = saleQuery.QueryDT("SELECT * FROM sale");
            DataRow dr = dt.NewRow();
            dr["invoiceNumber"] = "Abrahale Kiros";
            dr["qty"] = (counter+1);
            dr["total_profit"] = totalProfit;
            dr["amountDue"] = pAmount;
            dr["amountPayed"] = paidAmount;
            dr["total_cost"] = totalCost;
            dr["change"] = (paidAmount - pAmount);
            dr["tithe"] =(double)totalProfit * 0.10;
            dt.Rows.Add(dr);
            saleQuery.UpDate(dt);
        }
        private void PayBTN_Click(object sender, EventArgs e)
        {
            paidAmount = Convert.ToDecimal(cashTB.Text);
            change = (paidAmount - pAmount);
            insertSale();
            if (change >= (Decimal)0 && sqlCommandAB.message == null)
            {
                changeTB.Text = change.ToString();
               
            }
            else {
                MessageBox.Show(sqlCommandAB.message);
            }
        }

    }

}
