using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace lucru_individual
{
    public partial class Adaugare_filiala : Form
    {
        public Adaugare_filiala()
        {
            InitializeComponent();
        }
        public SqlConnection connection;
        private bool DigitOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
        private bool checkname()
        {
            string nume = textBox3.Text;
            if (!String.IsNullOrEmpty(nume))
            {
                return true;
            }
            else return false;
        }
        private bool checkadress()
        {
            string adress = textBox2.Text;
            if (!String.IsNullOrEmpty(adress))
            {
                return true;
            }
            else return false;
        }
        private bool checkphone()
        {
            string phone = textBox1.Text;
            
            if (DigitOnly(phone)&&phone.Length == 9)
            {   
                    return true;
            }
            else return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkphone())
            {
                MessageBox.Show("Verificati numarul de telefon!", "Eroare la numarul de telefon", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkname())
            {
                MessageBox.Show("Verificati Numele !", "Eroare la Nume", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkadress())
            {
                MessageBox.Show("Verificati Adresa !", "Eroare la Adresa", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string phone = textBox1.Text;
                string adress=textBox2.Text;
                string name=textBox3.Text;
                int idbranch = 0;
                connection.Open();
                SqlCommand insercontract = new SqlCommand("use donaris insert into branches values('" + name + "' , '" + adress + "' , '" + phone+ "');", connection);
                insercontract.ExecuteNonQuery();
                insercontract.Cancel();
                connection.Close();
                MessageBox.Show("Ati adaugat o filiala noua!");
            }
        }
    }
}
