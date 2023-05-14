using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lucru_individual
{
    public partial class Adaugare_lucrator : Form
    {
        public Adaugare_lucrator()
        {
            InitializeComponent();
        }
        public SqlConnection connection;
        private bool checkname()
        {        string adresa = textBox3.Text;
                if (!String.IsNullOrEmpty(adresa))
                {
                    return true;
                }
                else return false;

        }
        private bool checksurname()
        {
            string adresa = textBox4.Text;
            if (!String.IsNullOrEmpty(adresa))
            {
                return true;
            }
            else return false;
        }
        private bool checkid()
        {
            int id = 0;
            int check = 0;
            
            if (Int32.TryParse(textBox1.Text, out id))
            {
                connection.Open();

                SqlCommand commandd = new SqlCommand("use donaris select id from branches where id = " + id, connection);
                SqlDataReader reader = commandd.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            check = reader.GetInt32(0);
                        else
                            check = 0;

                    }
                }
                commandd.Cancel();
                reader.Close();
                connection.Close();
                if (check ==id)
                {
                    return true;
                }
                else return false;

            }
            else return false;
        }
        private bool checkpassword()
        {
            string adresa = textBox5.Text;
            if (!String.IsNullOrEmpty(adresa))
            {
                return true;
            }
            else return false;
        }
        private bool checklogin()
        {
            string adresa = textBox2.Text;
            if (!String.IsNullOrEmpty(adresa))
            {
                return true;
            }
            else return false;
        }
        private bool checkrole()
        {
            string adresa = comboBox1.Text;
            if (!String.IsNullOrEmpty(adresa))
            {
                return true;
            }
            else return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string nume, prenume, role, login, parola;
            int id=0, idbranch;
            if (!checkname())
            {
                MessageBox.Show("Verificati numele!", "Eroare la nume", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checksurname())
            {
                MessageBox.Show("Verificati prenumele!", "Eroare la prenume", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else if (!checklogin())
            {
                MessageBox.Show("Verificati loginul!", "Eroare la login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkpassword())
            {
                MessageBox.Show("Verificati parola!", "Eroare la parola", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkrole())
            {
                MessageBox.Show("Alegeti una din functii!", "Eroare la functie", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkid())
            {
                MessageBox.Show("Verificati id-ul introdus!", "Eroare la id", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                nume = textBox3.Text;
                prenume= textBox4.Text;
                role = comboBox1.Text;
                login=textBox2.Text;
                parola=textBox5.Text;
                idbranch =Convert.ToInt32(textBox1.Text);
                connection.Open();
                SqlCommand cmd = new SqlCommand("use donaris create login " + login + " with password = '" + parola + "' ;", connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    MessageBox.Show("Introduceti alt login!");
                }
                finally { 
                cmd.Cancel();
                cmd=new SqlCommand("use donaris create user "+ prenume + "_"+nume+" for login "+login,connection); cmd.ExecuteNonQuery();
                cmd.Cancel();
                cmd = new SqlCommand("use donaris alter role "+role+" add member "+ prenume + "_"+nume,connection); cmd.ExecuteNonQuery();
                cmd.Cancel();
                cmd = new SqlCommand("use donaris select max(id) from agents", connection);
                SqlDataReader reader = cmd.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    }

                }

                id++;
                cmd.Cancel();
                reader.Close();
                cmd = new SqlCommand("use donaris insert into agents values("+id+" , '"+prenume+"' , '"+nume+"' , "+idbranch+");",connection);
                cmd.ExecuteNonQuery();
                cmd.Cancel();
                connection.Close();
                MessageBox.Show("Ati adaugat un utilizator nou!");
                }

            }
        }
    }
}
