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
    public partial class Stergere_contract : Form
    {
        public Stergere_contract()
        {
            InitializeComponent();
        }
        public SqlConnection connection;
        private int idcontract = 0;
        public void Loadcombobox()
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("Select id from contracts", connection);
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
                cmd = new SqlCommand("Select * from contracts where id = " + comboBox1.Text, connection);
                idcontract = Int32.Parse(comboBox1.Text);
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
                button2.Visible = true;
                cmd.Cancel();
                connection.Close();
            }
        }

        private void Stergere_contract_Shown(object sender, EventArgs e)
        {
            Loadcombobox();
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
            button2.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                connection.Open();
                cmd = new SqlCommand("Delete from contracts where id = " + idcontract, connection);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Ati sters contractul cu id:" + idcontract);
                Loadcombobox();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("Nu s-a gasit nici un contract!");
            }
            finally
            {


                iconButton1.Visible = true;
                label1.Visible = true;
                comboBox1.Visible = true;
                button1.Visible = false;
                dataGridView1.Visible = false;
                button2.Visible = false;
                cmd.Cancel();
                connection.Close();
               
            }
        }
    }
}
