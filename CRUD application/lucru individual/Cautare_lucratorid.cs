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

namespace lucru_individual
{
    public partial class Cautare_lucratorid : Form
    {
        public Cautare_lucratorid()
        {
            InitializeComponent();
        }

        public SqlConnection connection;
        public void Loadcombobox()
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("Select id from agents", connection);
            SqlDataReader reader = cmd.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetInt32(0));

                }

            }
            reader.Close();
            cmd.Cancel();
            connection.Close();
        }
        private void iconButton1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                connection.Open();
                cmd = new SqlCommand("Select * from agents where id = " + comboBox1.Text, connection);
                DataTable table = new DataTable();
                table.Load(cmd.ExecuteReader());
                dataGridView1.DataSource = table;

            }
            catch (SqlException)
            {
                MessageBox.Show("Selectati un id din lista!");
            }
            finally
            {


                iconButton1.Visible = false;
                label1.Visible = false;
                comboBox1.Visible = false;
                button1.Visible = true;
                dataGridView1.Visible = true;
                cmd.Cancel();
                connection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            dataGridView1.Refresh();
            comboBox1.Visible = true;
            label1.Visible = true;
            iconButton1.Visible = true;
            dataGridView1.Visible = false;
            button1.Visible = false;
        }

        private void Cautare_lucratorid_Shown(object sender, EventArgs e)
        {
            Loadcombobox();
        }
    }
}
