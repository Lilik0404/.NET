using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Net.WebRequestMethods;
using System.Net.NetworkInformation;

namespace lucru_individual
{
    public partial class Form2 : Form
    {
        public string username, password;
       public SqlConnection connection;
        public Form2()
        {
            InitializeComponent();
            button1.Focus();
        }
        public bool IsConnectedToInternet()
        {
            string host = "google.com";  
            bool result = false;
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 3000);
                if (reply.Status == IPStatus.Success)
                    result = true;
            }
            catch {  result=false; }
            return result;
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
          string ip ="127.0.0.1";
          string port = "1433";
          string server = "SQLEXPRESS";
        string host = ip + "," + port+"\\"+server+";";
            
            username = textBox1.Text;
            Form1 main = new Form1();
            
            password = textBox2.Text;
            string connectionString = "Data Source="+host+"Initial Catalog=Donaris;User ID=" + username + ";Password=" + password + ";MultipleActiveResultSets=true;";

            if (IsConnectedToInternet())
            {
                SqlConnection connection = new SqlConnection(connectionString);
                using (connection)
                {
                    try
                    {
                        connection.Open();
                        this.Hide();
                        main.Show();
                        main.connection = connectionString;
                        main.conection = new SqlConnection(connectionString);
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("Ati gresit login sau parola!");
                    }
                }

            }
            else
            {
                MessageBox.Show("Nu aveti conectiune la internet!");
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1.PerformClick();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
