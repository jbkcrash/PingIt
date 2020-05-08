using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Media.Media3D;
using System.IO;

namespace PingIt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    
 
    public partial class MainWindow : Window
    {
        DispatcherTimer pingTimer = new DispatcherTimer();
        IPAddress ipAddressCustom = IPAddress.Loopback;

        public void AddLine(string text)
        {
            outputBox.AppendText(text);
            outputBox.AppendText("\u2028"); // Linebreak, not paragraph break
            outputBox.ScrollToEnd();
        }
        public void PingIPv4(string ipAddress)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = this.data.Text; // "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send(ipAddressCustom, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                AddLine(String.Format("Address: {0}", reply.Address.ToString()));
                AddLine(String.Format("RoundTrip time: {0}", reply.RoundtripTime));
                AddLine(String.Format("Time to live: {0}", reply.Options.Ttl));
                AddLine(String.Format("Don't fragment: {0}", reply.Options.DontFragment));
                AddLine(String.Format("Buffer size: {0}", reply.Buffer.Length));
            }
        }
        //fe80::d0a7:a507:5e23:d61c%18

        public void PingIPv6(string ipAddress)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = this.data.Text;
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            try
            {
                PingReply reply = pingSender.Send(ipAddressCustom, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    AddLine(String.Format("Address: {0}", reply.Address.ToString()));
                    AddLine(String.Format("RoundTrip time: {0}", reply.RoundtripTime));
                    //AddLine(String.Format("Time to live: {0}", reply.Options.Ttl));
                    //AddLine(String.Format("Don't fragment: {0}", reply.Options.DontFragment));
                    AddLine(String.Format("Buffer size: {0}", reply.Buffer.Length));
                }
            } catch (System.Net.NetworkInformation.PingException e)
            {
                AddLine(e.Message);

            }
        }


        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //First thing it so evaluate the address
            ipAddressCustom = IPAddress.Parse(ipaddress.Text);

            //ipaddress.Text = ipAddressCustom.ToString();

            if(continuous.IsChecked == false) // Not doing Continuous ping.
            {
                if (ipv4.IsChecked == true) // Doing IPv4
                {
                    PingIPv4(ipaddress.Text); // Do one Ping...

                }
                else
                {
                    PingIPv6(ipaddress.Text);
                }
            } 
            else // Now need to do a continuous ping, based on rate
            {
                int rate = 1000;
                // Lets get rate
                if (_10ms.IsChecked == true)
                {
                    rate = 10;
                }
                if (_100ms.IsChecked == true)
                {
                    rate = 100;
                }

                pingTimer.Interval = TimeSpan.FromMilliseconds(rate); // Set our rate
                pingTimer.Tick += timer_Tick;
                pingTimer.Start();
                stop.IsEnabled = true;
                ping.IsEnabled = false;
            }




           // if(ipv6.IsChecked == true)
          //  {
           //     MessageBox.Show("IPv6");
          //  }
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (ipv4.IsChecked == true)
            {
                PingIPv4(ipaddress.Text);

            }
            else
            {
                PingIPv6(ipaddress.Text);
            }
        }

        private void continuous_Checked(object sender, RoutedEventArgs e)
        {
            _10ms.IsEnabled = true;
            _100ms.IsEnabled = true;
            _1000ms.IsEnabled = true; 
        }
        private void continuous_Unchecked(object sender, RoutedEventArgs e)
        {
            _10ms.IsEnabled = false;
            _100ms.IsEnabled = false;
            _1000ms.IsEnabled = false;
        }

        private void stop_clicked(object sender, RoutedEventArgs e)
        {
            pingTimer.Stop();
            stop.IsEnabled = false;
            ping.IsEnabled = true;
        }

        private void ProcessRandom()
        {
            //TODO create random string, 32bit.
            if (_32char.IsChecked == true)
            {
                data.Text = RandomString(32);
            }
            if (_64char.IsChecked == true)
            {
                data.Text = RandomString(64);
            }
            if (mtu.IsChecked == true)
            {
                data.Text = RandomString(1454);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _32char.IsEnabled = true;
            _64char.IsEnabled = true;
            mtu.IsEnabled = true;

            ProcessRandom();
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            data.Text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            _32char.IsEnabled = false;
            _32char.IsChecked = true;
            _64char.IsEnabled = false;
            mtu.IsEnabled = false;
        }

        private void _64char_Checked(object sender, RoutedEventArgs e)
        {
            ProcessRandom();
        }

        private void mtu_Checked(object sender, RoutedEventArgs e)
        {
            ProcessRandom();
        }

        private void ipv6_Checked(object sender, RoutedEventArgs e)
        {
            ipAddressCustom = IPAddress.IPv6Loopback;
            this.ipaddress.Text = ipAddressCustom.ToString();
        }

        private void ipv4_Checked(object sender, RoutedEventArgs e)
        {
            ipAddressCustom = IPAddress.Loopback;
            if(this.ipaddress != null)
            {
                this.ipaddress.Text = ipAddressCustom.ToString();
            }
        }
    }
}
