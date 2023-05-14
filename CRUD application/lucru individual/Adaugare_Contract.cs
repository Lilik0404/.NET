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
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace lucru_individual
{
    public partial class Adaugare_Contract : Form
    {
        public Adaugare_Contract()
        {
            InitializeComponent();
        }
        public SqlConnection connection;
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private bool DigitOnly(string str)
        {
            foreach(char c in str)
            {
                if(c<'0'||c>'9')
                    return false;
            }
            return true;
        }
        private bool checktelefon()
        {
            string telefon = textBox1.Text;
            if(telefon.Length != 9)
            {
                return false;
            }
            else if (!telefon.StartsWith("0"))
            {
                return false;
            }
            else if(!DigitOnly(telefon))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool checkidnp()
        {
            string idnp = textBox5.Text;
            if (idnp.Length == 13)
            {
                string idnpcheck = "";
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select idnp from insured_persons where idnp=" + "'" + idnp + "'", connection);
                SqlDataReader reader = cmd.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        idnpcheck = reader.GetString(0);

                    }

                }
                connection.Close();
                cmd.Cancel();
                reader.Close();
                if (idnpcheck == idnp)
                    return false;
                else
                    return true;
            }
            else return false;
        }
        private bool checknume()
        {
            string nume = textBox3.Text;
            if (!DigitOnly(nume))
            {
                return true;
            }
            else return false;
        }
        private bool checkprenume()
        {
            string nume = textBox4.Text;
            if (!DigitOnly(nume))
            {
                return true;
            }
            else return false;
        }
        private bool checkprocent()
        {
            int procent = 0;
            if (Int32.TryParse(textBox7.Text,out procent))
            {   if (procent > 0)
                    return true;
                else return false;

            }
            else return false;
        }
        private bool checksuma()
        {
            int procent = 0;
            if (Int32.TryParse(textBox6.Text, out procent))
            {   if (procent > 0)
                    return true;
                else return false;

            }
            else return false;
        }
        private bool checkadresa()
        {
            string adresa = textBox2.Text;
            if (!String.IsNullOrEmpty(adresa))
            {
                return true;
            }
            else return false;

        }
        private bool checktip()
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
            if (!checktelefon())
            {
                MessageBox.Show("Verificati numarul de telefon!", "Eroare la numarul de telefon",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checknume())
            {
                MessageBox.Show("Verificati Numele !", "Eroare la Nume", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checktip())
            {
                MessageBox.Show("Selectati tipul asigurarii !", "Eroare la tipul asigurarii", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkprenume())
            {
                MessageBox.Show("Verificati Prenumele !", "Eroare la Prenume", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkprocent())
            {
                MessageBox.Show("Verificati Procentul !", "Eroare la Procent", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else if (!checksuma())
            {
                MessageBox.Show("Verificati Suma !", "Eroare la Suma", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!checkadresa())
            {
                MessageBox.Show("Verificati Adresa !", "Eroare la Adresa", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (checkidnp())
                {
                    int idclient = 0;
                    int idcontract = 0;
                    string nume = textBox3.Text;
                    string prenume = textBox4.Text;
                    string adresa = textBox2.Text;
                    string idnp = textBox5.Text;
                    string telefon = textBox1.Text;
                    string suma = textBox6.Text;
                    DateTime dateTime = DateTime.UtcNow.Date;
                    string procent = textBox7.Text;
                    int branch = 0;
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("use donaris select max(id) from insured_persons", connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            idclient = reader.GetInt32(0);

                        }

                    }
                    idclient++;
                    cmd.Cancel();
                    reader.Close();
                    SqlCommand insertclient = new SqlCommand("use donaris insert into insured_persons values('" + nume + "' , '" + prenume + "' , '" + adresa + "' , '" + idnp + "' , '" + telefon + "');", connection);
                    insertclient.ExecuteNonQuery();
                    insertclient.Cancel();
                    cmd = new SqlCommand("use donaris select max(id) from contracts", connection);
                    reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            idcontract = reader.GetInt32(0);
                        }

                    }

                    idcontract++;
                    cmd.Cancel();
                    reader.Close();
                    cmd = new SqlCommand("use donaris select in_branch from agents where id like user_id()", connection);
                    reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            branch = reader.GetInt32(0);
                        }

                    }
                    cmd.Cancel();
                    reader.Close();

                    SqlCommand insercontract = new SqlCommand("use donaris insert into contracts values(" + idcontract + " , " + idclient + " , " + suma + " , " + "'" + comboBox1.Text + "' , " + branch + " , 1 , user_id() , " + procent + ", '" + dateTime.ToString("yyyy-MM-dd") + "');", connection);
                    insercontract.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Ati adaugat un client si un contract nou!");
                }
                else
                {
                    int idclient = 0;
                    int idcontract = 0;
                    string nume = textBox3.Text;
                    string prenume = textBox4.Text;
                    string adresa = textBox2.Text;
                    string idnp = textBox5.Text;
                    string telefon = textBox1.Text;
                    string suma = textBox6.Text;
                    int userid = 0;
                    DateTime dateTime = DateTime.UtcNow.Date;
                    string year = DateTime.Now.ToString("yyyy");
                    string month = DateTime.Now.ToString("MM");
                    string day = DateTime.Now.ToString("dd");
                    string fulldate = year + "-" + month + "-" + day;
                    string procent = textBox7.Text;
                    int branch = 0;
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("use donaris select id from insured_persons where idnp = " + " '" + idnp + "'", connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            idclient = reader.GetInt32(0);

                        }

                    }

                    cmd.Cancel();
                    reader.Close();
                    cmd = new SqlCommand("Use donaris select max(id) from insured_persons", connection);
                    reader = cmd.ExecuteReader();
                    SqlCommand adduser;
                    if (idclient == 0)
                    {
                        using (reader)
                        {

                            while (reader.Read())
                            {
                                idclient = reader.GetInt32(0);

                            }
                        }
                    
                    adduser = new SqlCommand("use donaris insert into insured_persons values(" + idclient + ",'" + prenume + "','" + nume + "','" + adresa + "','" + idnp + "','" + telefon + "')", connection);
                    adduser.ExecuteNonQuery();
                    }
                cmd.Cancel();
                reader.Close();
                    cmd = new SqlCommand("use donaris select max(id) from contracts", connection);
                    reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            idcontract = reader.GetInt32(0);
                        }

                    }
                    cmd.Cancel();
                    reader.Close();
                    idcontract++;
                    cmd = new SqlCommand("use donaris select in_branch from agents where id like user_id()", connection);
                    reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            branch = reader.GetInt32(0);
                        }

                    }
                    cmd.Cancel();
                    reader.Close();
                    cmd = new SqlCommand("use donaris select user_id()", connection);
                    reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            userid = reader.GetInt16(0);
                        }

                    }
                    cmd.Cancel(); 
                    reader.Close();  
                    SqlCommand insercontract = new SqlCommand("use donaris insert into contracts values(" + idcontract + " , " + idclient + " , " + suma + " , " + "'" + comboBox1.Text + "' , " + branch + " , 1 ,"+userid+" , " + procent + ", '" + fulldate + "');", connection);
                    insercontract.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Ati adaugat un contract nou!");
                }
            }
        }
    }
}
