using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace AGK_POS
{
    public partial class MainPos : UserControl
    {
         String companyName = Main.companyName;
         String companyAddress = Main.companyAddress;
         String companyCell = Main.companyCell;
        private DataTable cart;
        private DataTable cart2;
        private DataTable cart3;
        private int itemPosition=0;
        private int AvaliableQuantity = 0;
        public static int dbQuantity = 0;
        
        private bool update = false;
        private string currentOrderNum;
        sqlCommandAB posSql = new sqlCommandAB();
        
        private static string executionPath = null;

        //for payment variables
        private static Decimal amountPaid;
        private static Decimal amountDue;
        private static Decimal creditLimit;
        private static Decimal TotalCredit;
        private static Decimal customerCreditBal;
        private static Decimal change;
        public static string orderNumber;
        public static string orderAmount;
        public static string orderQty;

        //One Database Quering variable
        sqlCommandAB queryAB = new sqlCommandAB();
        public MainPos()
        {
            InitializeComponent();
            currentOrderNum=makeInvoicNumber();
            DataTable data = posSql.QueryDT("Select * FROM OrderItem");
            cart = new DataTable();
            cart2 = new DataTable();
            cart3 = new DataTable();
            string[] columns = {"pName", "pCode", "pSellingPrice", "pQuantity", "pTotalAmount",};
            string[] columns2 = { "Order_Number", "pName", "pCode", "pSellingPrice", "pQuantity", "pTotalAmount", "pDiscount", "pVAT" };
            for (int i = 0; i < columns.Length; i++)
            {
                 cart.Columns.Add(new DataColumn(columns[i]));

            }
            for (int i = 0; i < columns2.Length; i++)
            {

                cart2.Columns.Add(new DataColumn(columns2[i]));
                cart3.Columns.Add(new DataColumn(columns2[i]));

            }

            invoiceTB.Text = currentOrderNum;
            executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            executionPath = executionPath.Substring(6); 
        }

        void form2_cellClicked(object sender, EventArgs e)
        {
            
        }
        public void fillOrders(string a) {
            try
            {
                dataGridView2.Rows.Clear();
            }
            catch (Exception) { }
            try
            {
                DataTable DT = queryAB.QueryDT("Select Order_Number, staff_Number,OrderAmount, OrderQuantity, OrderStatus, OrderDate FROM Orders WHERE Customer_Number='" + a.ToString() + "' AND (OrderStatus ='Pending' OR OrderStatus='Installment')");
                String[] rows;
                for (int j = 0; j < DT.Rows.Count; j++)
                {
                    rows = new String[] { (string)DT.Rows[j]["Order_Number"], (string)DT.Rows[j]["staff_Number"], DT.Rows[j]["OrderAmount"].ToString(), DT.Rows[j]["OrderQuantity"].ToString(), (string)DT.Rows[j]["OrderStatus"], DT.Rows[j]["OrderDate"].ToString() };
                    dataGridView2.Rows.Add(rows);
                }
            }
            catch (Exception ) { }

        }
        private void addCustomer_Click(object sender, EventArgs e)
        {

            using (addCustomerForm addCF = new addCustomerForm())
            {
                addCF.ShowDialog();
                addCF.cellClicked += new EventHandler(form2_cellClicked);
                custNumberTB.Text = addCF.getCustomerNumber;
                custFullNameTB.Text = addCF.getCustomerFullName;
                custCellTB.Text = addCF.getCustomerCell;
                custEmailTB.Text = addCF.getCustomerEmail;
                AccountBalanceTB.Text = addCF.getCustomerAccBal;
                fillOrders(addCF.getCustomerNumber);
                
            }
            
            ClearAll();
        }

        private void addProductBTN_Click(object sender, EventArgs e)
        {
            Clear();
            using (addProductForm addP = new addProductForm()) {
                addP.ShowDialog();
                addP.addProductClick += new EventHandler(addP_addProductClick);
                pCodeTB.Text = addP.getpCodes;
                pNameTB.Text = addP.getpNames;
                priceTB.Text = addP.getpPrice;
                dbQuantity = Convert.ToInt32(addP.getpQuantity);
                
                if (addP.getpQuantity != "" && addP.getpQuantity!= null)
                {
                    AvaliableQuantity = int.Parse(addP.getpQuantity);

                }

            }
        }

        void addP_addProductClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void loadData(){
            //Displaying Cart
              DataRow row = cart.NewRow();
              row["pName"] = pNameTB.Text;
              row["pCode"] = pCodeTB.Text;
              row["pSellingPrice"] = priceTB.Text;
              row["pQuantity"] = pQuantityTB.Text;              
              row["pTotalAmount"] = pTotalAmountTB.Text;

             //Storing cart
              DataRow row2 = cart2.NewRow();
              row2["pName"] = pNameTB.Text.ToString();
              row2["pCode"] = pCodeTB.Text.ToString();
              row2["pSellingPrice"] = priceTB.Text.ToString().Substring(1);
              row2["pQuantity"] = pQuantityTB.Text.ToString();
              row2["pTotalAmount"] = pTotalAmountTB.Text.ToString().Substring(1);
              if (discountTB.Text.ToString() != "")
              {
                  row2["pDiscount"] = discountTB.Text.ToString();
              }

              row2["Order_Number"] = currentOrderNum;

              cart2.Rows.Add(row2);
              cart.Rows.Add(row);
              dataGridView3.DataSource = cart;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 1)
            {
                int rowIndex = e.RowIndex;
                if (cart.Rows.Count > 0)
                {
                    cart.Rows.RemoveAt(rowIndex);
                    dataGridView3.DataSource = cart;

                }
            }

            else if (e.ColumnIndex == 0)
            {
                panelPayment.Visible = false;
                int rowIndex = e.RowIndex;
                if (cart.Rows.Count > 0)
                {
                    buttonAddCart.Enabled = false;
                    itemPosition = rowIndex;
                    pNameTB.Text = cart.Rows[rowIndex]["pName"].ToString();
                    pCodeTB.Text = cart.Rows[rowIndex]["pCode"].ToString();
                    priceTB.Text = cart.Rows[rowIndex]["pSellingPrice"].ToString();
                    pQuantityTB.Text = cart.Rows[rowIndex]["pQuantity"].ToString();

                }
          }

            
        }
        public void Clear()
        {
            buttonAddCart.Enabled = true;
            pNameTB.Clear();
            pCodeTB.Clear();
            priceTB.Clear();
            pQuantityTB.Clear();
            pTotalAmountTB.Clear();
            amountTB.Clear();
          //  VAT_CheckBox.Checked = false;
            discountTB.Clear();
            amountPaidTB.Text = "";
        }
        public void ClearAll()
        {   amountPaidTB.Text = "";
            button_Edit.Visible = true;
            buttonRemove.Visible = true;
            panelPayment.Visible = false;
            buttonAddCart.Enabled = true;
            pNameTB.Clear();
            pCodeTB.Clear();
            priceTB.Clear();
            pQuantityTB.Clear();
            pTotalAmountTB.Clear();
            amountTB.Clear();
          //  VAT_CheckBox.Checked = false;
            discountTB.Clear();
            cart.Rows.Clear();
            cart2.Rows.Clear();
            currentOrderNum = makeInvoicNumber();
            invoiceTB.Text = currentOrderNum.ToString();            
        }
        public bool checkItemInCart()
        {
            bool inCart=false;
            if(cart.Rows.Count>0 && !update){
            for(int i=0;i<cart.Rows.Count;i++){

              if(cart.Rows[i]["pCode"].Equals(pCodeTB.Text.ToString())){

                  inCart=true;
                  itemPosition=i;
              }

            }
          }

           return inCart;

        }

        public void checkAvaliability()
        {
            if (dbQuantity  == 0)
            {
                MessageBox.Show("The selected product is out of stock.");
            }
            else
            {          
              MessageBox.Show("Only " + dbQuantity.ToString() + " " + pNameTB.Text.ToString() + " are in-stock.");
            }
        }
        public void addToCart()
        {
            bool checkTB = false;
            if (pNameTB.Text == "" && !checkTB)
            {
                MessageBox.Show("Product name required. Please select a product with a name.");
                checkTB = true;
            }

            if (pCodeTB.Text == "" && !checkTB)
            {
                MessageBox.Show("Product code required. Please select a product with a code.");
                checkTB = true;
            }
            if (priceTB.Text == "" && !checkTB)
            {
                MessageBox.Show("Product selling price required. Please select a product with price.");
                checkTB = true;
            }
            if (pQuantityTB.Text == "" && !checkTB)
            {
                MessageBox.Show("Product quantity required. Please fill in quantity.");
                checkTB = true;

            }

            if (!checkTB)
            {
                int quantity = int.Parse(pQuantityTB.Text);
                if (quantity > dbQuantity)
                {
                    checkAvaliability();
                }
                else
                {
                    if (checkItemInCart())
                    {
                        try
                        {
                            int newQuantity = int.Parse(cart.Rows[itemPosition]["pQuantity"].ToString());
                            newQuantity += Convert.ToInt32(pQuantityTB.Text);
                            
                            if (quantity > dbQuantity)
                            {
                                checkAvaliability();
                            }
                            else
                            {
                                cart.Rows[itemPosition]["pQuantity"] = newQuantity.ToString();
                               // int newTotal = int.Parse(cart.Rows[itemPosition]["pTotalAmount"].ToString());
                              //  newTotal += int.Parse(pTotalAmountTB.Text);
                                
                                cart.Rows[itemPosition]["pTotalAmount"] = (int.Parse(priceTB.Text.Substring(1)) * newQuantity).ToString();
                                MessageBox.Show((int.Parse(priceTB.Text.Substring(1)) * newQuantity).ToString());
                                itemPosition = 0;
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Error! Failed to update quantity");
                        }

                    }
                    else
                    {
                        if (update)
                        {
                            cart.Rows.RemoveAt(itemPosition);
                            itemPosition = 0;
                            loadData();
                            update = false;

                        }
                        else
                        {
                          loadData();
                        }
                    }
                    Clear();
                }
            }
        }
        private void buttonAddCart_Click(object sender, EventArgs e)
        {
            addToCart();
        }
        public double getAmount(string a, string b)
        {
            double amount=0;
            try
            {
                if (priceTB.Text != "" && pQuantityTB.Text != "")
                {
                    amount = Convert.ToDouble(a) * Convert.ToDouble(b);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please Enter a numberic value for Quanity");
            }
            

            return amount;
        }
        private void pQuantityTB_TextChanged(object sender, EventArgs e)
        {
            if (pQuantityTB.Text.Equals(""))
            {
                
            }
            else
            {       amountTB.Text = "R"+getAmount(pQuantityTB.Text, priceTB.Text.Substring(1)).ToString();
                    pTotalAmountTB.Text = amountTB.Text;
            }
        }
        public void getDiscount()
        {
            double amount = 0;
            double discount = 0;
            double discountAmount;
            if (discountTB.Text != ""&& amountTB.Text!="")
            {

                  
                        try
                        {
                        discount = Double.Parse(discountTB.Text.ToString());
                        amount = Double.Parse(amountTB.Text.ToString());
                        discountAmount = amount * (discount / 100);
                        amount -= discountAmount;
                        pTotalAmountTB.Text = amount.ToString();
                        }                        
                        catch (Exception)
                        {
                            MessageBox.Show("Error! Please make sure all data is in the correct format");
                        }
                  
            }
            else
            {
                if (pQuantityTB.Text.Equals(""))
                {

                }
                else
                {
                    amountTB.Text = getAmount(pQuantityTB.Text, priceTB.Text).ToString();
                    pTotalAmountTB.Text = amountTB.Text;

                }
             }
        }
        //public void getVAT()
        //{
        //    if (VAT_CheckBox.Checked && amountTB.Text!="")
        //    {
        //        double amount = 0;
        //        double VAT = 0.15;
        //        double VAT_Amount;

        //            if(discountTB.Text==""){
        //                try
        //                {
        //                    amount = Double.Parse(amountTB.Text.ToString());
        //                    VAT_Amount = amount * VAT;
        //                    amount += VAT_Amount;
        //                    pTotalAmountTB.Text = amount.ToString();
        //                }
        //                catch (InvalidCastException)
        //                {
        //                    MessageBox.Show("Error! Please make sure all data is in the correct format");
        //                }
        //            }  
        //            else{

        //                try
        //                {
        //                    amount = Double.Parse(pTotalAmountTB.Text.ToString());
        //                    VAT_Amount = amount * VAT;
        //                    amount += VAT_Amount;
        //                    pTotalAmountTB.Text = amount.ToString();
        //                }
        //                catch (InvalidCastException)
        //                {
        //                    MessageBox.Show("Error! Please make sure all data is in the correct format");
        //                }

        //            }
        //    }
        //    else
        //    {
        //        if (pQuantityTB.Text.Equals(""))
        //        {

        //        }
        //        else
        //        {
        //            amountTB.Text = getAmount(pQuantityTB.Text, priceTB.Text).ToString();
        //            pTotalAmountTB.Text = amountTB.Text;
        //        }
        //    }
        //}

        //private void discountTB_TextChanged(object sender, EventArgs e)
        //{
        //    if (discountTB.Text != "")
        //    {
        //        getDiscount();
        //    }
        //    else
        //    {
        //        getVAT();
        //    }
        //}
        //private void VAT_CheckBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (VAT_CheckBox.Checked)
        //    {
        //        getVAT();

        //    }
        //    else
        //    {
        //        getDiscount();

        //    }
        //}
        private void buttonRemoveProductDetails_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void buttonSaveUpdates_Click(object sender, EventArgs e)
        {
            if (cart.Rows.Count == 0)
            {
                MessageBox.Show("Nothing to update. The cart is empty");
            }            
            else  {
                update = true;
                addToCart();
                buttonAddCart.Enabled = true;
                Clear();
            }
        }
        private void buttonReset_Click(object sender, EventArgs e)
        {
            cart.Rows.Clear();
             cart2.Rows.Clear();
            dataGridView3.DataSource = cart;
        }
        private void button1_Click(object sender, EventArgs e)
        {
           ClearAll();
        }
        public string makeInvoicNumber()
        {
            string name = "ORD";
            Random random = new Random();
            int username = random.Next(11, 999999999);
            name = name + username;
            return name;
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Clear();
            cart.Rows.Clear();
            cart2.Rows.Clear();
            dataGridView3.DataSource = cart;
            buttonAddCart.Enabled = true;
        }

        private void buttonFinalizeOrder_Click(object sender, EventArgs e)
        {

            if (cart2.Rows.Count > 0)
            {
            DataTable orders = posSql.QueryDT("Select * FROM Orders");
            DataRow orderRow = orders.NewRow();
            orderRow["Order_Number"] = currentOrderNum.ToString();
            if (custNumberTB.Text.ToString() != "")
            {
                orderRow["Customer_Number"] = custNumberTB.Text.ToString();

            orderRow["staff_Number"] = Main.staffNumber.ToString();
           double orderAmount=0;
           double orderQuantity=0;
           for (int i = 0; i < cart2.Rows.Count; i++)
           {
               try
               {
                   orderAmount += Double.Parse(cart2.Rows[i]["pTotalAmount"].ToString());
                   orderQuantity += Double.Parse(cart2.Rows[i]["pQuantity"].ToString());
               }
               catch (Exception)
               {

                   MessageBox.Show("Failed to compute Order amount and quantity.\nPlease make sure that InvoiceNumber  input  is correct.");
               }
           }

           orderRow["OrderAmount"] = orderAmount.ToString();
           orderRow["OrderQuantity"] = orderQuantity.ToString();
           orderRow["OrderStatus"] = "Pending";
          orders.Rows.Add(orderRow);
           posSql.UpDate(orders);
           if (sqlCommandAB.message ==null)
           {

               MessageBox.Show("Order saved successfully.");
           }
           else
           {

               MessageBox.Show("Failed to save Order!");
               MessageBox.Show(sqlCommandAB.message);
                    }

                    
            DataTable dt = posSql.QueryDT("Select * FROM OrderItem");
                    for (int i = 0; i < cart2.Rows.Count; i++)
                {
                    DataRow row = dt.NewRow();
                   // string r =dt.Rows[0]["Order_Number"].ToString(); 
                    row["Order_Number"] = cart2.Rows[i]["Order_Number"].ToString();
                    row["pName"] = cart2.Rows[i]["pName"].ToString();
                    row["pCode"] = cart2.Rows[i]["pCode"].ToString();
                    row["pSellingPrice"] = cart2.Rows[i]["pSellingPrice"].ToString();
                    row["pQuantity"] = cart2.Rows[i]["pQuantity"].ToString();
                    row["pTotalAmount"] = cart2.Rows[i]["pTotalAmount"].ToString();
                    //if (cart2.Rows[i]["pDiscount"].ToString() != "")
                    //{
                    //    row["pDiscount"] = cart2.Rows[i]["pDiscount"].ToString();
                    //}
                    //row["pVAT"] = cart2.Rows[i]["pVAT"].ToString();
                    dt.Rows.Add(row);
                }


                posSql.UpDate(dt);
                if (sqlCommandAB.message == null)
                {
                        dataGridView2.Rows.Clear();
                        DataTable dataOrders = posSql.QueryDT("Select Order_Number, staff_Number,OrderAmount, OrderQuantity, OrderStatus, OrderDate FROM Orders WHERE Customer_Number='" + custNumberTB.Text.ToString() + "'AND (OrderStatus ='Pending'OR OrderStatus='Installment')");
                    int len = dataOrders.Rows.Count;
                        String[] items = new String[len];
                        String[] row;
                        for (int i = 0; i < len; i++) {
                             row = new String[] {(string)dataOrders.Rows[i]["Order_Number"],(string)dataOrders.Rows[i]["staff_Number"],dataOrders.Rows[i]["OrderAmount"].ToString(),dataOrders.Rows[i]["OrderQuantity"].ToString(),(string)dataOrders.Rows[i]["OrderStatus"],dataOrders.Rows[i]["OrderDate"].ToString()};
                            dataGridView2.Rows.Add(row);
                        }

                   // conn.updateQuery(query); 
                    
                   // dataGridView2.DataSource = conn.getData();
                    ClearAll();
                }
                else
                {

                    MessageBox.Show("Failed to save Cart!");
                  
                }
            }
            else
            {
                MessageBox.Show("Customer number is missing. Please select the customer.");

            }
            }

            else
            {
                MessageBox.Show("Can't save an empty cart!");

            }
        }

      private void invoiceTB_TextChanged(object sender, EventArgs e)
      {

      }

      private void custNumberTB_TextChanged(object sender, EventArgs e)
      {
          try
          {
              cImg.Image = Image.FromFile(executionPath + @"\CustomerImages\" + (string)(custNumberTB.Text) + ".bmp");
          }
          catch (Exception) {
              cImg.Image = null;
          }
         
      }

      private void button4_Click(object sender, EventArgs e)
      {
          panelPayment.Visible = true;
      }

      private void button2_Click(object sender, EventArgs e)
      {
          panelPayment.Visible = false;
      }
      public Boolean validatePayment() {

        if (amountPaidTB.Text == "")
          {
              MessageBox.Show("Please Enter Cash Amount to be paid");
              amountPaidTB.Text = "";
              amountPaidTB.Focus();
              return true;
          }
        if (decimal.TryParse(amountPaidTB.Text, out amountPaid))
        { 
            return false;
        }
           
        
        else
        {
            MessageBox.Show("Please enter a valid amount");
            return true;
        }
   
      }
      private void payBTN_Click(object sender, EventArgs e)
      {
          if (paymentCB.Text == "")
          {
              MessageBox.Show("Please select Payment mode");
              paymentCB.Focus();

          }
          else
          {
              switch (paymentCB.Text)
              {
                  case "Cash":
                      sqlCommandAB cashPay = new sqlCommandAB();
                      if (validatePayment()) { }
                      else
                      {
                          if (decimal.TryParse(amountDueTB.Text.Substring(1), out amountDue))
                          {
                              //This is where the Database needs to be updated!
                              change = amountPaid - amountDue;
                              if (change >= 0)
                              {//Make Cash payments 
                                  InserPayment();
                                  //Update the Orders....
                                  UpdateOrderStatus();
                                  updateInventory();
                                  UpdateCustomerAccountLimit();
                                  updateCustomerCreditLimit();
                                  if (sqlCommandAB.message != null)
                                  {
                                      MessageBox.Show(sqlCommandAB.message);
                                  }
                                  else {
                                      MessageBox.Show("Successfully Saved Payment...");
                                      print();
                                      ClearAll();
                                     // dataGridView2.Rows.Clear();
                                  }
                                  
                              }
                              else
                              {
                                  //Over here I need to check if what i am doing is the right thing and the calculations for the Cash payments that is less than the amount
                                  //If a customer for example makes an order, with an order amount of R50000 and decides to pay an amount of only R10000
                                  //Then the system needs to make a cash payment of R10000 and then save the order as an
                                  //Installment with an Order amount of R40000.
                                  TotalCredit = (-1) * change + customerCreditBal;
                                  if (creditLimit > TotalCredit)
                                  {

                                      //This is working But for Now It shouldn't allow saving because CreditLimit must be lower...
                                      MessageBox.Show("Customer is over his Creadit Limit by " + (-1) * (creditLimit - TotalCredit));
                                     UpdateOrderAmount((amountDue-amountPaid));
                                     InserPayment();
                                     UpdateOrderDetails();
                                     updateInventory();
                                     UpdateCustomerAccount();
                                     print();
                                     printRecipt();
                                     ClearAll();
                                  }
                                  else {

                                      InserPayment();
                                      UpdateOrderDetails();
                                      updateInventory();
                                      UpdateCustomerAccount();
                                      //add the Order updates
                                      //I think I should modify this to serve as a notification to the employee 
                                      //That the transaction has completed, and that if there is an error then I
                                      //should reverse back the ransactions....

                                      //If there was an error somewhere the system should reverse the transactions
                                      MessageBox.Show("Please pay your Account within 3 Months");
                                      print();
                                      printRecipt();
                                      ClearAll();
                                  }
                                  
                              }


                          }
                          else { MessageBox.Show("There was an error with the order, We can't recieve payment now"); }

                      }

                      break;
                  case "Credit":
                      if (valCredit()) { }
                      else
                      {
                          InserPayment();
                          UpdateOrderDetails();
                          updateInventory();
                          UpdateCustomerAccount();
                          MessageBox.Show("Thank you, please pay R" + TotalCredit.ToString() + " within 3 months");
                          printRecipt();
                          ClearAll();
                      }
                      break;
              }

          }
      }
 
      public void UpdateOrderStatus() {
        sqlCommandAB upo = new sqlCommandAB();
        upo.updateDB("UPDATE Orders  Set OrderStatus ='Completed' WHERE (Order_Number ='" + (string)oNumberTB.Text+ "')");
       
      }
      public void UpdateOrderDetails()
      {
          sqlCommandAB upo = new sqlCommandAB();
          upo.updateDB("UPDATE Orders  Set OrderStatus ='Installment' WHERE (Order_Number ='" + (string)oNumberTB.Text + "')");

      }
      public void UpdateOrderAmount(Decimal price)
      {
          sqlCommandAB upo = new sqlCommandAB();
          upo.updateDB("UPDATE Orders  Set OrderAmount ='"+price+"' WHERE (Order_Number ='" + (string)oNumberTB.Text + "')");

      }
      public void updateInv(string pCode, int q) {
         
          sqlCommandAB upIn = new sqlCommandAB();
          int pqtyy = upIn.getSingleValue("SELECT pQty FROM Product WHERE pCode ='" + pCode + "'");
          upIn.updateDB("UPDATE Product set pQty =' "+(pqtyy-q)+"' WHERE pCode ='"+pCode+"'");
      }
      public void updateInventory() {
          sqlCommandAB upInv = new sqlCommandAB();
          DataTable dt = upInv.QueryDT("SELECT pCode,pQuantity FROM OrderItem WHERE (Order_Number ='" + (string)oNumberTB.Text + "')");

          int count = dt.Rows.Count;
        //  MessageBox.Show(count.ToString() + " order Number" + (string)oNumberTB.Text );
          for (int i = 0; i < count; i++) {
              updateInv(dt.Rows[i]["pCode"].ToString(),Convert.ToInt32(dt.Rows[i]["pQuantity"]));
              
          }
      
      }
      public void UpdateCustomerAccount()
      {
          sqlCommandAB upC = new sqlCommandAB();

          upC.updateDB("UPDATE Customer Set Customer_CreditBalance = '"+Convert.ToDouble(TotalCredit)+"' WHERE (Customer_Number ='" + (string)custNumberTB.Text+ "')");
         // upC.updateDB("UPDATE Customers set Customer_CreditLimit ='Customer_CreditLimit + " + (0.20) * (Convert.ToDouble(TotalCredit)) + "' WHERE Customer_Number = '" + (string)custNumberTB.Text + "'");
          //  MessageBox.Show(Convert.ToDouble(TotalCredit) + " " + (string)custNumberTB.Text);
      }
      public void updateCustomerCreditLimit()
      {

          string cNum = (string)custNumberTB.Text;
          double credit = Convert.ToDouble(getCreditB(cNum)) + Convert.ToDouble(amountPaidTB.Text)*0.20;
          sqlCommandAB ab = new sqlCommandAB();
          ab.updateDB("UPDATE Customer set Customer_CreditLimit ='" + credit + "' WHERE Customer_Number = '" + cNum + "'");
          
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
      public void UpdateCustomerAccountLimit()
      {
          sqlCommandAB upC = new sqlCommandAB();
          
         // upC.updateDB("UPDATE Customers Set Customer_CreditBalance = '" + Convert.ToDouble(TotalCredit) + "' WHERE (Customer_Number ='" + (string)custNumberTB.Text + "')");
          upC.updateDB("UPDATE Customer set Customer_CreditLimit ='" + (getCreditB((string)custNumberTB.Text)) + (0.20) * (Convert.ToDouble(TotalCredit)) + "' WHERE Customer_Number = '" + (string)custNumberTB.Text + "'");
          //  MessageBox.Show(Convert.ToDouble(TotalCredit) + " " + (string)custNumberTB.Text);
      }
      public void InserPayment()
      {
        
         // MessageBox.Show((string)oNumberTB.Text + " " + (string)custNumberTB.Text + " " + Convert.ToDouble(orderAmount) + " " + Convert.ToInt32(orderQty) + " " +
         //     Convert.ToDouble(amountPaidTB.Text) + " " + (string)paymentCB.Text);
          sqlCommandAB brandSql = new sqlCommandAB();
          DataTable dt = brandSql.QueryDT("SELECT * FROM Payment");
          DataRow dr = dt.NewRow();
          dr["Order_Number"] = (string)oNumberTB.Text;
          dr["staff_Number"] = (string)custNumberTB.Text;
          dr["Customer_Number"] = (string)custNumberTB.Text;
          dr["OrderAmount"] = Convert.ToDouble(orderAmount);
          if (change > 0) {
              dr["pChange"] = Convert.ToDouble(change);
          }
          dr["OrderQuantity"] = Convert.ToInt32(orderQty);
          try
          {
              dr["pAmount"] = Convert.ToDouble(amountPaidTB.Text);
          }
          catch (Exception) {
              dr["pAmount"] = 0;
          }
          dr["pType"] = (string)paymentCB.Text;    
          dt.Rows.Add(dr);
          brandSql.UpDate(dt);

      }
  
      private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
      {
          int rowIndex = dataGridView2.SelectedCells[0].RowIndex;
          DataGridViewRow dv = dataGridView2.Rows[rowIndex];
          orderQty = dv.Cells["OrderQuantity"].Value.ToString();
          orderAmount = dv.Cells["OrderAmount"].Value.ToString();
          amountDueTB.Text= "R"+orderAmount;
          orderNumber = dv.Cells["Order_Number"].Value.ToString();
          oNumberTB.Text = orderNumber;
          ClearAll();
          panelPayment.Visible = true;
          button_Edit.Visible = false;
          buttonRemove.Visible = false;
          string order_Num = dv.Cells["Order_Number"].Value.ToString();
         cart2= posSql.QueryDT("Select * from OrderItem where Order_Number ='" + order_Num + "'");
         cart3 = cart2;
          for (int i = 0; i < cart2.Rows.Count; i++)
          {
              invoiceTB.Text = cart2.Rows[i]["Order_Number"].ToString();

              DataRow row = cart.NewRow();
              row["pName"] = cart2.Rows[i]["pName"].ToString();
              row["pCode"] = cart2.Rows[i]["pCode"].ToString();
              row["pSellingPrice"] = cart2.Rows[i]["pSellingPrice"].ToString();
              row["pQuantity"] = cart2.Rows[i]["pQuantity"].ToString();
              row["pTotalAmount"] = cart2.Rows[i]["pTotalAmount"].ToString();
              cart.Rows.Add(row);
          }
          dataGridView3.DataSource = cart;

          sqlCommandAB custBal = new sqlCommandAB();
          DataTable dt = custBal.QueryDT("SELECT Customer_CreditBalance, Customer_CreditLimit FROM Customer WHERE (Customer_Number ='" + (string)custNumberTB.Text + "')");
          if (sqlCommandAB.message != null)
          {
              MessageBox.Show("There is an error by the Connecting to the Database, Cannot get Customer Details");
          }

          else
          {
              //dt.Rows[0]["Customer_CreditBalance"].ToString();
              try
              {
                  if( (decimal.TryParse(dt.Rows[0]["Customer_CreditLimit"].ToString(), out creditLimit) && decimal.TryParse(dt.Rows[0]["Customer_CreditBalance"].ToString(), out customerCreditBal))) { }
                  else
                  {
                      MessageBox.Show("Customer Credit Details unavaileable, Please Restart the system...");
                  }
                  AccountBalanceTB.Text = "R"+customerCreditBal.ToString();
              }
              catch (Exception) { AccountBalanceTB.Text = "0.00"; MessageBox.Show("You may recieve cash payments only."); }
              
              
          }
      }
         public Boolean valCredit() {
             if (TotalCredit == 0) {
                 MessageBox.Show("You don't have any orders to take on credit");
                 return true;
             }
           //  else if(TotalCredit >
             return false;
             
      }
              private void print() {
          PrintDialog printDialog = new PrintDialog();

          PrintDocument printDocument = new PrintDocument();

          printDialog.Document = printDocument; //add the document to the dialog box...        

          printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt);



          DialogResult result = printDialog.ShowDialog();

          if (result == DialogResult.OK)
          {
              printDocument.Print();

          }
          

      }
              private void printRecipt()
              {
                  PrintDialog printDialog = new PrintDialog();

                  PrintDocument printDocument = new PrintDocument();

                  printDialog.Document = printDocument; //add the document to the dialog box...        

                  printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceiptCredit);



                  DialogResult result = printDialog.ShowDialog();

                  if (result == DialogResult.OK)
                  {
                      printDocument.Print();

                  }


              }
         public void CreateReceipt(object sender, System.Drawing.Printing.PrintPageEventArgs e)
         {            
             double cash = 0.00d;
             double change = 0.00d;
             cash = (double)amountPaid;
             Graphics g = e.Graphics;
             Font font = new Font("Courier New", 12);
             float fontHight = font.GetHeight();
             int startX = 10;
             int startY = 10;
             int offset = 40;
             g.DrawString(companyName+" - Invoice", new Font("Courier New", 30, FontStyle.Underline), new SolidBrush(Color.Black), startX, startY + 5);
             offset = offset + (int)fontHight + 5;
             g.DrawString("Address: "+companyAddress, new Font("Courier New", 12), new SolidBrush(Color.Black), startX, startY + offset);
             offset = offset + (int)fontHight + 5;
             g.DrawString("Contacts: ("+companyCell+")", new Font("Courier New", 12), new SolidBrush(Color.Black), startX, startY + offset);
             offset = offset + (int)fontHight + 10;
             g.DrawString("Staff Name: ---> " + Main.staffName, new Font("Courier New", 10), new SolidBrush(Color.Black), startX, startY + offset);

             offset = offset + (int)fontHight + 5;
             g.DrawString("Order Number: ---> " + cart3.Rows[0]["Order_Number"].ToString(), new Font("Courier New", 10), new SolidBrush(Color.Black), startX, startY + offset);
             offset = offset + (int)fontHight + 5;
             g.DrawString(DateTime.Now.ToString("MM/dd/yyyy hh:mm"), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
             offset = offset + (int)fontHight + 5;

             string top = "#".PadRight(5) + "Product Name".PadRight(25) + "Price".PadRight(10) + "Quantity".PadRight(10) + "Total Amount".PadRight(10);
             g.DrawString(top, new Font("Courier New", 12, FontStyle.Underline), new SolidBrush(Color.Black), startX, startY + offset);
             offset = offset + (int)fontHight + 5;
             double totalprice = 0.00d; 

             for (int i = 0; i < cart3.Rows.Count; i++)
             {
                 string item = (i + 1).ToString().PadRight(5) + cart3.Rows[i]["pName"].ToString().PadRight(25) + "R" + cart3.Rows[i]["pSellingPrice"].ToString().PadRight(10) +
                     cart3.Rows[i]["pQuantity"].ToString().PadRight(10) + "R" + cart3.Rows[i]["pTotalAmount"].ToString().PadRight(10);


                 g.DrawString(item, font, new SolidBrush(Color.Black), startX, startY + offset);

                 offset = offset + (int)FontHeight + 5;


                 totalprice += Convert.ToDouble(cart3.Rows[i]["pTotalAmount"]);

                 //  MessageBox.Show(pName + " " + pPrice + " " + pQuantity + " " + pTotalAmnt + " " + totalprice);
             }
             change = (cash - totalprice);

             offset = offset + 20;
             g.DrawString("Grand Total ".PadRight(30) + "R" + totalprice, new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
             offset = offset + 30;
             g.DrawString("CASH ".PadRight(30) + "R" + cash.ToString(), new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
             offset = offset + 30;
             g.DrawString("CHANGE ".PadRight(30) + "R" + change, new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
             offset = offset + 30;
             g.DrawString("     Thank you for visiting us, Please Call Again!", font, new SolidBrush(Color.Black), startX + 80, startY + offset);

         }
         public void CreateReceiptCredit(object sender, System.Drawing.Printing.PrintPageEventArgs e)
         {
             
             
             Graphics g = e.Graphics;
             Font font = new Font("Courier New", 12);
             float fontHight = font.GetHeight();
             int startX = 10;
             int startY = 10;
             int offset = 40;
             g.DrawString(companyName +" - Invoice", new Font("Courier New", 30, FontStyle.Underline), new SolidBrush(Color.Black), startX, startY + 5);
             offset = offset + (int)fontHight + 5;
             g.DrawString("Address: " + companyAddress , new Font("Courier New", 12), new SolidBrush(Color.Black), startX, startY + offset);
             offset = offset + (int)fontHight + 5;
             g.DrawString("Contacts: ("+companyCell+")", new Font("Courier New", 12), new SolidBrush(Color.Black), startX, startY + offset);
             offset = offset + (int)fontHight + 10;
             g.DrawString("Staff Name: ---> " + Main.staffName, new Font("Courier New", 10), new SolidBrush(Color.Black), startX, startY + offset);

             offset = offset + (int)fontHight + 5;
             g.DrawString("Order Number: ---> " + cart3.Rows[0]["Order_Number"].ToString(), new Font("Courier New", 10), new SolidBrush(Color.Black), startX, startY + offset);
             offset = offset + (int)fontHight + 5;
             g.DrawString(DateTime.Now.ToString("MM/dd/yyyy hh:mm"), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
             offset = offset + (int)fontHight + 5;

             string top = "#".PadRight(5) + "Product Name".PadRight(25) + "Price".PadRight(10) + "Quantity".PadRight(10) + "Total Amount".PadRight(10);
             g.DrawString(top, new Font("Courier New", 12, FontStyle.Underline), new SolidBrush(Color.Black), startX, startY + offset);
             offset = offset + (int)fontHight + 5;
             double totalprice = 0.00d;

             for (int i = 0; i < cart3.Rows.Count; i++)
             {
                 string item = (i + 1).ToString().PadRight(5) + cart3.Rows[i]["pName"].ToString().PadRight(25) + "R" + cart3.Rows[i]["pSellingPrice"].ToString().PadRight(10) +
                     cart3.Rows[i]["pQuantity"].ToString().PadRight(10) + "R" + cart3.Rows[i]["pTotalAmount"].ToString().PadRight(10);


                 g.DrawString(item, font, new SolidBrush(Color.Black), startX, startY + offset);

                 offset = offset + (int)FontHeight + 5;


                 totalprice += Convert.ToDouble(cart3.Rows[i]["pTotalAmount"]);

                 //  MessageBox.Show(pName + " " + pPrice + " " + pQuantity + " " + pTotalAmnt + " " + totalprice);
             }

             offset = offset + 20;
             g.DrawString("Your Total Credit is now = R" +TotalCredit.ToString() + " Please Pay your Credit within 3 months ", new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
             
             //This is the reason the print was a bit set off because I forgot to set the off set
             offset = offset + 20;
            
             g.DrawString("     Thank you for visiting us, Please Call Again!", font, new SolidBrush(Color.Black), startX + 80, startY + offset);

         }
      private void paymentCB_SelectedIndexChanged(object sender, EventArgs e)
      {

          if (paymentCB.Text == "Credit") {
              amountPaidTB.Visible = false;
          //    changeTB.Visible = false;
              label27.Visible = false;
          //    label4.Visible = false;
              totalCreditTB.Visible = true;
              label19.Visible = true;
              decimal ad = Convert.ToDecimal(amountDueTB.Text.Substring(1));
              TotalCredit = customerCreditBal + ad;
              
              totalCreditTB.Text = TotalCredit.ToString();
              
          }
          else if (paymentCB.Text == "Cash") {
              totalCreditTB.Visible = false;
              label19.Visible = false;
              amountPaidTB.Visible = true;
             // changeTB.Visible = true;
              label27.Visible = true;
            //  label4.Visible = true;
          }
      }

    }
}
