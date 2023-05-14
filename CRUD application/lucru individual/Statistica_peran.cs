using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace lucru_individual
{
    public partial class Statistica_peran : Form
    {
        public Statistica_peran()
        {
            InitializeComponent();
        }
        public SqlConnection con;
        
        private void iconButton1_Click(object sender, EventArgs e)
        {
            int count = 0, sum = 0, avg = 0, branchid = 0;
            con.Open();

            SqlCommand commandd = new SqlCommand("select in_branch from agents where id like user_id() ", con);
            SqlDataReader readerr = commandd.ExecuteReader();

            using (readerr)
            {
                while (readerr.Read())
                {
                    branchid = readerr.GetInt32(0);

                }
            }
            commandd.Cancel();
            readerr.Close();

            SqlCommand command = new SqlCommand("select count(id) from contracts where branch=" + branchid + "and DATEDIFF(year, date_conclusion, GETDATE())<=1 ", con);
            SqlDataReader reader = command.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                        count = reader.GetInt32(0);
                    else
                        count = 0;
                }
            }
            command.Cancel();
            reader.Close();

            SqlCommand command1 = new SqlCommand("select sum(total_amount ) from contracts where branch=" + branchid + "and DATEDIFF(year, date_conclusion, GETDATE())<=1 ", con);
            SqlDataReader reader1 = command1.ExecuteReader();
            using (reader1)
            {
                while (reader1.Read())
                {
                    if (!reader1.IsDBNull(0))
                        sum = reader1.GetInt32(0);
                    else
                        sum = 0;
                }
            }
            command1.Cancel();
            reader1.Close();
            SqlCommand command2 = new SqlCommand("select avg(total_amount ) from contracts where branch=" + branchid + "and DATEDIFF(year, date_conclusion, GETDATE())<=1 ", con);
            SqlDataReader reader2 = command2.ExecuteReader();
            using (reader2)
            {
                while (reader2.Read())
                {
                    if (!reader2.IsDBNull(0))
                        avg = reader2.GetInt32(0);
                    else avg = 0;
                }
            }
            command2.Cancel();
            reader2.Close();
            label4.Text = count.ToString();
            label5.Text = sum.ToString() + " MDL";
            label6.Text = avg.ToString() + " MDL";
            con.Close();
        }
    }
}
