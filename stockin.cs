using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace AGK_POS
{
    public partial class stockin : UserControl
    {
   
        //Attributes
        private OpenFileDialog loader = new OpenFileDialog();
        private bool hide = false;
      //  private  DataTable dt;
        private int value;
        private Image img;
        private int brandID;
        private String pCode;
      
        private string executionPath = null; 
        //Constrictor
        public stockin()
        {
            InitializeComponent();
            executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            executionPath = executionPath.Substring(6);
            CategorysAndBrand[] itemArray;
            getCatData(out itemArray);

           
        }
        private Image ResizeImage(Image img, Size newSize)
        {
            Image newImg = new Bitmap(newSize.Width, newSize.Width);
            using (Graphics art = Graphics.FromImage((Bitmap)newImg)){

                art.DrawImage(img, new Rectangle(Point.Empty, newSize));
            }

            return newImg;
        }

        //====================================================================================
        //This is what I have added and Modified the Item Entry (Start Time 12:22AM 4/28/2018
       // ======================================================================================
        public void getCatData(out CategorysAndBrand[] Cat_Array) {
                 selectCat.Items.Clear();
            

                sqlCommandAB brandSql = new sqlCommandAB();
                DataTable DT = brandSql.QueryDT("SELECT * FROM Category");
                int rowCount = DT.Rows.Count;
                Cat_Array = new CategorysAndBrand[rowCount];
                string[] row;
                for (int j = 0; j < rowCount; j++)
                {
                    row = new string[] { (string)DT.Rows[j]["categoryName"], (string)DT.Rows[j]["categoryDescription"] };
                    

                    selectCat.Items.Add(DT.Rows[j]["categoryName"]);
                }
                

            
        }
        public void getBrandData(out CategorysAndBrand[] Cat_Array, string category)
        {
            comboBox1.Items.Clear();
            

            sqlCommandAB brandSql = new sqlCommandAB();
            DataTable DT = brandSql.QueryDT("SELECT * FROM Brand WHERE categoryName= '" + category + "' ");
            int rowCount = DT.Rows.Count;
            Cat_Array = new CategorysAndBrand[rowCount];
            string[] row;
            for (int j = 0; j < rowCount; j++)
            {
                row = new string[] { (string)DT.Rows[j]["brandName"], (string)DT.Rows[j]["brandDescription"] };
                
                comboBox1.Items.Add(DT.Rows[j]["brandName"]);

            }
            
        }


        private void selectCat_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (selectCat.SelectedIndex == -1) { }
            else
            {
                CategorysAndBrand[] brandArray;
                getBrandData(out brandArray, selectCat.SelectedItem.ToString());
            }
        }

     
        public void InsertNewProduct()
        {
            
            sqlCommandAB brandSql = new sqlCommandAB();
            DataTable dt = brandSql.QueryDT("SELECT * FROM Product");
            DataRow dr = dt.NewRow();
            try { dr["categoryName"] = selectCat.SelectedItem.ToString(); }
            catch (NullReferenceException) {
                MessageBox.Show("Please Select a category first.");
            }
            
            dr["brandID"] = brandID;
           dr["pCode"] = tb_itemBarCode.Text.ToString();
           dr["pName"] = tb_itemName.Text.ToString();
           try
           {
               dr["pCostPrice"] = Convert.ToDecimal(tb_itemStockPrice.Text);
               dr["pSellingPrice"] = Convert.ToDecimal(tb_itemSellingPrice.Text);
               dr["pReorderQty"] = Convert.ToInt32(ReorderPoint.Value);
               dr["pQty"] = Convert.ToInt32(qty.Value);
           }
           catch (FormatException) {
               MessageBox.Show("Check your input values");
           }
           dr["pDescription"] = tb_itemDescription.Text.ToString();    
           // dr["pImage"] = imageString;

           
            dt.Rows.Add(dr);
            brandSql.UpDate(dt);

        }
        public Boolean validate(){
            if(selectCat.Text == "" || selectCat.SelectedIndex == -1){
                MessageBox.Show("Select A category First");
                selectCat.Focus();
                return true;           
            }
            else if(comboBox1.Text == "" || comboBox1.SelectedIndex == -1){
                MessageBox.Show("Select a brand First");
                comboBox1.Focus();
                return true;
            
            }
            else if (tb_itemName.Text.ToString()=="" || tb_itemSellingPrice.Text=="" ||tb_itemStockPrice.Text.ToString() == "")
            {  
                MessageBox.Show("Please fill in all the required details");
                return true;
            } 

            else 
            {
                  return false;
            }
        
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (validate()) { }
            else
            {
                InsertNewProduct();
                if (img != null)
                {
                    ResizeImage(img, new Size(80, 80)).Save(executionPath + @"\ItemImages\" + tb_itemBarCode.Text.ToString() + ".bmp");
                }
                DialogResult dr = MessageBox.Show("A New Product," + tb_itemName.Text + "\nHas Been Added to the Database");
                if (dr == DialogResult.OK)
                {
                    addProductForm[] products;
                    getProduct(out products);
                    Clear();
                }
            }
        }
        public void Clear() {
            selectCat.Enabled = true;
            buttonSave.Enabled = true;
            comboBox1.Enabled = true;
            deleteBTN.Enabled = false;
            updateBTN.Enabled = false;
            qty.Value = 0;
            ReorderPoint.Value = 0;
            tb_itemSellingPrice.Text = tb_itemStockPrice.Text = tb_itemName.Text = tb_itemDescription.Text = tb_itemBarCode.Text = "";
            itemImage.Image = Properties.Resources.defaultImage;

        }

        public string makeProductCode(string one, string two ) {
            Random rand = new Random();
            Int64 num = rand.Next(1,5000);
            string code = one.Substring(0,1) + two.Substring(0, 5) + num;
            return code;
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                buttonLoadPic.Enabled = false;
                removeImage.Enabled = false;
            }
            else {
                buttonLoadPic.Enabled = true;
                removeImage.Enabled = true;
                sqlCommandAB brandSql = new sqlCommandAB();
                brandID = brandSql.getSingleValue("SELECT brandID FROM Brand WHERE (categoryName = '" + selectCat.SelectedItem.ToString() + "') AND ( brandName ='" + comboBox1.SelectedItem.ToString() + "')");
                tb_itemBarCode.Text = makeProductCode(selectCat.SelectedItem.ToString(), comboBox1.SelectedItem.ToString());
                addProductForm[] products;
                getProduct(out products);
            }
        
        }

        public void getProduct(out addProductForm[] Cat_Array)
        {
            dataGridView3.Rows.Clear();
            sqlCommandAB brandSql = new sqlCommandAB();
            DataTable DT = brandSql.QueryDT("SELECT * FROM Product");
            int rowCount = DT.Rows.Count;
            Cat_Array = new addProductForm[rowCount];
            string[] row;
            for (int j = 0; j < rowCount; j++)
            {
                
                string brandName = brandSql.getSingleString("SELECT brandName FROM Brand WHERE brandID = '" + Convert.ToInt32(DT.Rows[j]["brandID"]) + "'");
                row = new string[] { (string)DT.Rows[j]["pCode"], (string)DT.Rows[j]["pName"], (string)DT.Rows[j]["categoryName"], brandName, DT.Rows[j]["pSellingPrice"].ToString() };
                                
                dataGridView3.Rows.Add(row);
                loadData();

            }

        }
        public void loadData()
        {
            
            int numRow = dataGridView3.Rows.Count;
            for (int j = 0; j < numRow; j++)
            {
                try
                {
                    dataGridView3.Rows[j].Cells["Photo"].Value = Image.FromFile(executionPath + @"\ItemImages\" + dataGridView3.Rows[j].Cells["code"].Value.ToString() + ".bmp");
                }
                catch (Exception)
                {

                    dataGridView3.Rows[j].Cells["Photo"].Value = Properties.Resources.defaultImage.GetThumbnailImage(80, 80, null, new IntPtr());

                }
            }
        }

        //====================================================================================
////When the save button is clicked, do this:
//        private void buttonSave_Click(object sender, EventArgs e) {
//                    string currentDate = DateTime.Now.ToString("yyy-MM-dd");

//                    if (tb_itemName.Text.ToString()=="" || tb_itemBarCode.Text=="" ||
//                             tb_itemStockPrice.Text.ToString() == "")                   
                                    
//                       {

//                           MessageBox.Show("Please fill in all the required details");

//                       } 

//                    else {

//                          string querY = "SELECT * FROM Item";
//                          SQLConnector sqlConn = new SQLConnector(querY);
//                          dt = sqlConn.getData();

//                          FileStream fs;
//                              Byte[] imagebinary;
//                               fs = new FileStream(loader.FileName, FileMode.Open, FileAccess.Read);
//                              imagebinary = new byte[Convert.ToInt32(fs.Length)];
//                               fs.Read(imagebinary, 0, Convert.ToInt32(fs.Length));
//                           fs.Close();
            
//                          DataRow row = dt.NewRow();

//            row["Brand_ID"] = value;
//                 row["Item_Name"] = tb_itemName.Text.ToString();
//                       row["Item_Code"] = tb_itemBarCode.Text.ToString();
//                            // row["Item_Threshold"] = tb_itemThreshold.Text.ToString();
//                                    row["Item_on_Promotion"] = onPromo;
//                                       row["Item_StockPrice"] = tb_itemStockPrice.Text.ToString();
//                                          row["Item_SellingPrice"] = tb_itemSellingPrice.Text.ToString();
//                                  //     row["Item_SellingScale"] = tb_itemSellingScale.Text.ToString();
//                                //   row["Item_Quantity"] = tb_itemQuantity.Text.ToString();
//                          row["Item_Added_date"] = currentDate;
//                   //   row["Item_Size"] = tb_itemSize.Text.ToString();
//                      if (onPromo == 0)
//                      {
//                          row["Item_Promotion_Price"] = 0.00;
//                      }
//                      else
//                      {

//                          row["Item_Promotion_Price"] = 0.00;

//                      }
//                row["Photo"] = imagebinary;
                           
//             dt.Rows.Add(row);
//             sqlConn.insertData(dt);
              
//             if (sqlConn.insertChecker)
//             {
//                  MessageBox.Show(tb_itemName + " added successfully.");
//                  clear();
//             }
 
//            }
//        }

//








//hide and show sub menu
        private void showMenu(object sender, EventArgs e)
        {
            subMenuPanel.Show();
        }

        private void hideMenu(object sender, EventArgs e)
        {
            if (hide)
            {
                subMenuPanel.Hide();
            }
        }
//

//fix the menu visible
        private void fixShow(object sender, MouseEventArgs e)
        {
            if (hide == true)
            {
                hide = false;

            }

            else
            {
                hide = true;
            }

        }
//

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
            
            comboBox1.SelectedIndex = -1;
            selectCat.SelectedIndex = -1;

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            
        }
     

//Load a picture into a picture box
        private void buttonLoadPic_Click(object sender, EventArgs e)
        {
            try
            {
                loader.Filter = "Choose Image(AllFiles)|*.jpg;*.png;*.gif ";
                if (loader.ShowDialog() == DialogResult.OK)
                {
                    img = Image.FromFile(loader.FileName);
                    itemImage.Image = img;
                }
            }
            catch
            {

                MessageBox.Show("Error! Please check if the image is in a correct format and if it exist.");

            }
        }
//
        private void itemImage_Click(object sender, EventArgs e)
        {


        }
//Delete a picture
        private void Button6_Click(object sender, EventArgs e)
        {
            itemImage.Image = Properties.Resources.defaultImage;
        }

        private void clear()
        {

            tb_itemSellingPrice.Enabled = true;
            tb_itemName.Clear();
            tb_itemBarCode.Clear();
            tb_itemStockPrice.Clear();
            tb_itemSellingPrice.Clear();
            itemImage.Image = Properties.Resources.defaultImage;
           
            

        }

        private void selectedIndexChanged(object sender, EventArgs e)
        {
           value = (int)comboBox1.SelectedValue;
           string v = value.ToString();
          

        }

       

          private void tb_itemName_TextChanged(object sender, EventArgs e)
        {
            if (tb_itemName.Text != "")
            {
                star1Label.Hide();
            }
            else
            {
                star1Label.Show();

            }
        }

        private void tb_itemStockPrice_TextChanged_1(object sender, EventArgs e)
        {
            if (tb_itemStockPrice.Text != "")
            {
                star4Label.Hide();
            }
            else
            {
                star4Label.Show();

            }
        }

        private void tb_itemSellingPrice_TextChanged_1(object sender, EventArgs e)
        {
            if (tb_itemSellingPrice.Text != "")
            {
                star5Label.Hide();
            }
            else
            {
                star5Label.Show();

            }
        }

        private void stockin_Load(object sender, EventArgs e)
        {
            dataGridView3.RowTemplate.Height = 80;
            addProductForm[] products;
            getProduct(out products);
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Clear();
            buttonSave.Enabled = false;
            selectCat.Enabled = false;
            comboBox1.Enabled = false;
            int selectedrowindex = dataGridView3.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView3.Rows[selectedrowindex];
            string a = Convert.ToString(selectedRow.Cells["code"].Value);
           //  pCode = (string)tb_itemBarCode.Text;
            sqlCommandAB p = new sqlCommandAB();
            DataTable dt = p.QueryDT("SELECT * FROM Product WHERE (pCode = '"+a+"')");
            selectCat.Text = selectedRow.Cells["category"].Value.ToString();
            comboBox1.Text = selectedRow.Cells["brand"].Value.ToString();
            
            populate(dt);
            pCode = (string)tb_itemBarCode.Text;
            deleteBTN.Enabled = true;
            updateBTN.Enabled = true;

        }
        public void populate(DataTable me) {
                
                tb_itemBarCode.Text =(string)me.Rows[0]["pCode"];
                tb_itemName.Text = (string)me.Rows[0]["pName"];
                tb_itemSellingPrice.Text = me.Rows[0]["pSellingPrice"].ToString();
                tb_itemStockPrice.Text = me.Rows[0]["pCostPrice"].ToString();
                ReorderPoint.Text = me.Rows[0]["pReorderQty"].ToString();
                qty.Text = me.Rows[0]["pQty"].ToString();
                tb_itemDescription.Text = (string)me.Rows[0]["pDescription"];
               try
                {
                itemImage.Image = Image.FromFile(executionPath + @"\ItemImages\" + (string)me.Rows[0]["pCode"] + ".bmp");
               
                    
                }
                catch (Exception) { 
                  
                }
        
        }

        private void deleteBTN_Click(object sender, EventArgs e)
        {
            int selectedrowindex = dataGridView3.SelectedCells[0].RowIndex;          
            DataGridViewRow selectedRow = dataGridView3.Rows[selectedrowindex];

            string a = Convert.ToString(selectedRow.Cells["code"].Value);
            sqlCommandAB customerSql = new sqlCommandAB();
            DialogResult dr = MessageBox.Show("Are you sure you want to Delete " + tb_itemName.Text.ToString() + "Permanently?", "Confim Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dr == DialogResult.Yes)
            {
                if (sqlCommandAB.message != null)
                {
                    MessageBox.Show(sqlCommandAB.message);
                }
                else
                {
                    DataTable DT = customerSql.QueryDT("DELETE  FROM Product where pCode ='" + a + "' ");
                    dataGridView3.Rows.Clear();
                    addProductForm[] products;
                    getProduct(out products);
                    loadData();
                    Clear();
                }
            }
        }

        private void updateBTN_Click(object sender, EventArgs e)
        {
            String PN = (string)tb_itemName.Text;
            Decimal  StP = Convert.ToDecimal(tb_itemSellingPrice.Text);
            Decimal SP = Convert.ToDecimal(tb_itemStockPrice.Text);
            int RP = Convert.ToInt32(ReorderPoint.Text); 
            int qunti = Convert.ToInt32(qty.Text);
            String PD = (string)tb_itemDescription.Text;
            
           // MessageBox.Show("item name " + PN + " sellingPrice " + StP + " Stock Price " + SP + " Reorder point " + RP + " qauntity " + qunti + " Des " + PD + " PCode "+pCode );
            sqlCommandAB st = new sqlCommandAB();
            st.updateDB("UPDATE Product set pCostPrice = '" + SP + "', pName ='" + PN + "', pSellingPrice ='" + StP + "', pReorderQty ='" + RP + "', pQty ='" + qunti + "', pDescription ='" + PD + "' WHERE pCode = '" +pCode+ "'");
            // pName = '" + PN + "'   
                DialogResult dr = MessageBox.Show("Product " + PN.ToString() + " Has Been Updated");
                if (dr == DialogResult.OK)
                {
                    Clear();
                }
            
        }
        public void getProductSearch(out addProductForm[] Cat_Array)
        {
            dataGridView3.Rows.Clear();
            
            string allQ = (string)textBox1.Text;
            sqlCommandAB brandSql = new sqlCommandAB();
            DataTable DT = brandSql.QueryDT("SELECT * FROM Product  WHERE ((pCode LIKE '" + allQ + "%') OR (brandID LIKE '%" + allQ + "%')) OR (pName LIKE '%" + allQ + "%')");
            int rowCount = DT.Rows.Count;
            Cat_Array = new addProductForm[rowCount];
            string[] row;
            for (int j = 0; j < rowCount; j++)
            {

                string brandName = brandSql.getSingleString("SELECT brandName FROM Brand WHERE brandID = '" + Convert.ToInt32(DT.Rows[j]["brandID"]) + "'");
                row = new string[] { (string)DT.Rows[j]["pCode"], (string)DT.Rows[j]["pName"], (string)DT.Rows[j]["categoryName"], brandName, DT.Rows[j]["pSellingPrice"].ToString() };

                dataGridView3.Rows.Add(row);
                loadData();

            }

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            addProductForm[] Cat_Array;
            getProductSearch(out Cat_Array);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (img != null)
            {
                ResizeImage(img, new Size(80, 80)).Save(executionPath + @"\ItemImages\" + tb_itemBarCode.Text.ToString() + ".bmp");
            }
        }

    
    }
}
