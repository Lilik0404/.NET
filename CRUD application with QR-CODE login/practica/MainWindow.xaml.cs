using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AForge.Video.DirectShow;
using ZXing;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.VisualBasic;
using SixLabors.ImageSharp.ColorSpaces.Conversion;

namespace practica
{ 
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        SqlConnection sqlConnection;//Declararea variabilei pentru conectarea la Serverul SQL
        FilterInfoCollection filterInfoCollection;//Declararea variabilei pentru alegerea camerei
        VideoCaptureDevice device;//Declararea variabilei unde va fi stocata informatia din camera
        private int verificationCode;//Declararea variabilei unde va fi stocat codul de verificare
        private string mail="";
        private void setmail(SqlConnection sqlConnection)
        {
            try
            {
                sqlConnection.Open();
                SqlCommand com = new SqlCommand("USE MARKET select EmployMail from employees where EmployID = USER_ID()", sqlConnection);
                using (SqlDataReader reader = com.ExecuteReader())
                    while (reader.Read())
                        mail= reader.GetString(0);
                sqlConnection.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Nu sa putut gasi email contactati serviciul suport tehnic");
                System.Threading.Thread.Sleep(5000);
                this.Close();

            }
            }
        private bool checkmail(string mailaddress)//Functia de verificare a utilizatorului prin intermediului a unei scrisori pe mail
        {
          

            MessageBox.Show("VERIFICARE!");
            Random random = new Random();//Declararea unui obiect de clasa random pentru a creea codul
          verificationCode = random.Next(100000, 999999);//Crearea codului random de 6 cifre
            try
            {
                MailMessage mail = new MailMessage();//Declarearea variabilei de tip mail pentru a putea trimite scrisori
                mail.From = new MailAddress("mail");//Declararea adresei de la care se va transmite
                mail.To.Add(mailaddress);//Adaugarea adresei ce va primi parola
                mail.Subject = "Codul de verificare";//Tema scrisorii
                mail.Body = $"Codul de verificare :{verificationCode}.";//Textul scrisorii
                SmtpClient smtp = new SmtpClient("smtp.client");//Conectarea la serverul SMTP(SIMPLE MAIL TRANSFER PROTOCOL)
                smtp.Port = 587;//Alegerea portului pentru SMTP
                smtp.Credentials = new NetworkCredential("mail", "password");//Introducerea parolei si login pentru SMTP
                smtp.EnableSsl = false;//Criptarea Datelor
                smtp.Send(mail);//Trimiterea scrisorii
                string code = Interaction.InputBox("Introduceti codul", "Verificare Utilizator");//Declaram un InputBox pentru a primi ca valoare codul de la tastatura
                if (code == verificationCode.ToString())//Verficam daca codul este corect
                {
                    return true;
                }
                else return false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la transmitere a codului de verificare: " + ex.Message);
                return false;
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)//Declaram metoda la apasarea butonului Login
        {
            
            String pass = PassWord_Box.Password.ToString(), login = Login_TextBox.Text.ToString();//Declaram variabile in care vor fi stocate textul din TextBox-ri
            if (Login_TextBox.Text.Length < 5)//Verificam daca loginul este corect
            {
                MessageBox.Show("Loginul nu este complectat corect!");
                Login_TextBox.BorderBrush = System.Windows.Media.Brushes.Red;//In cazul in care nu este introdus corect textbox-ul va fi colorat in rosu
            }
            else if (PassWord_Box.Password.Length < 4)//Verificam daca parola este corecta
            {
                MessageBox.Show("Parola nu este complectata corect!");
                PassWord_Box.BorderBrush = System.Windows.Media.Brushes.Red;//In cazul in care nu este introdus corect textbox-ul va fi colorat in rosu
            }

            else
            {
                Login_TextBox.BorderBrush = System.Windows.Media.Brushes.Gray;//In cazul in care este introdus corect textbox-ul va avea culoarea sura
                PassWord_Box.BorderBrush = System.Windows.Media.Brushes.Gray;//In cazul in care este introdus corect textbox-ul va avea culoarea sura
                string firstpart = "RGF0YSBTb3VyY2U9IDEyNy4wLjAuMSwxNDMzXFxTUUxFWFBSRVNTO0luaXRpYWwgQ2F0YWxvZz1NYXJrZXQ7VXNlciBJRD0=";//Declaram partile stringului de conectare criptat la SQl Server
                string secondpart = "O1Bhc3N3b3JkPQ==";//Declaram partile stringului de conectare criptat la SQl Server
                string thirdpart = "O011bHRpcGxlQWN0aXZlUmVzdWx0U2V0cz10cnVlOw==";//Declaram partile stringului de conectare criptat la SQl Server
                string a = Encoding.UTF8.GetString(Convert.FromBase64String(firstpart)) + login + Encoding.UTF8.GetString(Convert.FromBase64String(secondpart)) + pass + Encoding.UTF8.GetString(Convert.FromBase64String(thirdpart));//Cream stringul de conectare din mai multe parti criptate
                if (checkcon(new SqlConnection(a)))//Verificam daca este posibila conectarea la sql cu login si parola noastra
                {
                    setmail(new SqlConnection(a));
                    if (checkmail(mail))//Incepem verificarea utilizatorului prin intermediul la email
                    {
                        sqlConnection = new SqlConnection(a);
                        try
                        {
                            
                            workwindow work = new workwindow(sqlConnection);
                            work.Show();
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Eroare la conectare!"+ex.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Codul introdus este incorect!");
                    }
                }
                else
                {
                    MessageBox.Show("Verificati datele!");
                }
            }
        }

        private void Qr_Button_Click(object sender, RoutedEventArgs e)
        {
            PassWord_Box.Visibility = Visibility.Hidden;
            Login_Button.Visibility = Visibility.Hidden;
            Login_label.Visibility = Visibility.Hidden;
            Login_TextBox.Visibility = Visibility.Hidden;
            Pass_label.Visibility = Visibility.Hidden;
            Qr_Button.Visibility = Visibility.Hidden;
            Logo_image.Visibility = Visibility.Hidden;
            Hello_label.Visibility = Visibility.Hidden;

            devicecombobox.Visibility = Visibility.Visible;
            Iesire_button.Visibility = Visibility.Visible;
            Camera.Visibility = Visibility.Visible;
            Start.Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {   //la incarcare sa fie actualizate toate camerele ce sunt conectate
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach(FilterInfo filterInfo in filterInfoCollection)
            {
                devicecombobox.Items.Add(filterInfo.Name);

            }
            devicecombobox.SelectedIndex = 0;
        }

        private void Iesre_button(object sender, RoutedEventArgs e)
        {
            PassWord_Box.Visibility = Visibility.Visible;
            Login_Button.Visibility = Visibility.Visible;
            Login_label.Visibility = Visibility.Visible;
            Login_TextBox.Visibility = Visibility.Visible;
            Pass_label.Visibility = Visibility.Visible;
            Qr_Button.Visibility = Visibility.Visible;
            Logo_image.Visibility = Visibility.Visible;
            Hello_label.Visibility = Visibility.Visible;

            devicecombobox.Visibility = Visibility.Hidden;
            Iesire_button.Visibility = Visibility.Hidden;
            Camera.Visibility = Visibility.Hidden;
            Start.Visibility = Visibility.Hidden;
            device.Stop();
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri("linella-md.png", UriKind.Relative);
            bi3.EndInit();
            Camera.Source= bi3;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {   
           device=new VideoCaptureDevice(filterInfoCollection[devicecombobox.SelectedIndex].MonikerString);//foloseste ca device cel selectat din combobox
            device.NewFrame += CaptureDevice_NewFrame;
            if (device.IsRunning)
            {
                device.Stop();
                Camera.Source = null;
            }
            device.Start();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        public static Bitmap BitmapFromSource(BitmapSource bitmapsource)//functia ce transforma bitmap in image source
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }

        private bool checkcon(SqlConnection con)//functie ce verifica conexiunea la sql
        {
            try
            {
                con.Open();
                con.Close();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        static string Decrypt(string encryptedMessage)//functia ce decripteaza qr codul citit
        {
            string decryptedMessage = "";
            foreach (char c in encryptedMessage)
            {
                decryptedMessage += (char)(c - 1);
            }
            return decryptedMessage;
        }
        void timer_Tick(object sender, EventArgs e)
        { 
            if (Camera.Source != null)
            {
                BarcodeReader reader = new BarcodeReader();
                Result result = reader.Decode(BitmapFromSource((BitmapSource)Camera.Source));
                if (result != null)
                {
                    string first = "RGF0YSBTb3VyY2U9IDEyNy4wLjAuMSwxNDMzXFxTUUxFWFBSRVNTO0luaXRpYWwgQ2F0YWxvZz1NYXJrZXQ7";
                    string main = Encoding.UTF8.GetString(Convert.FromBase64String(result.ToString()));
                    string second = "TXVsdGlwbGVBY3RpdmVSZXN1bHRTZXRzPXRydWU7";
                    string constring = Encoding.UTF8.GetString(Convert.FromBase64String(first)) + Decrypt(main) + Encoding.UTF8.GetString(Convert.FromBase64String(second));

                    sqlConnection = new SqlConnection(constring);
                    if (checkcon(sqlConnection))
                    {
                        setmail(sqlConnection);
                        if (checkmail(mail))
                        {
                            try
                            {
                                workwindow work = new workwindow(sqlConnection);
                                work.Show();
                                device.Stop();
                                Camera.Source = null;
                                this.Close();
                            }
                            catch (Exception x)
                            {
                                MessageBox.Show("Eroare" + x.ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show("Codul introdus este incorect!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nu este posibila autentificarea!");
                    }
                    
                }
            }
        }

        private void CaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {//functia ce permite generarea din video de pe camera in imagini bitmap
            BitmapImage bi;
            using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
            {   

                BitmapImage bi1 = new BitmapImage();
                bi1.BeginInit();
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bi1.StreamSource = ms;
                bi1.EndInit();
                bi = bi1;
            }
            bi.Freeze(); 
            Dispatcher.BeginInvoke(new ThreadStart(delegate { Camera.Source = bi; }));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {   
            if(device!=null&&device.IsRunning)
                device.Stop();
        }
    }
}
