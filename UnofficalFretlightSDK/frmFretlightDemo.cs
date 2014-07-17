using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using cntlLedBulb;

namespace UnofficalFretlightSDK
{
    /// <summary>
    /// Author: Poleboy10, Copyright (c) 2014  
    /// Project: Unofficial Fretlight SDK Demo in VS2010 C#
    /// 
    /// USB HID communication is provided by Mike O'briens robust HidLibray class 
    /// (dll and source code included) Copyright (c) 2010 Ultraviolet Catastrophe
    ///
    /// Be sure to also check out my LED Fretboard Light Show, and LED Fretboard 
    /// Simulator programs for your Fretlight guitar.  These free VB6 programs
    /// and more can be found on my website at: http://goo.gl/GM4dBc
    /// </summary>
    public partial class FretlightDemo : Form
    {
        private const string MESSAGEBOX_CAPTION = "frmFretlightDemo";
        private LedBulb[] _ledBulb;
        private byte[] FbArray;
        private Timer ConnectionTimer = new Timer();

        public FretlightDemo()
        {
            InitializeComponent();
            ConnectionTimer.Interval = 1250; // polling time
            ConnectionTimer.Tick += new EventHandler(connectionTimer_Tick);
            ConnectionTimer.Start();
            FretlightHID.FretlightAttached += new EventHandler(Fretlight_DeviceAttached);
            FretlightHID.FretlightRemoved += new EventHandler(Fretlight_DeviceRemoved);
            FretlightHID.SwitchOneClicked += new EventHandler<FretlightEventArgs>(Fretlight_SwitchOneClicked);
            FretlightHID.SwitchTwoClicked += new EventHandler<FretlightEventArgs>(Fretlight_SwitchTwoClicked);
        }

        private void connectionTimer_Tick(object sender, EventArgs e)
        {
            // initial HID device polling required only at program startup
            if (FretlightHID.OpenConnection()) ConnectionTimer.Stop();
        }

        private void FretLight_Load(object sender, EventArgs e)
        {
            FretlightLED.InitLed();
            DrawFretNums();
            LoadFbImg();
            DrawFretString();
            LoadLeds();
        }

        private void FretLight_FormClosed(object sender, EventArgs e)
        {
            FretlightLED.AllLedOff();
            FretlightHID.CloseConnection();
        }

        private void FbArray2Guitar()
        {
            // FretlightLED.FgBuf2Guitar(FretlightLED.FbArray2FgBuf(FbArray)); // old working method
            FretlightHID.WritePacket(FretlightLED.FbArray2Packet(FbArray)); // new streamlined method
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 132; i++)
            {
                _ledBulb[i].On = true;
                FbArray[i] = 0x01;
            }

            if (chkGuitarCom.Checked) FbArray2Guitar(); // alt use: FretlightLED.AllLedOn();
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 132; i++)
            {
                _ledBulb[i].On = false;
                FbArray[i] = 0x00;
            }

            if (chkGuitarCom.Checked) FbArray2Guitar(); // alt use: FretlightLED.AllLedOff();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (chkGuitarCom.Checked)
            {
                FretlightLED.AllLedOff();
                FretlightHID.WriteData(FretlightLED.TestDataHex());
            }
            FbArray = FretlightLED.OutBuf2FbArray(FretlightLED.TestDataHex());
            for (int i = 0; i < 132; i++)
            {
                if (FbArray[i] == 0x01) _ledBulb[i].On = true;
                else _ledBulb[i].On = false;
            }
        }

        private void btnFootSw_Click(object sender, EventArgs e)
        {
            byte[] data = FretlightHID.ReadData();
            if (data != null)
                lblStatus.Text = "FtSwData: " + BitConverter.ToString(data);
            else
                lblStatus.Text = "Not connected";
        }


        private void Fretlight_DeviceRemoved(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {   // Event can be received on a separate thread, so we need to push the message
                // back on the GUI thread before we execute.
                BeginInvoke(new Action<object, EventArgs>(Fretlight_DeviceRemoved), sender, e);
                return;
            }

            ledBulbStatus.On = false;
            ShowMsg("Fretlight removed.");
        }

        private void Fretlight_DeviceAttached(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {   // Event can be received on a separate thread, so we need to push the message
                // back on the GUI thread before we execute.
                BeginInvoke(new Action<object, EventArgs>(Fretlight_DeviceAttached), sender, e);
                return;
            }

            ledBulbStatus.On = true;
            ShowMsg("Fretlight attached.");
            if (chkGuitarCom.Checked) FretlightHID.WritePacket(FretlightLED.FbArray2Packet(FbArray)); // new streamlined method
            // if (chkGuitarCom.Checked) FretlightLED.FgBuf2Guitar(FretlightLED.FbArray2FgBuf(FbArray)); // old method
        }

        private void Fretlight_SwitchOneClicked(object sender, FretlightEventArgs e)
        {
            if (InvokeRequired)
            {   // Event can be received on a separate thread, so we need to push the message
                // back on the GUI thread before we execute.
                BeginInvoke(new Action<object, FretlightEventArgs>(Fretlight_SwitchOneClicked), sender, e);
                return;
            }

            UpdateGUIFromState(e.State);

            Debug.WriteLine("Fretlight switch one clicked event");
            Debug.WriteLine(String.Format("Fretlight switch one state: {0}", (e.State.SwitchOne)));
        }

        private void Fretlight_SwitchTwoClicked(object sender, FretlightEventArgs e)
        {
            if (InvokeRequired)
            {   // Event can be received on a separate thread, so we need to push the message
                // back on the GUI thread before we execute.
                BeginInvoke(new Action<object, FretlightEventArgs>(Fretlight_SwitchTwoClicked), sender, e);
                return;
            }

            UpdateGUIFromState(e.State);

            Debug.WriteLine("Fretlight switch two clicked event");
            Debug.WriteLine(String.Format("Fretlight switch two state: {0}", (e.State.SwitchOne)));
        }

        private void UpdateGUIFromState(FretlightState state)
        {
            string SW1 = state.SwitchOne == FootSwitchState.Off ? "Off" : "On";
            string SW2 = state.SwitchTwo == FootSwitchState.Off ? "Off" : "On";
            lblStatus.Text = "SW1: " + SW1 + "  SW2: " + SW2;
        }

        private void ShowMsg(string msgText)
        {
            MessageBox.Show(msgText, MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var a = new frmAbout())
            {
                a.ShowDialog();
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var h = new frmHelp())
            {
                h.ShowDialog();
            }
        }

        private void FretlightDemo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X > 0 && e.X < picFb.Width && e.Y > 0 && e.Y < picFb.Height)
            { } // mouse is over picFb
            else
                txtDisplay.Text = "Fretboard Display";
        }

        private void picFb_MouseMove(object sender, MouseEventArgs e)
        {
            int fretNum = (int)Math.Floor(e.X / (picFb.Width / 21.9));
            int gString = (int)Math.Floor(e.Y / (picFb.Height / 6.0));
            txtDisplay.Text = String.Format("Fret {0}, String {1}, Note {2}, {3}", fretNum, (gString + 1), (FretlightLED.NutNotes[gString] + fretNum), Convert.ToString(FretlightLED.NoteNum2NoteName(FretlightLED.NutNotes[gString] + fretNum)).Trim(' '));
            // txtDisplay.Text = String.Format("FbArray() Element Number: {0}", FretLightLED.StrFret2FbNdx(gString, FretNum);
        }

        private void DrawFretString()
        {
            const int offset = 5; // positioning tweak
            const int fStart = 1;
            int fStop = picFb.Height - 1;
            const int sStart = 1;
            int sStop = picFb.Width - 1;

            var fs = new Bitmap(picFb.Image, picFb.Width, picFb.Height);
            using (var g = Graphics.FromImage(fs))
            {
                var blackPen = new Pen(Color.Black, 1);
                var greyPen = new Pen(Color.LightGray, 1);
                var whitePen = new Pen(Color.White, 1);

                // draw frets
                for (int fret = 1; fret <= 21; fret++) // don't draw a fret at the nut
                {
                    int fretX = fret * (picFb.Width / 22) + (picFb.Width / 22) - offset;
                    g.DrawLine(blackPen, fretX + 1, fStart, fretX + 1, fStop);
                    g.DrawLine(whitePen, fretX + 2, fStart, fretX + 2, fStop);
                    g.DrawLine(greyPen, fretX + 3, fStart, fretX + 3, fStop);
                    g.DrawLine(blackPen, fretX + 4, fStart, fretX + 4, fStop);
                }

                // draw Strings
                for (int gString = 1; gString <= 6; gString++)
                {
                    int stringY = gString * (picFb.Height / 6) - ((picFb.Height / 6) / 2);

                    if (gString == 1)
                    {
                        g.DrawLine(greyPen, sStart, stringY, sStop, stringY);
                        g.DrawLine(blackPen, sStart, stringY + 1, sStop, stringY + 1);
                    }

                    stringY = stringY - 1;

                    if (gString == 2 || gString == 3)
                    {
                        g.DrawLine(greyPen, sStart, stringY, sStop, stringY);
                        g.DrawLine(greyPen, sStart, stringY + 1, sStop, stringY + 1);
                        g.DrawLine(blackPen, sStart, stringY + 2, sStop, stringY + 2);
                    }

                    if (gString == 4 || gString == 5)
                    {
                        g.DrawLine(greyPen, sStart, stringY, sStop, stringY);
                        g.DrawLine(whitePen, sStart, stringY + 1, sStop, stringY + 1);
                        g.DrawLine(blackPen, sStart, stringY + 2, sStop, stringY + 2);
                    }

                    if (gString == 6)
                    {
                        g.DrawLine(greyPen, sStart, stringY, sStop, stringY);
                        g.DrawLine(whitePen, sStart, stringY + 1, sStop, stringY + 1);
                        g.DrawLine(greyPen, sStart, stringY + 2, sStop, stringY + 2);
                        g.DrawLine(blackPen, sStart, stringY + 3, sStop, stringY + 3);
                    }
                }
            }
            picFb.Image = fs;
            // fs.Save("D:\\Temp\\FretString.bmp"); // saves the image to file
        }

        private void DrawFretNums()
        {
            // this method of loading images works well without the need to use a Paint event
            const int offsetX = 5; // positioning tweak
            const int offsetY = 2; // positioning tweak
            picFn.Width = picFb.Width;
            picFn.Left = picFb.Left;

            var fn = new Bitmap(picFn.Width, picFn.Height);
            using (var g = Graphics.FromImage(fn))
            {
                g.Clear(this.BackColor); // prevents font recursion darkening
                g.DrawString("Nut", new Font("Arial", 10), Brushes.Black, offsetX, offsetY);

                for (int fret = 1; fret <= 21; fret++)
                {
                    int posX;
                    if (fret < 10)
                        posX = fret * (picFn.Width / 22) + (offsetX * 2);
                    else
                        posX = fret * (picFn.Width / 22) + offsetX;

                    g.DrawString(fret.ToString(), new Font("Tahoma", 10), Brushes.Black, posX, offsetY);
                }
            }
            picFn.Image = fn;
            // fn.Save("D:\\Temp\\FretNums.bmp"); // saves the image to file
        }

        private void LoadLeds()
        {
            // add a control array of LED Bulbs to PictureBox
            const int offset = 17; // position tweak
            const int diaLed = 9;
            _ledBulb = new LedBulb[132];
            FbArray = new byte[132];

            int i = 0;
            for (int fbFret = 0; fbFret <= 21; fbFret++)
            {
                int fretX = fbFret * (picFb.Width / 22) + offset;

                for (int fbString = 1; fbString <= 6; fbString++)
                {
                    int gstringY = fbString * (picFb.Height / 6) - Convert.ToInt16((picFb.Height / 6.0) / 1.32); // tweak 1.32

                    _ledBulb[i] = new LedBulb
                        {
                            BackColor = Color.Transparent, // slow loading, but looks great
                            //  BackColor = Color.Silver, // faster loading
                            Tag = i,
                            Top = gstringY,
                            Height = diaLed,
                            Left = fretX,
                            Width = diaLed,
                            On = false
                        };

                    _ledBulb[i].Click += new EventHandler(ledBulb_Click);
                    picFb.Controls.Add(_ledBulb[i]);
                    FbArray[i] = 0x00; // off

                    i = i + 1;
                }
            }
        }

        private void LoadFbImg()
        {
            // crop fretboard image to 22 frets and load PictureBox image
            Bitmap img = new Bitmap(Properties.Resources.FB_Land);
            // critical - do not change
            int cropWidth = img.Width / 24 * 22;
            Bitmap fb = img.Clone(new Rectangle(0, 0, cropWidth, img.Height), img.PixelFormat);
            picFb.Image = fb;
            // fb.Save("D:\\Temp\\FretBoard.bmp"); // saves the image to file
        }

        // Turn the LED bulb On or Off

        private void ledBulb_Click(object sender, EventArgs e)
        {
            int i = (int)((LedBulb)sender).Tag; // number of the LedBulb
            _ledBulb[i].On = !_ledBulb[i].On;

            if (_ledBulb[i].On)
                FbArray[i] = 0x01; // on
            else
                FbArray[i] = 0x00; // off

            if (chkGuitarCom.Checked) FbArray2Guitar();

            Debug.WriteLine("_ledBulb[" + i + "] was clicked");
            Debug.WriteLine("FbArray[" + i + "] contains " + FbArray[i].ToString());
        }
    }
}


