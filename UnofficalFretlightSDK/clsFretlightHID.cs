using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HidLibrary;

namespace UnofficalFretlightSDK
{
    /// <summary>
    /// Author: Poleboy10, Copyright (c) 2014 
    /// This class is responsible for all USB HID communcation between the
    /// software and the Fretlight Guitar using Mike O'Brien's HidLibrary class
    /// which can be found at: http://github.com/mikeobrien/HidLibrary 
    /// </summary>
    public static class FretlightHID
    {
        // portions of this code were inspired by Thomas Hammer (www.thammer.net) 
        // Griffin PowerMate project which also uses the HidLibrary class

        private const string MESSAGEBOX_CAPTION = "clsFretlightHID";
        // VID and PID for the Fretlight Guitar
        private const int VendorId = 0x0925;
        private const int ProductId = 0x2000;
        private static HidDevice FretlightGuitar;
        public static bool deviceReady = false;
        private static FretlightState prevState;

        /// <summary>
        /// establish four custom EventHandlers for the Fretlight guitar
        /// </summary>
        public static event EventHandler FretlightAttached;
        public static event EventHandler FretlightRemoved;
        public static event EventHandler<FretlightEventArgs> SwitchOneClicked;
        public static event EventHandler<FretlightEventArgs> SwitchTwoClicked;

        /// <summary>
        /// Initializes a new instance of the FretlightState class.
        /// </summary>
        public static void FootSwitchPrevState()
        {
            prevState = new FretlightState(FootSwitchState.Off, FootSwitchState.Off);
        }

        /// <summary>
        /// Try to open a connection with the Fretlight guitar
        /// after a successful connection, a DeviceAttached event will normally be sent
        /// </summary>
        public static bool OpenConnection()
        {
            Debug.WriteLine("Connecting ... ");
            FretlightGuitar = HidDevices.Enumerate(VendorId, ProductId).FirstOrDefault();

            if (FretlightGuitar != null)
            {
                FretlightGuitar.OpenDevice();

                // show debugger info 
                Debug.WriteLine("Fretlight found");
                byte[] data;
                FretlightGuitar.ReadProduct(out data);
                var deviceInfo = Encoding.Unicode.GetString(data).TrimEnd("\0".ToCharArray());
                Debug.WriteLine("Product: " + deviceInfo);
                FretlightGuitar.ReadManufacturer(out data);
                // important byte[] to string conversion method for other HidLibrary methods
                deviceInfo = Encoding.Unicode.GetString(data).TrimEnd("\0".ToCharArray());
                Debug.WriteLine("Manufacturer: " + deviceInfo);
                Debug.WriteLine("Description: " + FretlightGuitar.Description);

                FretlightGuitar.Inserted += DeviceAttachedHandler;
                FretlightGuitar.Removed += DeviceRemovedHandler;
                FretlightGuitar.MonitorDeviceEvents = true;
                return true;
            }

            Debug.WriteLine("Fretlight guitar not found.");
            return false;
        }

        private static void OnReport(HidReport report)
        {
            if (deviceReady == false) return;
            if (report.Data.Length == 1)
            {
                FretlightState state = ParseState(report.Data);
                if (!state.IsValid) Debug.WriteLine("Invalid Fretlight state");

                else
                {
                    GenerateEvents(state);

                    Debug.Write("Fretlight raw data: ");
                    for (int i = 0; i < report.Data.Length; i++)
                    {
                        Debug.Write(String.Format("{0:000} ", report.Data[i]));
                    }
                    Debug.WriteLine("");
                }
            }

            FretlightGuitar.ReadReport(OnReport);
        }

        private static FretlightState ParseState(byte[] data)
        {
            if (data.Length == 2)
            {
                FootSwitchState switchOne = data[0] == 0 ? FootSwitchState.Off : FootSwitchState.On;
                FootSwitchState switchTwo = data[1] == 0 ? FootSwitchState.Off : FootSwitchState.On;

                return new FretlightState(switchOne, switchTwo);
            }
            return new FretlightState(); // FretlightState.Invalid() will return false
        }

        private static void OnSwitchOne(FretlightState state)
        {
            var handle = SwitchOneClicked;
            if (handle != null)
            {
                handle(null, new FretlightEventArgs(state));
            }
        }

        private static void OnSwitchTwo(FretlightState state)
        {
            var handle = SwitchTwoClicked;
            if (handle != null)
            {
                handle(null, new FretlightEventArgs(state));
            }
        }

        private static void GenerateEvents(FretlightState state)
        {
            if (state.SwitchOne == FootSwitchState.Off && prevState.SwitchOne == FootSwitchState.Off)
                OnSwitchOne(state);
            else
                OnSwitchOne(state);

            if (state.SwitchTwo == FootSwitchState.Off && prevState.SwitchTwo == FootSwitchState.Off)
                OnSwitchTwo(state);
            else
                OnSwitchTwo(state);

            prevState = state;
        }

        private static void DeviceAttachedHandler()
        {
            deviceReady = true;
            if (FretlightAttached != null)
                FretlightAttached(null, EventArgs.Empty);

            //    FretlightGuitar.ReadReport(OnReport); // monitors read data (footswitch)
            Debug.WriteLine("Fretlight guitar attached.");
        }

        private static void DeviceRemovedHandler()
        {
            deviceReady = false;
            if (FretlightRemoved != null)
                FretlightRemoved(null, EventArgs.Empty);

            Debug.WriteLine("Fretlight guitar removed.");
        }

        /// <summary>
        /// Read footswitch settings (data)  
        /// Bit 0 is the right switch and bit 1 is the left switch
        /// </summary>
        public static byte[] ReadData()
        {
            if (!deviceReady) return null;
            HidDeviceData hddData = FretlightGuitar.Read(10);

            if (hddData.Status != 0) ShowMsg("Error: reading device.");

            Debug.WriteLine("Read data: " + BitConverter.ToString(hddData.Data));
            return hddData.Data;
        }

        /// <summary>
        /// Write 7 bytes of (LED) data to the guitar
        /// 10 milliseconds is allowed to complete the transfer
        /// </summary>
        public static void WriteData(byte[] data)
        {
            if (!deviceReady) return;
            if (data.Length != 7) ShowMsg("Error: data length");
            HidReport report = FretlightGuitar.CreateReport();
            report.ReportId = 0x00; // is always zero
            report.Data = data;

            bool success = FretlightGuitar.WriteReport(report, 10);

            if (!success && FretlightGuitar.IsOpen) ShowMsg("Error: writing to device.");
        }

        /// <summary>
        /// Write three 7 byte data packets to the guitar
        /// inspired by Rodrigo Groppa method in Fretlight Animator project
        /// </summary>
        public static void WritePacket(byte[,] packetData)
        {
            var outBuffer = new byte[7];

            Buffer.BlockCopy(packetData, 0, outBuffer, 0, 7);
            WriteData(outBuffer);

            Buffer.BlockCopy(packetData, 7, outBuffer, 0, 7);
            WriteData(outBuffer);

            Buffer.BlockCopy(packetData, 14, outBuffer, 0, 7);
            WriteData(outBuffer);
        }

        /// <summary>
        /// Closes the connection
        /// </summary>
        public static void CloseConnection()
        {
            if (FretlightGuitar != null) FretlightGuitar.CloseDevice();
            deviceReady = false;
        }

        private static void ShowMsg(string msgText)
        {
            MessageBox.Show(msgText, MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
