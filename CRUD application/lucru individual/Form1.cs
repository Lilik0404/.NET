using FontAwesome.Sharp;
using System.Drawing;
using System;
using System.Windows.Forms;
using System.Windows.Media;
using System.Linq;
using System.Runtime.InteropServices;
using RJCodeAdvance.RJControls;
using System.Runtime.Remoting.Channels;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace lucru_individual
{
    public partial class Form1 : Form
    {
        private Size formSize;

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        public string connection="";
        public SqlConnection conection;
       
        private void Form1_Load(object sender, EventArgs e)
        {
            formSize = this.ClientSize;
            

        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCCALCSIZE = 0x0083;//Standar Title Bar - Snap Window
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MINIMIZE = 0xF020; //Minimize form (Before)
            const int SC_RESTORE = 0xF120; //Restore form (Before)
            const int WM_NCHITTEST = 0x0084;//Win32, Mouse Input Notification: Determine what part of the window corresponds to a point, allows to resize the form.
            const int resizeAreaSize = 10;

            #region Form Resize
            // Resize/WM_NCHITTEST values
            const int HTCLIENT = 1; //Represents the client area of the window
            const int HTLEFT = 10;  //Left border of a window, allows resize horizontally to the left
            const int HTRIGHT = 11; //Right border of a window, allows resize horizontally to the right
            const int HTTOP = 12;   //Upper-horizontal border of a window, allows resize vertically up
            const int HTTOPLEFT = 13;//Upper-left corner of a window border, allows resize diagonally to the left
            const int HTTOPRIGHT = 14;//Upper-right corner of a window border, allows resize diagonally to the right
            const int HTBOTTOM = 15; //Lower-horizontal border of a window, allows resize vertically down
            const int HTBOTTOMLEFT = 16;//Lower-left corner of a window border, allows resize diagonally to the left
            const int HTBOTTOMRIGHT = 17;//Lower-right corner of a window border, allows resize diagonally to the right

            ///<Doc> More Information: https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-nchittest </Doc>

            if (m.Msg == WM_NCHITTEST)
            { //If the windows m is WM_NCHITTEST
                base.WndProc(ref m);
                if (this.WindowState == FormWindowState.Normal)//Resize the form if it is in normal state
                {
                    if ((int)m.Result == HTCLIENT)//If the result of the m (mouse pointer) is in the client area of the window
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32()); //Gets screen point coordinates(X and Y coordinate of the pointer)                           
                        Point clientPoint = this.PointToClient(screenPoint); //Computes the location of the screen point into client coordinates                          

                        if (clientPoint.Y <= resizeAreaSize)//If the pointer is at the top of the form (within the resize area- X coordinate)
                        {
                            if (clientPoint.X <= resizeAreaSize) //If the pointer is at the coordinate X=0 or less than the resizing area(X=10) in 
                                m.Result = (IntPtr)HTTOPLEFT; //Resize diagonally to the left
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize))//If the pointer is at the coordinate X=11 or less than the width of the form(X=Form.Width-resizeArea)
                                m.Result = (IntPtr)HTTOP; //Resize vertically up
                            else //Resize diagonally to the right
                                m.Result = (IntPtr)HTTOPRIGHT;
                        }
                        else if (clientPoint.Y <= (this.Size.Height - resizeAreaSize)) //If the pointer is inside the form at the Y coordinate(discounting the resize area size)
                        {
                            if (clientPoint.X <= resizeAreaSize)//Resize horizontally to the left
                                m.Result = (IntPtr)HTLEFT;
                            else if (clientPoint.X > (this.Width - resizeAreaSize))//Resize horizontally to the right
                                m.Result = (IntPtr)HTRIGHT;
                        }
                        else
                        {
                            if (clientPoint.X <= resizeAreaSize)//Resize diagonally to the left
                                m.Result = (IntPtr)HTBOTTOMLEFT;
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize)) //Resize vertically down
                                m.Result = (IntPtr)HTBOTTOM;
                            else //Resize diagonally to the right
                                m.Result = (IntPtr)HTBOTTOMRIGHT;
                        }
                    }
                }
                return;
            }
            #endregion

            //Remove border and keep snap window
            if (m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
            {
                return;
            }

            //Keep form size when it is minimized and restored. Since the form is resized because it takes into account the size of the title bar and borders.
            if (m.Msg == WM_SYSCOMMAND)
            {
                /// <see cref="https://docs.microsoft.com/en-us/windows/win32/menurc/wm-syscommand"/>
                /// Quote:
                /// In WM_SYSCOMMAND messages, the four low - order bits of the wParam parameter 
                /// are used internally by the system.To obtain the correct result when testing 
                /// the value of wParam, an application must combine the value 0xFFF0 with the 
                /// wParam value by using the bitwise AND operator.
                int wParam = (m.WParam.ToInt32() & 0xFFF0);

                if (wParam == SC_MINIMIZE)  //Before
                    formSize = this.ClientSize;
                if (wParam == SC_RESTORE)// Restored form(Before)
                    this.Size = formSize;
            }
            base.WndProc(ref m);
        }
        public Form1()
        {
            InitializeComponent();
                CollapseMenu();
                this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            
        }

        private void iconButton1_Click(object sender, System.EventArgs e)
        {
            Open_DropdownMenu(Statistica, sender);
        }

        private void iconButton2_Click(object sender, System.EventArgs e)
        {
            Open_DropdownMenu(Cautare, sender);
        }

        private void iconButton3_Click(object sender, System.EventArgs e)
        {
            Open_DropdownMenu(Adaugare, sender);
        }

        private void iconButton4_Click(object sender, System.EventArgs e)
        {
            Open_DropdownMenu(Stergere, sender);
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        public string sqlreturnrole()
        {
            conection.Open();
            string role = "";
            SqlCommand commandd = new SqlCommand("use donaris\nSELECT DP1.name\r\n FROM sys.database_role_members AS DRM  \r\n RIGHT OUTER JOIN sys.database_principals AS DP1  \r\n   ON DRM.role_principal_id = DP1.principal_id  \r\n LEFT OUTER JOIN sys.database_principals AS DP2  \r\n   ON DRM.member_principal_id = DP2.principal_id  \r\nWHERE DP1.type = 'R' and DP2.name =USER_NAME()\r\nORDER BY DP1.name desc;", conection);
            SqlDataReader readerr = commandd.ExecuteReader();
            using (readerr)
            {
                while (readerr.Read())
                {
                    if (!readerr.IsDBNull(0))
                        role = readerr.GetString(0);
                    else
                        role = "";
                }
            }
            commandd.Cancel();
            readerr.Close();
            conection.Close();
            return role;
        }
        public string sqlreturnname()
        {
            conection.Open();
            string name = "";
            SqlCommand commandd = new SqlCommand("use donaris select USER_NAME()", conection);
            SqlDataReader reader = commandd.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                        name = reader.GetString(0);
                    else
                        name = "";

                }
            }
            commandd.Cancel();
            reader.Close();
            conection.Close();
            return name;
            
        }
        void checkrole()
        {
            label1.Text = "Donaris Viena Group Buna ziua " + sqlreturnname();
            if (sqlreturnrole() == "lucrator")
            {
                iconButton3.Visible = false;
                iconButton4.Visible = false;

            }
            else if (sqlreturnrole() == "manager")
            {
                iconButton4.Visible = false;
                lucratorToolStripMenuItem.Visible = false;
                filialaToolStripMenuItem.Visible = false;
            }

        }
        private void iconButton6_Click(object sender, EventArgs e)
        {   
            Form2 ehehe= new Form2();
            ehehe.Show();
            this.Close();
            
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void iconButton8_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void iconButton7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            CollapseMenu();
        }

        private void CollapseMenu()
        {
            if (this.panel1.Width > 200)
            {

                this.panel1.Width = 100;
                pictureBox1.Visible = false;
                iconButton5.Dock = DockStyle.Top;
                foreach (Button icbtt in panel1.Controls.OfType<Button>())
                {
                    icbtt.Text = "";
                    icbtt.ImageAlign = ContentAlignment.MiddleCenter;
                    icbtt.Padding = new Padding(0);

                }
            }
            else
            {
                this.panel1.Width = 234;
                pictureBox1.Visible = true;
                iconButton5.Dock = DockStyle.None;
                foreach (Button icbtt in panel1.Controls.OfType<Button>())
                {
                    icbtt.Text = "  " + icbtt.Tag.ToString();
                    icbtt.ImageAlign = ContentAlignment.MiddleCenter;
                    icbtt.Padding = new Padding(10, 0, 0, 0);

                }
            }
        }

        private void iconDropDownButton1_Click(object sender, EventArgs e)
        {

        }
        private void Open_DropdownMenu(RJDropdownMenu dropdownMenu, object sender)
        {
            Control control = (Control)sender;
            dropdownMenu.VisibleChanged += new EventHandler((sender2, ev)
            => DropdownMenu_VisibleChanged(sender2, ev, control));
            dropdownMenu.Show(control, control.Width, 0);
        }
        private void DropdownMenu_VisibleChanged(object sender, EventArgs e, Control ctrl)
        {
            RJDropdownMenu dropdownMenu = (RJDropdownMenu)sender;
            if (!DesignMode)
            {
                if (dropdownMenu.Visible)
                    ctrl.BackColor = System.Drawing.Color.FromArgb(159, 161, 224);
                else ctrl.BackColor = System.Drawing.Color.CornflowerBlue;
            }
        }
        private void Statistica_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        

        private void perZiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();

            Statistica_perzi child = new Statistica_perzi() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(child);
            child.Dock = DockStyle.Fill;

            child.Show();
        }

        private void perLunaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Statistica_perluna child = new Statistica_perluna() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(child);
            child.Show();
        }

        private void perAnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Statistica_peran child = new Statistica_peran() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            child.MdiParent = this;
            panel3.Controls.Add(child);

            child.Show();
        }

        private void cautareContractToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Cautare_contract child = new Cautare_contract() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
 
            panel3.Controls.Add(child);
            child.connection = conection;
            child.Show();
        }

        private void cautareClientIDNPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Cautare_idnp child = new Cautare_idnp() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(child);
            child.connection = conection;
            child.Show();
        }

        private void cautareFilialaIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Cautare_filialaid child = new Cautare_filialaid() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(child);
            child.connection = conection;
            child.Show();
        }

        private void cautareLucratorIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Cautare_lucratorid child = new Cautare_lucratorid() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(child);
            child.connection = conection;
            child.Show();
        }

        private void perZiToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Statistica_perzi child = new Statistica_perzi() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(child);
            child.Show();
            child.con = conection;
        }

        private void perLunaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Statistica_perluna child = new Statistica_perluna() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(child);
            child.Show();
            child.con = conection;
        }

        private void perAnToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Statistica_peran child = new Statistica_peran() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(child);
            child.Show();
            child.con = conection;
            
        }

        private void totalaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Statistica_total child = new Statistica_total() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(child);
            child.Show();
            child.con = conection;
        }

       
        private void contractToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Stergere_contract child = new Stergere_contract() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(child);
            child.connection = conection;
            child.Show();
        }

       
        private void contractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Adaugare_Contract child = new Adaugare_Contract() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(child);
            child.connection = conection;
            child.Show();
        }

        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
        }

        private void lucratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Adaugare_lucrator child = new Adaugare_lucrator() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(child);
            child.connection = conection;
            child.Show();
        }

        private void filialaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();
            Adaugare_filiala child = new Adaugare_filiala() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(child);
            child.connection = conection;
            child.Show();
        }

        private void panel3_Validated(object sender, EventArgs e)
        {
            
        }

        private void panel3_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            
        }

        private void Form1_Enter(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            checkrole();
        }
    }
}
