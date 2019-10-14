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
    public partial class Orders : UserControl
    {
       public Orders()
        {
            InitializeComponent();            
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowTemplate.Height = 50;
            dataGridView1.RowTemplate.MinimumHeight = 50;
            fillGrid();

        }

         public void fillGrid()
         {
             dataGridView1.Rows.Clear();
             sqlCommandAB o = new sqlCommandAB();
             DataTable dt = o.QueryDT("SELECT * FROM Orders");
             int count = dt.Rows.Count;
             
             string[] dr;
             for (int i = 0; i < count; i++) {
                 dr = new string[] { (string)dt.Rows[i]["Order_Number"], (string)dt.Rows[i]["Customer_Number"], (string)dt.Rows[i]["staff_Number"], dt.Rows[i]["OrderAmount"].ToString(), dt.Rows[i]["OrderQuantity"].ToString(), (string)dt.Rows[i]["OrderStatus"], dt.Rows[i]["OrderDate"].ToString() };
                 dataGridView1.Rows.Add(dr);
             }       
         
         }
         public void fillGrid(string a)
         {
             dataGridView1.Rows.Clear();
             sqlCommandAB o = new sqlCommandAB();
             DataTable dt = o.QueryDT("SELECT * FROM Orders "+a);
             int count = dt.Rows.Count;

             string[] dr;
             for (int i = 0; i < count; i++)
             {
                 dr = new string[] { (string)dt.Rows[i]["Order_Number"], (string)dt.Rows[i]["Customer_Number"], (string)dt.Rows[i]["staff_Number"], dt.Rows[i]["OrderAmount"].ToString(), dt.Rows[i]["OrderQuantity"].ToString(), (string)dt.Rows[i]["OrderStatus"], dt.Rows[i]["OrderDate"].ToString() };
                 dataGridView1.Rows.Add(dr);
             }
         }
         public void orderItem(string a)
         {
             
             sqlCommandAB o = new sqlCommandAB();
             DataTable dt = o.QueryDT("SELECT * FROM OrderItem " + a);
             int count = dt.Rows.Count;

             string[] dr;
             for (int i = 0; i < count; i++)
             {
                 dr = new string[] { (string)dt.Rows[i]["pCode"], (string)dt.Rows[i]["pName"], dt.Rows[i]["pSellingPrice"].ToString(), dt.Rows[i]["pQuantity"].ToString(), dt.Rows[i]["pTotalAmount"].ToString() };
                 dataGridView2.Rows.Add(dr);
             }

             DataTable dz = o.QueryDT("SELECT * FROM Payment " + a);
             int c = dz.Rows.Count;
             if(c >0){
                 groupBox2.Visible = true;
                 grandTotal.Text = "R"+dz.Rows[0]["OrderAmount"].ToString();
                 pAmount.Text = "R"+dz.Rows[0]["pAmount"].ToString();
                 pChange.Text = "R"+dz.Rows[0]["pChange"].ToString();
                 pMode.Text = dz.Rows[0]["pType"].ToString();
             }
         }


         private void Complete_CheckedChanged(object sender, EventArgs e)
         {

             dataGridView2.Rows.Clear();
             groupBox2.Visible = false;
             fillGrid("WHERE OrderStatus = 'Completed'");
         }

         private void Installment_CheckedChanged(object sender, EventArgs e)
         {

             dataGridView2.Rows.Clear();
             groupBox2.Visible = false;
             fillGrid("WHERE OrderStatus = 'Installment'");
         }

         private void pending_CheckedChanged(object sender, EventArgs e)
         {
             dataGridView2.Rows.Clear();
             groupBox2.Visible = false;
             fillGrid("WHERE OrderStatus = 'Pending'");
         }

         private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
         {
             int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

             DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

             string a = Convert.ToString(selectedRow.Cells["Order_Number"].Value);
             dataGridView2.Rows.Clear();
             orderItem("WHERE Order_Number = '"+a+"'");
         }

         private void radioOnline_CheckedChanged(object sender, EventArgs e)
         {

             dataGridView2.Rows.Clear();
             groupBox2.Visible = false;
             fillGrid("WHERE OrderStatus = 'Online-Transaction'");
         }
    }

}
