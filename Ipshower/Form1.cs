using Microsoft.WindowsAPICodePack.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ipshower
{
    public partial class Form1 : Form
    {
        String ips = "";
        String ips2 = "";
        bool isshowall = false;
        public Form1()
        {

            InitializeComponent();
            //notifyIcon1.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon1.Visible = true;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.Text = "Network";
            notifyIcon1.BalloonTipTitle = "Network Monitor Started";
            notifyIcon1.BalloonTipText = "Showing Available Network";
            notifyIcon1.ShowBalloonTip(0);
            //this.Opacity = .98;
            this.BackColor = Color.White;
            this.ForeColor = SystemColors.ControlDark;
            this.Height = 300;
            this.ShowInTaskbar = false;
            this.Width = 300;

            //this.BackColor = Color.Transparent;
            this.TopMost = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            //this.Left = resolution.Width - 150;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(resolution.Width - 310, 0 + 10);
            SetNetworkResult();
            //this.WindowState = FormWindowState.Maximized;
            this.Paint += Watermark_Paint;




            NetworkChange.NetworkAddressChanged += new
                 NetworkAddressChangedEventHandler(AddressChangedCallback);
            //notifyIcon1.Visible = true;
            //notifyIcon1.ShowBalloonTip(10000);

            Console.WriteLine("Listening for address changes. Press any key to exit.");
            Console.ReadLine();
        }

        void SetNetworkResult()
        {
            var sConnected = "";
            var networks = NetworkListManager.GetNetworks(NetworkConnectivityLevels.Connected);
            foreach (var network in networks)
            {
                sConnected = ((network.IsConnected == true) ? " (connected)" : " (disconnected)");
                ips2 += "Network : " + network.Name + "\n Category : " + network.Category.ToString() + sConnected + "\n";
                Console.WriteLine(ips);
            }


            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                {

                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        if (nic.Name == "Wi-Fi" || nic.Name == "Ethernet")

                            ips += nic.Name + "  " + nic.OperationalStatus + "\n" + ip.Address.ToString() + "\n";
                        Console.WriteLine("   {0} is {1},{2}", nic.Name, nic.OperationalStatus, ip.Address.ToString());


                    }
                }
            }
        }
        public void AddressChangedCallback(object sender, EventArgs e)
        {

        }

        private void Watermark_Paint(object sender, PaintEventArgs e)
        {

            if (isshowall)

                e.Graphics.DrawString(ips, new Font("tahoma", 10, FontStyle.Bold), Brushes.Green, 0, 0);
            else

            if (value != "")
            {
                // play with this drawing code to change your "watermark"
                SizeF szF = e.Graphics.MeasureString(ips + ips2, new Font("Segoe UI", 15, FontStyle.Bold));
                //e.Graphics.RotateTransform(-90);

                this.Height = (int)szF.Height;
                e.Graphics.DrawString(ips, new Font("tahoma", 15, FontStyle.Bold), Brushes.Green, 0, 0);
                e.Graphics.DrawString(ips2, new Font("tahoma", 10, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, Color.Green)), 0, 100);
                //    for (int y = 0; y <= max; y = y + ((int)szF.Height))
                //    {
                //        e.Graphics.DrawString(ips, new Font("tahoma", 10, FontStyle.Bold), Brushes.Red, 0, y);
                //}
            }

        }

        private void Form1_MaximumSizeChanged(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            //this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void hIdeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void showAllNetworkDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isshowall = true;
            ips = "";
            ips2 = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                {

                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        //if (nic.Name == "Wi-Fi" || nic.Name == "Ethernet")

                        ips += nic.Name + "  " + nic.OperationalStatus + "\n" + ip.Address.ToString() + "\n";
                        Console.WriteLine("   {0} is {1},{2}", nic.Name, nic.OperationalStatus, ip.Address.ToString());


                    }
                }
            }
            // play with this drawing code to change your "watermark"

            SizeF szF = this.CreateGraphics().MeasureString(ips, new Font("Segoe UI", 10, FontStyle.Bold));
            this.Height = (int)szF.Height;

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            isshowall = false;
            ips = "";
            ips2 = "";
            SetNetworkResult();
            this.Invoke((MethodInvoker)delegate
            {
                SizeF szF = this.CreateGraphics().MeasureString(ips + ips2, new Font("Segoe UI", 10, FontStyle.Bold));
                this.Height = (int)szF.Height;
                this.Invalidate();
            });
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)ips);
            notifyIcon1.BalloonTipTitle = "Copy to Clip";
            notifyIcon1.BalloonTipText = ips;
            notifyIcon1.ShowBalloonTip(0);
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var CON = ((System.Windows.Forms.ToolStripControlHost)sender).Text;
            if (CON == "NONE")
                this.Opacity = 1;
            else if (CON == "10")
                this.Opacity = .1;
            else if (CON == "25")
                this.Opacity = .25;
            else if (CON == "50")
                this.Opacity = .50;
            else if (CON == "75")
                this.Opacity = .75;
            else
                this.Opacity = 1;

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            isshowall = true;
            ips = "";
            ips2 = "";
            int[] ports = { 1433, 3306, 3389, 8069, 80, 1434 };
            foreach (var item in ports)
            {
                using (TcpClient tcpClient = new TcpClient())
                {



                    var port = item;
                    try
                    {
                        var ip = "127.0.0.1";

                        tcpClient.Connect(ip, port);
                        ips += "localhost    " + port.ToString() + "     open \n";
                        Console.WriteLine("Port open");
                    }
                    catch (Exception)
                    {
                        ips += "localhost    " + port.ToString() + "     Closed \n";
                        Console.WriteLine("Port closed");
                    }
                }
            }
            this.Invoke((MethodInvoker)delegate
            {
                SizeF szF = this.CreateGraphics().MeasureString(ips, new Font("Segoe UI", 10, FontStyle.Bold));
                this.Height = (int)szF.Height;
            });
        }

        private void toolStripTextBox1_Enter(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                isshowall = true;
                ips = "";
                ips2 = "";
                var text = toolStripTextBox1.Text;
                var arr = text.Split(';');
                try
                {
                    var ip = arr[0];
                    var port = Int32.Parse(arr[1]);



                    using (TcpClient tcpClient = new TcpClient())
                    {




                        try
                        {


                            tcpClient.Connect(ip, port);
                            ips += $"{ip}    " + port.ToString() + "     open \n";
                            Console.WriteLine("Port open");
                        }
                        catch (Exception)
                        {
                            ips += $"{ip}     " + port.ToString() + "     Closed \n";
                            Console.WriteLine("Port closed");
                        }
                    }

                    this.Invoke((MethodInvoker)delegate
                    {
                        SizeF szF = this.CreateGraphics().MeasureString(ips, new Font("Segoe UI", 10, FontStyle.Bold));
                        this.Height = (int)szF.Height;
                    });
                }
                catch (Exception es)
                {

                }
            }
        }
    }


    //public partial class Form1 : Form
    //{

    //    public String value = "Idle_Mind"; // set this somehow
    //    private const int WS_EX_TRANSPARENT = 0x20;

    //    //public Watermark()
    //    //{

    //    //}

    //    // this makes the form ignore all clicks, so it is "passthrough"
    //    protected override System.Windows.Forms.CreateParams CreateParams
    //    {
    //        get
    //        {
    //            CreateParams cp = base.CreateParams;
    //            cp.ExStyle = cp.ExStyle | WS_EX_TRANSPARENT;
    //            return cp;
    //        }
    //    }


    //}

}
