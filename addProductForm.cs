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
    public partial class addProductForm : Form
    {
        private string pCodes;
        private string pNames;
        private string pPrice;
        private string pQuantity;         
        private string executionPath = null; 
        public string getpCodes{

            get
            {
                return pCodes;
            }
    
        }
        public string getpQuantity
        {

            get
            {
                return pQuantity;
            }
        }

        public string getpNames
        {

            get
            {
                return pNames ;
            }

        }
        public string getpPrice
        {

            get
            {
                return pPrice;
            }

        }
        public addProductForm()
        {
            InitializeComponent();
            executionPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            executionPath = executionPath.Substring(6);
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.RowTemplate.MinimumHeight = 80;
        }
        public void getProducts() {
            dataGridView1.Rows.Clear();
            sqlCommandAB addProducts = new sqlCommandAB();
            DataTable DT = addProducts.QueryDT("SELECT * FROM Product");
            String[] row;
            for (int j = 0; j < DT.Rows.Count; j++) {
                string brand = addProducts.getSingleString("SELECT brandName FROM brand WHERE brandID = '" + DT.Rows[j]["brandID"] + "'");
                row = new string[] { (string)DT.Rows[j]["pName"], DT.Rows[j]["pCode"].ToString(), (string)DT.Rows[j]["categoryName"], (string)brand, "R" + DT.Rows[j]["pSellingPrice"].ToString(), DT.Rows[j]["pQty"].ToString(), DT.Rows[j]["pReorderQty"].ToString(), (string)DT.Rows[j]["pDescription"] };
                dataGridView1.Rows.Add(row);
            }
           
        }
        public void loadData()
        {
            getProducts();
            int numRow = dataGridView1.Rows.Count;
            totalProducts.Text = "Total Number Of Items: " + numRow;
            for (int i = 0; i < numRow; i++)
            {
                try
                {
                    dataGridView1.Rows[i].Cells["Photo"].Value = Image.FromFile(executionPath + @"\ItemImages\" + dataGridView1.Rows[i].Cells["iCode"].Value.ToString() + ".bmp");
                }
                catch (Exception)
                {
                    dataGridView1.Rows[i].Cells["Photo"].Value = Properties.Resources.defaultImage.GetThumbnailImage(80, 80, null, new IntPtr());

                }

            }
        }
        private void addProductForm_Load(object sender, EventArgs e)
        {            
             CategoryCombox.SelectedIndex = -1;
             BrandcomboBox.SelectedIndex = -1;
             
             loadData();
        }   
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public event EventHandler addProductClick;
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

            pCodes = (string)(selectedRow.Cells["iCode"].Value);
            pNames = (string)selectedRow.Cells["iName"].Value;
            pPrice = selectedRow.Cells["iSellingprice"].Value.ToString();
            pQuantity = selectedRow.Cells["iQty"].Value.ToString();
            if (addProductClick != null)
            {
                addProductClick(this, new EventArgs());
            }
            this.Hide();

        }

    }
}
