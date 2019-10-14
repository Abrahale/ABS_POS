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
    public partial class CategorysAndBrand : UserControl
    {
        //to update the brands table
      //  private static string categoryNameG = "ab";        
        private static int brandID = 1;
        
        public CategorysAndBrand()
        {
            InitializeComponent();
            
            CategorysAndBrand[] catArray;
            getCatData(out catArray);
            getBrandData(out catArray);

            
        }

        public void Clear() {
            CategoryNameTB.Text = Category_DescriptionTB.Text = "";
            brandNameTB.Text = brandDescriptionTB.Text = "";
            updateBTN.Enabled = false;
            deleteBTN.Enabled = false;
            saveBTN.Enabled = true;
        }
        public void InserNewCategory() {
      
                sqlCommandAB brandSql = new sqlCommandAB();
                DataTable dt = brandSql.QueryDT("SELECT * FROM Category");
                DataRow dr = dt.NewRow();
                dr["categoryName"] = CategoryNameTB.Text;
                dr["categoryDescription"] = Category_DescriptionTB.Text;
                dt.Rows.Add(dr);
                brandSql.UpDate(dt);
        
        }
        public void InsertNewBrand()
        {
                sqlCommandAB brandSql = new sqlCommandAB();
                DataTable dt = brandSql.QueryDT("SELECT * FROM Brand");
                DataRow dr = dt.NewRow();
                dr["categoryName"] = selectCat.SelectedItem.ToString();
                dr["brandName"] = brandNameTB.Text;
                dr["brandDescription"] = brandDescriptionTB.Text;

                dt.Rows.Add(dr);
                brandSql.UpDate(dt);

        }
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
                    dataGridView1.Rows.Add(row);

                    selectCat.Items.Add(DT.Rows[j]["categoryName"]);
                }
                selectCat.SelectedIndex = -1;

            
        }
        public void getBrandData(out CategorysAndBrand[] Cat_Array)
        {

                sqlCommandAB brandSql = new sqlCommandAB();
                DataTable DT = brandSql.QueryDT("SELECT * FROM Brand");
                int rowCount = DT.Rows.Count;
                Cat_Array = new CategorysAndBrand[rowCount];
                string[] row;
                for (int j = 0; j < rowCount; j++)
                {
                    row = new string[] { (string)DT.Rows[j]["brandName"], (string)DT.Rows[j]["categoryName"], (string)DT.Rows[j]["brandDescription"] };
                    dataGridView2.Rows.Add(row);


                }

        }
        public void getBrandData(out CategorysAndBrand[] Cat_Array, string category)
        {
                brandDescriptionDT.HeaderText  = "Brand Description";
                sqlCommandAB brandSql = new sqlCommandAB();
                DataTable DT = brandSql.QueryDT("SELECT * FROM Brand WHERE categoryName= '" + category + "' ");
                int rowCount = DT.Rows.Count;
                Cat_Array = new CategorysAndBrand[rowCount];
                string[] row;
                for (int j = 0; j < rowCount; j++)
                {
                    row = new string[] { (string)DT.Rows[j]["brandName"], (string)DT.Rows[j]["categoryName"], (string)DT.Rows[j]["brandDescription"] };
                    dataGridView2.Rows.Add(row);


                }

        }
        private void saveBTN_Click(object sender, EventArgs e)
        {
            InserNewCategory();
            DialogResult dr = MessageBox.Show("A New Category " + CategoryNameTB.Text + " Has Been Added to the Data Base");
            if (dr == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
                CategorysAndBrand[] catArray;
                getCatData(out catArray);
                Clear();
            }
        }

        private void cancelBTN_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void clearBTN_Click_1(object sender, EventArgs e)
        {
            Clear();
            brandDescriptionDT.HeaderText = "Category";
            selectCat.SelectedIndex =-1;

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            Clear();
            brandNameTB.Text = brandDescriptionTB.Text = "";
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

            string a = Convert.ToString(selectedRow.Cells["CategoryName"].Value);

                sqlCommandAB customerSql = new sqlCommandAB();
                DataTable DT = customerSql.QueryDT("SELECT * FROM Category where categoryName ='" + a + "' ");
                CategoryNameTB.Text = (string)DT.Rows[0]["categoryName"];
                Category_DescriptionTB.Text = (string)DT.Rows[0]["categoryDescription"];



            //This should fill the Brand Table
            dataGridView2.Rows.Clear();
            CategorysAndBrand[] brandArray;
            selectCat.SelectedItem = a;
       
            getBrandData(out brandArray,a);
            
            deleteBTN.Enabled = true;
            updateBTN.Enabled = true;
            saveBTN.Enabled = false;
        }

        private void deleteBTN_Click(object sender, EventArgs e)
        {
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

            string a = Convert.ToString(selectedRow.Cells["CategoryName"].Value);

                sqlCommandAB customerSql = new sqlCommandAB();


                DialogResult dr = MessageBox.Show("Are you sure you want to Delete " + CategoryNameTB.Text.ToString() + "Permanently?", "Confim Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dr == DialogResult.Yes)
                {

                    DataTable DT = customerSql.QueryDT("DELETE  FROM Category where categoryName ='" + a + "' ");
                    dataGridView1.Rows.Clear();
                    CategorysAndBrand[] customer;
                    getCatData(out customer);
                    Clear();
                }
        }

        private void selectCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            brandNameTB.Enabled = true;
            brandDescriptionTB.Enabled = true;
            brandSBTN.Enabled = true;
            dataGridView2.Rows.Clear();
            CategorysAndBrand[] brandArray;
                 
                 if (selectCat.SelectedIndex == -1)
                 {
                     getBrandData(out brandArray);
                     brandUBTN.Enabled = false;
                     brandDBTN.Enabled = false;
                     brandSBTN.Enabled = false;
                 }
                 else
                 {
                     getBrandData(out brandArray, selectCat.SelectedItem.ToString());
                 }

        }

        private void barndSBTN_Click(object sender, EventArgs e)
        {   InsertNewBrand();
            DialogResult dr = MessageBox.Show("A New Brand " + brandNameTB.Text + "  Has Been Added to the Data Base");
            if (dr == DialogResult.OK)
            {
                dataGridView2.Rows.Clear();
                CategorysAndBrand[] brandArray;
                getBrandData(out brandArray);
                Clear();
            }
        }

        private void brandClearBTN_Click(object sender, EventArgs e)
        {Clear();
            brandNameTB.Text = brandDescriptionTB.Text = "";
            selectCat.SelectedIndex = -1;
            brandUBTN.Enabled = false;
            brandDBTN.Enabled = false;
            brandSBTN.Enabled = true;
            selectCat.SelectedIndexChanged -= selectCat_SelectedIndexChanged;
            
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectCat.SelectedIndexChanged -= selectCat_SelectedIndexChanged;
            brandNameTB.Enabled = true;
            brandDescriptionTB.Enabled = true;
            int selectedrowindex = dataGridView2.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView2.Rows[selectedrowindex];
            if(CategoryNameTB.Text != ""){
                selectCat.SelectedValue = CategoryNameTB.Text;
            }
            else{                
               
                selectCat.SelectedItem = Convert.ToString(selectedRow.Cells["brandDescriptionDT"].Value);
            }
            
            brandNameTB.Text = Convert.ToString(selectedRow.Cells["brandNameDT"].Value);
            brandDescriptionTB.Text = Convert.ToString(selectedRow.Cells["brandDescriptionDT2"].Value);
            brandUBTN.Enabled = true;
            brandDBTN.Enabled = true;
            brandSBTN.Enabled = false;
            selectCat.SelectedIndexChanged += selectCat_SelectedIndexChanged;
        }

        private void brandDBTN_Click(object sender, EventArgs e)
        {

            int selectedrowindex = dataGridView2.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dataGridView2.Rows[selectedrowindex];

            string a = Convert.ToString(selectedRow.Cells["brandNameDT"].Value);
            sqlCommandAB customerSql = new sqlCommandAB();
            int id = customerSql.getSingleValue("SELECT brandID FROM Brand WHERE( categoryName = '" + selectCat.SelectedItem.ToString() + "' AND brandName ='" + a + "')");
           
            
            DialogResult dr = MessageBox.Show("Are you sure you want to Delete " + brandNameTB.Text.ToString() + " Permanently?", "Confim Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dr == DialogResult.Yes)
            {

                DataTable DT = customerSql.QueryDT("DELETE  FROM Brand WHERE brandID = '"+id+"' AND brandName ='" + a + "' ");
                if (sqlCommandAB.message != null)
                {
                    MessageBox.Show(sqlCommandAB.message + "Couldn't delete " + a);
                }
                else
                {
                    dataGridView2.Rows.Clear();
                    CategorysAndBrand[] customer;
                    getBrandData(out customer);
                    Clear();
                }
            }
        }

        private void updateBTN_Click(object sender, EventArgs e)
        {
           
            sqlCommandAB categroySql = new sqlCommandAB();
            if (CategoryNameTB.Text == "")
            {
                MessageBox.Show("Category is Blank");
            }
            else
            {
                categroySql.updateDB("UPDATE Category set categoryDescription='" + (string)Category_DescriptionTB.Text + "' WHERE categoryName='" + (string)CategoryNameTB.Text + "'");
                if (sqlCommandAB.message != null)
                {
                    MessageBox.Show(sqlCommandAB.message + "Couldn't update " + (string)CategoryNameTB.Text + "\nDue to netwrok Error, Please check your network and try again");
                }
                else
                {
                    MessageBox.Show("Category Description for  " + (string)CategoryNameTB.Text + " update succefully");
                    dataGridView2.Rows.Clear();
                    CategorysAndBrand[] customer;
                    getBrandData(out customer);
                    Clear();
                }

            }
        }
        //public string[] brandValues(string a, string b, string c) {
        //    string[] brandthings = new string[] {a,b,c };
        //    return brandthings;
        //}
        private void brandUBTN_Click(object sender, EventArgs e)
        {
            int selectedrowindex = dataGridView2.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dataGridView2.Rows[selectedrowindex];
            string a = Convert.ToString(selectedRow.Cells["brandNameDT"].Value);

            sqlCommandAB brandSql = new sqlCommandAB();
            int id = brandSql.getSingleValue("SELECT brandID FROM Brand WHERE( categoryName = '" + selectCat.SelectedItem.ToString() + "' AND brandName ='" + a + "')");
           
            if (brandNameTB.Text.Equals(""))
            {
                MessageBox.Show("Please Enter a Brand Name");
            }

            else {                
                
                brandSql.updateDB("UPDATE Brand set brandName='"+(string)brandNameTB.Text+"',brandDescription='"+(string)brandDescriptionTB.Text+"' WHERE brandID='"+id+"'");
                if (sqlCommandAB.message != null)
                {
                    MessageBox.Show(sqlCommandAB.message + "Couldn't update " + a + "\nDue to netwrok Error, Please check your network and try again");
                }
                else
                {
                    MessageBox.Show("The brand  " + a + " update succefully");
                    dataGridView2.Rows.Clear();
                    CategorysAndBrand[] customer;
                    getBrandData(out customer);
                    Clear();
                }

            }
        }
        

     
      



    }
}
