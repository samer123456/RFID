using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using WFApp_Electronic_Scale.Properties;

namespace WFApp_Electronic_Scale
{
    public class UHF : IDisposable
    {
        private static SerialPort _serialPort;
        private static readonly List<byte> _buffer = new List<byte>(); // Use List instead of array for better performance
        private static readonly byte[] FRAME_START = { 0x43, 0x54 }; // "CT"
        private const int FRAME_LENGTH = 24;
        private const int BUFFER_MAX_SIZE = 1024; // Prevent buffer from growing too large
        public static event Action<int> OnTagReceived;
        private static readonly string settingFilePath = "setting.json";
        private static int _lastTagId = -1;
        private static DateTime _lastReadTime = DateTime.MinValue;
        private static readonly TimeSpan _readInterval = TimeSpan.FromSeconds(1);
        private static bool _disposed = false;
        private static readonly object _lockObject = new object();


        // دالة لتهيئة المنفذ التسلسلي
        public static void Init()
        {
            lock (_lockObject)
            {
                if (_disposed) return;

                _serialPort = new SerialPort();
                InitializeSettingsFile();
                GetSettingFromFile();
                _serialPort.Open();
                _serialPort.DataReceived += DataReceivedHandler;
            }
        }

        // دالة لإغلاق المنفذ
        public static void Close()
        {
            lock (_lockObject)
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    _serialPort.DataReceived -= DataReceivedHandler;
                    _serialPort.Close();
                    _serialPort.Dispose();
                    _serialPort = null;
                }
                _buffer.Clear();
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
            lock (_lockObject)
            {
                if (_disposed || _serialPort == null || !_serialPort.IsOpen) return;

                int bytesToRead = _serialPort.BytesToRead;
                if (bytesToRead == 0) return;

                byte[] incomingData = new byte[bytesToRead];
                _serialPort.Read(incomingData, 0, bytesToRead);

                // Append to buffer - prevent buffer from growing too large
                if (_buffer.Count + incomingData.Length > BUFFER_MAX_SIZE)
                {
                    _buffer.Clear(); // Clear buffer if it gets too large
                }
                _buffer.AddRange(incomingData);

                while (_buffer.Count >= FRAME_LENGTH)
                {
                    int startIdx = IndexOf(_buffer, FRAME_START);
                    if (startIdx == -1)
                    {
                        _buffer.Clear();
                        return;
                    }

                    if (_buffer.Count >= startIdx + FRAME_LENGTH)
                    {
                        // Extract frame using Span for better performance
                        var frame = _buffer.GetRange(startIdx, FRAME_LENGTH);

                        Console.WriteLine("🧱 Frame: " + BitConverter.ToString(frame.ToArray()).Replace("-", " "));

                        // Extract tag data more efficiently
                        var tagRaw = frame.GetRange(18, 4);
                        var tagBytes = tagRaw[3] == 0x00
                            ? tagRaw.GetRange(0, 3)
                            : tagRaw;

                        int tagId = tagBytes.Count == 3
                            ? (tagBytes[0] << 16) | (tagBytes[1] << 8) | tagBytes[2]
                            : BitConverter.ToInt32(tagBytes.ToArray(), 0);

                        // التحقق من تغير القيمة والفاصل الزمني
                        if (tagId != _lastTagId && (DateTime.Now - _lastReadTime) > _readInterval)
                        {
                            _lastTagId = tagId;
                            _lastReadTime = DateTime.Now;

                            // إطلاق الحدث مع قيمة الـ Tag ID
                            OnTagReceived?.Invoke(tagId);

                            Console.WriteLine("🆔 Tag Hex: " + BitConverter.ToString(tagBytes.ToArray()).Replace("-", ""));
                            Console.WriteLine("🆔 Tag ID: " + tagId);
                        }

                        // Remove processed data from buffer
                        _buffer.RemoveRange(0, startIdx + FRAME_LENGTH);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static int IndexOf(List<byte> buffer, byte[] pattern)
        {
            for (int i = 0; i <= buffer.Count - pattern.Length; i++)
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Close();
                }
                _disposed = true;
            }
        }
    }
}