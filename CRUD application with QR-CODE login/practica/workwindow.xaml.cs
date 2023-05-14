using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Win32;
using Excel = Microsoft.Office.Interop.Excel;
using MessageBox = System.Windows.MessageBox;

namespace practica
{
    /// <summary>
    /// Логика взаимодействия для workwindow.xaml
    /// </summary>
    public partial class workwindow : Window
    {
        private SqlConnection connection;
        public workwindow()
        {
            InitializeComponent();
        }
        private DataTable dt = new DataTable("products");//crearea tabelului unde vom afisa datele
        private SqlDataAdapter sda;//adapter pentru a primi datele de pe sql server
        public workwindow(SqlConnection connection)
        {
            this.connection = connection;//setarea conexiunii
            InitializeComponent();
            afisareproduse(this.connection);//apelam functia pentru a afisa datele

        }
        private void afisareproduse(SqlConnection connection)//functia de afisare produse
        {
            try
            {
                connection.Open();//deschidem conexiunea
                String CmdString = "USE Market select * from products where BranchID=(select BranchID from employees where EmployID =user_id()) ";//introducem aceasta pentru a primi produsele din filiale in care este utilizatorul cu care ne-am logat
                SqlCommand cmd = new SqlCommand(CmdString, connection);//cream comanda
                sda = new SqlDataAdapter(cmd);
                dt.Clear();//curatim tabelul
                sda.Fill(dt);//introducem datele in tabel
                DataGridMy.ItemsSource = dt.DefaultView;//Setam cum va fi afisat totul
                connection.Close();//dupa ce am terminat inchidem conexiunea
            }
            catch (Exception)
            {
                MessageBox.Show("A aparut o eraore");
            }
        }
        private void actualizareproduse(SqlConnection connection)//functia de actualizare a produselor
        {
            try
            {
                connection.Open();//deschidem conexiunea
                SqlCommand cmd = new SqlCommand("USE MARKET exec updateproducsts", connection);//declaram comanda ce va executa o procedura
                cmd.ExecuteNonQuery();//executam comanda
                connection.Close();//inchidem conexiunea
                MessageBox.Show("Produsele au fost actualizate");
            }
            catch (Exception)
            {
                MessageBox.Show("A aparut o eraore");
            }
        }
        private void showexprproduse(SqlConnection connection)//functia pentru a afisa produsele cu termenul expirat
        {
            try
            {
                connection.Open();//deschidem conexiunea
                String CmdString = "USE Market Select * from products where Pret_Actual = 0 and BranchID=(select BranchID from employees where EmployID =user_id())";//declaram comanda
                SqlCommand cmd = new SqlCommand(CmdString, connection);
                sda = new SqlDataAdapter(cmd);
                dt.Clear();
                sda.Fill(dt);
                DataGridMy.ItemsSource = dt.DefaultView;
                connection.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("A aparut o eraore");
            }

        }
        private void showproduse50(SqlConnection connection)//functia pentru a afisa produsele cu reducere de 50% in ordinea crescatoare a preturilor initiale
        {
            try
            {
                connection.Open();//deschidem conexiunea
                String CmdString = "USE Market Select * from products where Pret_Actual = Pret_Initial/2 and BranchID=(select BranchID from employees where EmployID =user_id()) order by Pret_Initial asc";//declaram stringul de comanda
                SqlCommand cmd = new SqlCommand(CmdString, connection);
                sda = new SqlDataAdapter(cmd);
                dt.Clear();
                sda.Fill(dt);
                DataGridMy.ItemsSource = dt.DefaultView;
                connection.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("A aparut o eraore");
            }
        }
        private void showproduse20(SqlConnection connection)//functia pentru a afisa produsele cu reducere de 20% in ordinea crescatoare a preturilor actuale
        {
            try
            {
                connection.Open();//deschidem conexiunea
                String CmdString = "USE Market Select * from products where Pret_Actual = Pret_Initial*0.8 and BranchID=(select BranchID from employees where EmployID =user_id()) order by Pret_Actual asc";//declaram stringul de comanda
                SqlCommand cmd = new SqlCommand(CmdString, connection);
                sda = new SqlDataAdapter(cmd);
                dt.Clear();
                sda.Fill(dt);
                DataGridMy.ItemsSource = dt.DefaultView;
                connection.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("A aparut o eraore");
            }
        }
        private void countshowmin1an(SqlConnection connection)//functia ce afiseaza produsele ce au cel putin 1 an ca termen valabil
        {
            try
            {
                int num = 0;
                connection.Open();//deschidem conexiunea
                String CmdString = "USE Market select count(ProductID) from products where DATEDIFF(DAY, GETDATE(), ProductExpiration)>=365 and BranchID=(select BranchID from employees where EmployID =user_id())";//declaram stringul de comanda
                SqlCommand cmd = new SqlCommand(CmdString, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    num = reader.GetInt32(0);//aflam numarul de produse
                }
                reader.Close();
                MessageBox.Show("Numarul de produse:" + num);
                cmd = new SqlCommand("USE Market select * from products where DATEDIFF(DAY, GETDATE(), ProductExpiration)>=365 and BranchID=(select BranchID from employees where EmployID =user_id())", connection);
                sda = new SqlDataAdapter(cmd);
                dt.Clear();
                sda.Fill(dt);
                DataGridMy.ItemsSource = dt.DefaultView;//afisam produsele
                connection.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("A aparut o eroare");
            }
        }
        private void showmax1luna(SqlConnection connection)//functia ce va afisa produsele cu termenul de expirare de 1 luna max
        {
            try
            {
                connection.Open();//deschidem conexiunea
                String CmdString = "USE Market select * from products where DATEDIFF(MONTH, GETDATE(), ProductExpiration)<=1 and Pret_Actual >0 and BranchID=(select BranchID from employees where EmployID =user_id())";
                SqlCommand cmd = new SqlCommand(CmdString, connection);
                sda = new SqlDataAdapter(cmd);
                dt.Clear();
                sda.Fill(dt);
                DataGridMy.ItemsSource = dt.DefaultView;
                connection.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("A aparut o eraore");
            }
        }
        private void showmax5days(SqlConnection connection)//functia ce va afisa produsele cu termenul de expirare de 5 zile max
        {
            try
            {
                connection.Open();//deschidem conexiunea
                String CmdString = "USE Market select * from products where DATEDIFF(DAY, GETDATE(), ProductExpiration)<=5 and Pret_Actual >0 and BranchID=(select BranchID from employees where EmployID =user_id())";
                SqlCommand cmd = new SqlCommand(CmdString, connection);
                sda = new SqlDataAdapter(cmd);
                dt.Clear();
                sda.Fill(dt);
                DataGridMy.ItemsSource = dt.DefaultView;
                connection.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("A aparut o eraore");
            }
        }
        private void delete(SqlConnection connection,string id)//functia ce va sterge produsele dorite
        {
            try
            {
                connection.Open();//deschidem conexiunea
                SqlCommand cmd = new SqlCommand("use market Delete from products where productid = " + id, connection);
                cmd.ExecuteNonQuery();
                cmd.Cancel();
                connection.Close();

                MessageBox.Show("Produsul a fost sters!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare!"+ex.ToString());
            }
        }
        private void AfisareTotala_Button_Click(object sender, RoutedEventArgs e)
        {
            afisareproduse(connection);
        }

        private void Inregistrare_Button_Click(object sender, RoutedEventArgs e)
        {
            AddProduct add = new AddProduct(connection);
            add.Show();
            this.Close();
        }

        private void Actualizare_Button_Click(object sender, RoutedEventArgs e)
        {
            actualizareproduse(connection);
        }

        private void ProduseExpirate_Button_Click(object sender, RoutedEventArgs e)
        {
            actualizareproduse(connection);
            showexprproduse(connection);

        }

        private void _50Crescator_Button_Click(object sender, RoutedEventArgs e)
        {
            showproduse50(connection);
        }

        private void _20Crescator_Button_Click(object sender, RoutedEventArgs e)
        {
            showproduse20(connection);
        }

        private void CountAn_Button_Click(object sender, RoutedEventArgs e)
        {
            countshowmin1an(connection);
        }

        private void MaxVal1luna_Button_Click(object sender, RoutedEventArgs e)
        {
            showmax1luna(connection);
        }

        private void ExpirateExport_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {   //codul de salvare a datelor in excel si dupa stergerea lor
                connection.Open();//deschidem conexiunea
                String CmdString = "USE Market Select * from products where Pret_Actual = 0 and BranchID=(select BranchID from employees where EmployID =user_id())";
                SqlCommand cmd = new SqlCommand(CmdString, connection);
                sda = new SqlDataAdapter(cmd);
                dt.Clear();
                sda.Fill(dt);
                DataGridMy.ItemsSource = dt.DefaultView;
                connection.Close();
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();//declaram dialogul de salvare a fisierului
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    Excel.Application excel = new Excel.Application();//declaram documentul excel
                    Excel.Workbook workbook = excel.Workbooks.Add(Type.Missing);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;//declaram fereastra de lucru

                    int headerIndex = 1;
                    foreach (DataColumn column in dt.Columns)
                    {
                        worksheet.Cells[1, headerIndex] = column.ColumnName;//introducem denumirele in excel
                        headerIndex++;
                    }


                    int rowIndex = 2;
                    foreach (DataRow row in dt.Rows)
                    {
                        int columnIndex = 1;
                        foreach (DataColumn column in dt.Columns)
                        {
                            worksheet.Cells[rowIndex, columnIndex] = row[column.ColumnName];//introducem datele
                            columnIndex++;
                        }
                        rowIndex++;
                    }


                    workbook.SaveAs(saveFileDialog.FileName);//salvam fisierul
                    workbook.Close();//inchidem fisierul
                    excel.Quit();
                     CmdString = "USE Market delete  from products where Pret_Actual = 0 and BranchID=(select BranchID from employees where EmployID =user_id())";
                     cmd = new SqlCommand(CmdString, connection);
                    cmd.ExecuteNonQuery();//stergem toate produsele expirate dupa exportarea lor in sql
                    MessageBox.Show("Datele au fost exportate.");
                }
            }
            catch(Exception ex) {
                MessageBox.Show("Eroare:" + ex.ToString());
                    }
        }

        private void Max5Zile_Button_Click(object sender, RoutedEventArgs e)
        {
            showmax5days(connection);
        }

        private void RaportPerCategorie_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Sterge_Button_Click(object sender, RoutedEventArgs e)
        {   //cod ce permite stergerea produsului selectat din datagridview
            DataRowView dataRowView = (DataRowView)DataGridMy.SelectedItem;
            String ID = Convert.ToString(dataRowView.Row[0]);
            MessageBox.Show(ID);
            delete(connection, ID);
            
        }
        private void Iesire_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
