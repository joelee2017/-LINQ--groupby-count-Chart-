﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace 常用LINQ查詢語法_groupby與count
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// cbCountry.DataSource 查詢Country Distinct清除重覆
        /// group c by c.Country into CountryGroup  select new
        /// 建立一個新的群組查詢
        ///國家 = CountryGroup.Key,  索引
        ///人數 = CountryGroup.Count() 計數
        ///OrderByDescending(group=>group.人數) 排序()指定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: 這行程式碼會將資料載入 'northwindDataSet.Customers' 資料表。您可以視需要進行移動或移除。
            this.customersTableAdapter.Fill(this.northwindDataSet.Customers);
            NorthwindEntities dc = new NorthwindEntities();

           // cbCountry.DataSource = dc.Customers.Select(c => c.Country).Distinct().ToArray();
            //combobox

            
            var query = (from c in dc.Customers
                         group c by c.Country into CountryGroup
                         select new
                         {
                             國家 = CountryGroup.Key,
                             人數 = CountryGroup.Count()
                         }).OrderByDescending(group=>group.人數);
           
            dataGridView1.DataSource = query.ToArray();

        }

        private void chart1_PrePaint(object sender, System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs e)
        {
            foreach(DataPoint p in chart1.Series[0].Points)
            {
                p["Exploded"] = "True";
            }
        }

        /// <summary>
        /// string Country = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
        /// 建立變數供  dc.Customers.Where 比對使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            NorthwindEntities dc = new NorthwindEntities();
            string Country = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            dataGridView2.DataSource = dc.Customers.Where(c => c.Country == Country).ToArray();
        }


        /// <summary>
        /// LINQ 敘述
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            NorthwindEntities dc = new NorthwindEntities();
            //var query = from c in dc.Customers
            //            where !(from o in dc.Orders select o.CustomerID).Distinct().
            //            Contains(c.CustomerID)
            //            select c;
            //dataGridView2.DataSource = query.ToArray();

            var query = dc.Customers.Where(
                c => c.Orders.Count() >= 5).Select(c =>
                   c.CompanyName);
            dataGridView2.DataSource = query.ToArray();
        }
    }
}
