using System;
using System.IO.Ports;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Diagnostics;

namespace WFApp_Electronic_Scale
{
    public partial class Form1 : Form
    {
        private SerialPort port;

        //SerialPort port = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);

        string logFilePath = "log.txt";
        private string ReadData = "";
        public Form1()
        {
            InitializeComponent();
            InitializeDefaults();
            port = new SerialPort();
            port.DataReceived += Port_DataReceived;

        }

        private void InitializeDefaults()
        {
            cmbParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
            cmbStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));

            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                cmbLetters.Items.Add(letter.ToString());
            }

            //for (char letter = 'أ'; letter <= 'ي'; letter++)
            //{
            //    cmbLetters.Items.Add(letter.ToString());
            //}

            string arabicLetters = "أبجدهوزحطيكلمنسعفصقرشتثخذضظغ";
            foreach (char letter in arabicLetters)
            {
                cmbLetters.Items.Add(letter.ToString());
            }
            cmbParity.SelectedItem = Parity.None.ToString();// "None";
            cmbStopBits.SelectedItem = StopBits.One.ToString();// "One";
            txtPort.Text = "COM3";
            txtBaudRate.Text = "9600";
            txtDataBits.Text = "8";
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                string portName = string.IsNullOrWhiteSpace(txtPort.Text) ? "COM3" : txtPort.Text;

                int baudRate = 9600;
                if (!string.IsNullOrWhiteSpace(txtBaudRate.Text) && int.TryParse(txtBaudRate.Text, out int parsedBaud))
                {
                    baudRate = parsedBaud;
                }

                Parity parity = Parity.None;
                if (Enum.TryParse(cmbParity.SelectedItem.ToString(), out Parity parsedParity))
                {
                    parity = parsedParity;
                }

                int dataBits = 8;
                if (!string.IsNullOrWhiteSpace(txtDataBits.Text) && int.TryParse(txtDataBits.Text, out int parsedBits))
                {
                    dataBits = parsedBits;
                }

                StopBits stopBits = StopBits.One;
                if (Enum.TryParse(cmbStopBits.SelectedItem.ToString(), out StopBits parsedStopBits))
                {
                    stopBits = parsedStopBits;
                }

                port.PortName = portName;
                port.BaudRate = baudRate;
                port.Parity = parity;
                port.DataBits = dataBits;
                port.StopBits = stopBits;
                // MessageBox.Show($"Port Configured: {port.PortName}, {port.BaudRate}, {port.Parity}, {port.DataBits}, {port.StopBits}");

                try
                {
                    //port.ReadTimeout = 3000;
                    //port.ReceivedBytesThreshold = 40;
                    port.Open();
                    Log("port is oppen");
                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();

                    while (string.IsNullOrWhiteSpace(ReadData))
                    {
                        VeriTalepGonder();
                        System.Threading.Thread.Sleep(1000); // cihazın cevap vermesi için bekleme
                    }
                    //Log("Closing port...");
                    //port.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    if (port.IsOpen)
                    {
                        port.Close();
                        Log("Closed port...");
                    }
                }
            }
            catch (Exception ex)
            {
                Log("Error: " + ex.Message);
            }
        }

        private void btnSendTrigger_Click(object sender, EventArgs e)
        {
            try
            {
                char CHR = Convert.ToChar(2);
                string STRG1 = CHR.ToString();
                CHR = Convert.ToChar(1);
                string STRG2 = CHR.ToString();
                string STRG3 = "DNG";
                CHR = Convert.ToChar(13);
                string STRG4 = CHR.ToString();
                if (!string.IsNullOrWhiteSpace(txtCommand.Text))
                {
                    STRG3 = txtCommand.Text;
                }

                //Log($"STX: {Convert.ToInt32(STRG1[0])}, SOH: {Convert.ToInt32(STRG2[0])}, {STRG3}, CR: {Convert.ToInt32(STRG4[0])}");
                //txtWeight.Text = (STRG1 + STRG2 + STRG3 + STRG4);
                //txtWeight.Text = $"STX: {Convert.ToInt32(STRG1[0])}, SOH: {Convert.ToInt32(STRG2[0])}, {STRG3}, CR: {Convert.ToInt32(STRG4[0])}";
                port.Write(STRG1 + STRG2 + STRG3 + STRG4);

                string data = port.ReadExisting();
                // Log("ReadExisting" + port.ReadExisting());
                // Log("ReadBufferSize" + port.ReadBufferSize); // 4096 حجم البفر
                // Log("ReadLine" + port.ReadLine());
                // Log("BytesToRead" + port.BytesToRead);
                // Log("ReceivedBytesThreshold" + port.ReceivedBytesThreshold); // 1 يعني أن الحدث DataReceived سيتم إطلاقه بمجرد استلام أول بايت من البيانات
                // Log("ReadTimeout" + port.ReadTimeout); // -1  انتظر للأبد حتى تصلك البيانات
            }

            catch (Exception EX)
            {
                MessageBox.Show("Error : " + EX.Message);
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (port.IsOpen && port.BytesToRead != 0)
            {
                ReadData = port.ReadExisting();
                string weight = ReadWeight(ReadData);
                Invoke(new Action(() =>
                {
                    txtWeight.Text = weight;
                    Log("received from Port_DataReceived string: " + weight);
                }));
            }
        }
        private string ReadWeight(string weight)
        {
            try
            {
                //foreach (char c in weight)
                //{
                //    Log($"{c} - {(int)c}");
                //}
                //if (port.IsOpen /*&& port.BytesToRead != 0*/)
                //{
                // weight += port.ReadExisting();
                Log("weight:" + weight);
                Log("weight Lenght:" + weight.Length.ToString());

                if (weight == Convert.ToChar(5).ToString())
                {
                    txtWeight.Text = "WRONG WEIGHING!  ";
                    string ilk_satir = "WRONG WEIGHING! ";
                    Thread.Sleep(500);
                    ReadData = "";
                }
                if (weight == Convert.ToChar(6).ToString())
                {
                    Thread.Sleep(500);
                    ReadData = "";
                }
                if (weight.Length >= 41)
                {
                    char[] array = weight.ToCharArray(0, 41);
                    if (array[0] == Convert.ToChar(2) /*&& (array[40] == Convert.ToChar(13) || array[40] == Convert.ToChar(3))*/)
                    {
                        ReadData = weight.Substring(26, 6);
                        txtWeight.Text = ReadData;
                        //ReadData = "";
                        port.Write(Convert.ToChar(4).ToString());
                    }
                }
                // }
                return ReadData.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        private void Log(string message)
        {
            string logEntry = DateTime.Now + " - " + message + "\n";
            txtLog.AppendText(logEntry + Environment.NewLine);
            File.AppendAllText(logFilePath, logEntry);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (port != null && port.IsOpen == true)
            {
                port.Close();
                Log("Port closed.");
            }
        }

        private void VeriTalepGonder()
        {
            try
            {
                char CHR = Convert.ToChar(2);
                string STRG1 = CHR.ToString();
                CHR = Convert.ToChar(1);
                string STRG2 = CHR.ToString();
                string STRG3 = "DNG";
                CHR = Convert.ToChar(13);
                string STRG4 = CHR.ToString();
                port.Write(STRG1 + STRG2 + STRG3 + STRG4);
            }
            catch (Exception EX)
            {
                MessageBox.Show("Error : " + EX.Message);
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            CityLoader cityLoader = new CityLoader(cmbCities);
            //await cityLoader.LoadCitiesAsync();
        }
    }
}
