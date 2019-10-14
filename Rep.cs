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
    public partial class Rep : UserControl
    {
        public Rep()
        {
            InitializeComponent();
            ///////////////////////////////////////////////////

    
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void crystalReportViewer3_Load(object sender, EventArgs e)
        {

        }

        private void Rep_Load(object sender, EventArgs e)
        {
            ordersTableAdapter1.Fill(group9DataSet1.Orders);
            orderItemTableAdapter1.Fill(group9DataSet1.OrderItem);
            CrystalReport21.SetDataSource(group9DataSet1);
            crystalReportViewer1.ReportSource = CrystalReport21;


            ///////////////////////////////////////////////////////////
            brandTableAdapter1.Fill(group9DataSet1.Brand);
            productTableAdapter1.Fill(group9DataSet1.Product);
            CrystalReport41.SetDataSource(group9DataSet1);
            crystalReportViewer2.ReportSource = CrystalReport41;

            customerTableAdapter1.Fill(group9DataSet1.Customer);

            CrystalReport81.SetDataSource(group9DataSet1);
            crystalReportViewer4.ReportSource = CrystalReport81;

            CrystalReport101.SetDataSource(group9DataSet1);
            crystalReportViewer3.ReportSource = CrystalReport101;
        }

       

       

       

        

        
    }
}
