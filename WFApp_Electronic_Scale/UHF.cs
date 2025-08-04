using System;
using System.IO.Ports;

namespace WFApp_Electronic_Scale
{
    public class UHF
    {
        static SerialPort _serialPort;
        static byte[] buffer = new byte[0];
        static readonly byte[] FRAME_START = { 0x43, 0x54 }; // "CT"
        const int FRAME_LENGTH = 24;

        // دالة لتهيئة المنفذ التسلسلي
        public static void Init()
        {
            _serialPort = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);
            _serialPort.DataReceived += DataReceivedHandler;
            _serialPort.Open();
            Console.WriteLine("Listening to serial port...");
        }

        // دالة لإغلاق المنفذ
        public static void Close()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
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

                    Console.WriteLine("🆔 Tag Hex: " + BitConverter.ToString(tagBytes).Replace("-", ""));
                    Console.WriteLine("🆔 Tag ID: " + tagId);

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