using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using WFApp_Electronic_Scale.Properties;

namespace WFApp_Electronic_Scale
{
    public class UHF
    {
        static SerialPort _serialPort;
        static byte[] buffer = new byte[0];
        static readonly byte[] FRAME_START = { 0x43, 0x54 }; // "CT"
        const int FRAME_LENGTH = 24;
        public static event Action<int> OnTagReceived; // حدث جديد
        private static string settingFilePath = "setting.json";
        private static int _lastTagId = -1; // متغير لتخزين آخر قيمة تم قراءتها
        private static DateTime _lastReadTime = DateTime.MinValue;
        private static readonly TimeSpan _readInterval = TimeSpan.FromSeconds(1); // فترة زمنية بين القراءات


        // دالة لتهيئة المنفذ التسلسلي
        public static void Init()
        {
            _serialPort = new SerialPort();
            InitializeSettingsFile();
            GetSettingFromFile();
            _serialPort.Open();
            _serialPort.DataReceived += DataReceivedHandler;
        }

        // دالة لإغلاق المنفذ
        public static void Close()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }
        private static void GetSettingFromFile()
        {
            try
            {
                if (File.Exists(settingFilePath))
                {
                    string json = File.ReadAllText(settingFilePath);
                    var settings = JsonConvert.DeserializeObject<SettingsModel>(json);

                    string portName = string.IsNullOrWhiteSpace(settings.PortName) ? "COM5" : settings.PortName;

                    int baudRate = 115200;
                    if (!string.IsNullOrWhiteSpace(settings.BaudRate) && int.TryParse(settings.BaudRate, out int parsedBaud))
                    {
                        baudRate = parsedBaud;
                    }

                    Parity parity = Parity.None;
                    if (Enum.TryParse(settings.Parity, out Parity parsedParity))
                    {
                        parity = parsedParity;
                    }

                    int dataBits = 8;
                    if (!string.IsNullOrWhiteSpace(settings.DataBits) && int.TryParse(settings.DataBits, out int parsedBits))
                    {
                        dataBits = parsedBits;
                    }

                    StopBits stopBits = StopBits.One;
                    if (Enum.TryParse(settings.StopBits, out StopBits parsedStopBits))
                    {
                        stopBits = parsedStopBits;
                    }

                    _serialPort.PortName = portName;
                    _serialPort.BaudRate = baudRate;
                    _serialPort.Parity = parity;
                    _serialPort.DataBits = dataBits;
                    _serialPort.StopBits = stopBits;


                    //Console.WriteLine("Listening to serial port...");
                    // MessageBox.Show($"Port Configured: {_serialPort.PortName}, {_serialPort.BaudRate}, {_serialPort.Parity}, {_serialPort.DataBits}, {_serialPort.StopBits}");
                }
                else
                {
                    MessageBox.Show("ملف الاعدادات غير موجود!", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public static void InitializeSettingsFile()
        {
            if (!File.Exists(settingFilePath))
            {
                _serialPort = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);

                var setting = new SettingsModel
                {
                    ApiUrl = "https://stage-masarak.frappe.cloud/api/method/get_trip_by_tag_id",
                    Username = "f39a13dc264037d",
                    Password = "42ec5dcfc6e0d58",
                    PortName = "COM5",
                    BaudRate = "115200",
                    Parity = "0",
                    DataBits = "8",
                    StopBits = "1"
                };

                string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
                File.WriteAllText(settingFilePath, json);
            }
        }
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            int bytesToRead = _serialPort.BytesToRead;
            byte[] incomingData = new byte[bytesToRead];
            _serialPort.Read(incomingData, 0, bytesToRead);

            // Append to buffer
            byte[] newBuffer = new byte[buffer.Length + incomingData.Length];
            Buffer.BlockCopy(buffer, 0, newBuffer, 0, buffer.Length);
            Buffer.BlockCopy(incomingData, 0, newBuffer, buffer.Length, incomingData.Length);
            buffer = newBuffer;

            while (buffer.Length >= FRAME_LENGTH)
            {
                int startIdx = IndexOf(buffer, FRAME_START);
                if (startIdx == -1)
                {
                    buffer = new byte[0];
                    return;
                }

                if (buffer.Length >= startIdx + FRAME_LENGTH)
                {
                    byte[] frame = new byte[FRAME_LENGTH];
                    Array.Copy(buffer, startIdx, frame, 0, FRAME_LENGTH);

                    Console.WriteLine("🧱 Frame: " + BitConverter.ToString(frame).Replace("-", " "));

                    byte[] tagRaw = new byte[4];
                    Array.Copy(frame, 18, tagRaw, 0, 4);

                    byte[] tagBytes = tagRaw[3] == 0x00
                        ? new[] { tagRaw[0], tagRaw[1], tagRaw[2] }
                        : tagRaw;

                    int tagId = tagBytes.Length == 3
                        ? (tagBytes[0] << 16) | (tagBytes[1] << 8) | tagBytes[2]
                        : BitConverter.ToInt32(tagBytes, 0);


                    // التحقق من تغير القيمة والفاصل الزمني
                    if (tagId != _lastTagId && (DateTime.Now - _lastReadTime) > _readInterval)
                    {
                        _lastTagId = tagId;
                        _lastReadTime = DateTime.Now;

                        // إطلاق الحدث مع قيمة الـ Tag ID
                        OnTagReceived?.Invoke(tagId);

                        Console.WriteLine("🆔 Tag Hex: " + BitConverter.ToString(tagBytes).Replace("-", ""));
                        Console.WriteLine("🆔 Tag ID: " + tagId);
                    }
                    // Trim the buffer
                    int remaining = buffer.Length - (startIdx + FRAME_LENGTH);
                    byte[] newBuf = new byte[remaining];
                    Array.Copy(buffer, startIdx + FRAME_LENGTH, newBuf, 0, remaining);
                    buffer = newBuf;

                }
                else
                {
                    break;
                }
            }
        }

        private static int IndexOf(byte[] buffer, byte[] pattern)
        {
            for (int i = 0; i <= buffer.Length - pattern.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (buffer[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return i;
            }
            return -1;
        }
    }
}