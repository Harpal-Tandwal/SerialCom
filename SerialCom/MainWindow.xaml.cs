using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Diagnostics;

namespace SerialCom
{
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort serialPort = new SerialPort();
        private string data_out;


        public MainWindow()
        {
            InitializeComponent();

            serialPort.DataReceived += SerialPort_DataReceived;
            string[] ports = SerialPort.GetPortNames();
            string[] baud_rates = { "9600", "115200" };
            string[] data_bits = { "6", "7", "8" };
            string[] stop_bits = { "One", "Two" };
            string[] parity_bits = { "None", "Odd", "Even" };

            port_cBox.ItemsSource = ports;
            baud_rate_cBox.ItemsSource = baud_rates;
            data_bit_cBox.ItemsSource = data_bits;
            stop_bit_cBox.ItemsSource = stop_bits;
            parity_bit_cBox.ItemsSource = parity_bits;
            btn_close.IsEnabled = false;
            //Column2.Visibility = Visibility.Collapsed;


        }
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string[] ports = SerialPort.GetPortNames();
            port_cBox.ItemsSource = ports;
            if (ports.Length > 0)
            {
                Debug.WriteLine("Available serial ports:");
                foreach (string portName in ports)
                {
                    Debug.WriteLine(portName);

                }
            }
            else
            {
                MessageBox.Show("No serial port available.Please Connect your device");
            }


        }

        private void btn_open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                serialPort.PortName = port_cBox.Text;


                serialPort.BaudRate = Convert.ToInt32(baud_rate_cBox.Text);
                serialPort.DataBits = Convert.ToInt32(data_bit_cBox.Text);
                serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stop_bit_cBox.Text);

                serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), parity_bit_cBox.Text);


                serialPort.Open();

                progress_br.Value = 100;
                tb_data_send.Background = Brushes.Green;
                tb_data_send.Foreground = Brushes.White;
                Column2.Visibility = Visibility.Visible;
                btn_open.IsEnabled = false;
                btn_close.IsEnabled = true;



            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                Console.WriteLine($"Serial port  closed.");
                progress_br.Value = 0;
                tb_data_send.Background = Brushes.White;
                tb_data_send.Foreground = Brushes.Black;
                Column2.Visibility = Visibility.Collapsed;
                btn_open.IsEnabled = true;
                btn_close.IsEnabled = false;
            }
            else
            { MessageBox.Show("{serilPort.Name} is closed"); }
        }

        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            try {
                if(serialPort.IsOpen) 
                {
                    serialPort.WriteLine(tb_data_send.Text);
                        tb_data_send.Clear();
                        }
            
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
          
            string data = serialPort.ReadLine();
            Dispatcher.Invoke(() => tb_data_receive.AppendText(data + Environment.NewLine));
            //MessageBox.Show("data received");
        }
    }
}