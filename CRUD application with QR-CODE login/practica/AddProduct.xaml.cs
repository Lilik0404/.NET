using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace practica
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private SqlConnection connection;
        public AddProduct()
        {
            InitializeComponent();
        }
        public AddProduct(SqlConnection connection)
        {
            InitializeComponent();
            this.connection = connection;
        }
        private bool check()
        {   if (CatID_TB.Text.Length < 1 || BranchID_TB.Text.Length < 1 || Denumire_TB.Text.Length < 2 || DataExp_TB.Text.Length < 2 || DataFabr_TB.Text.Length < 2 || Pret_TB.Text.Length < 1)
                return false;
            return true;
        }
        private void Adauga_Click(object sender, RoutedEventArgs e)
        {
            if (check())
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"use market insert into products(CategoryID,BranchID,ProductName,ProductExpiration,ProductFabr,Pret_Initial) values({int.Parse(CatID_TB.Text)},{int.Parse(BranchID_TB.Text)}, '{Denumire_TB.Text}','{DataExp_TB.Text}','{DataFabr_TB.Text}',{float.Parse(Pret_TB.Text)}) ",connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Produsul a fost adaugat!");
                }
                catch (Exception)
                {
                    MessageBox.Show("Eroare! Verificati datele sau contactaci suportul tehnic!");
                }
            }
        }

        private void Iesire_Click(object sender, RoutedEventArgs e)
        {
            workwindow work = new workwindow(connection);
            work.Show();
            this.Close();
        }
    }
}
