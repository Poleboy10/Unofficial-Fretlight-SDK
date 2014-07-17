using System;
using System.Linq;
using System.Windows.Forms;

namespace UnofficalFretlightSDK
{
    /// <summary>
    /// Author: Poleboy10, Copyright (c) 2014 
    /// This class is responsible for all intermediate data conversions,
    /// such as, going from GUI to guitar, and/or from guitar to GUI.
    /// Also inlcuded are some conversion methods for MIDI data which 
    /// may be used in other Fretlight projects
    /// </summary>
    public class FretlightLED
    {
        private const string MESSAGEBOX_CAPTION = "clsFretlightLed";
        private static byte[] LedHex; // key to LED fretboard 
        public static int[] NutNotes; // establishes guitar tuning
        public static string[] DefaultNoteNames;

        /// <summary>
        /// Initialize the FretlightLED class arrarys
        /// </summary>
        public static void InitLed()
        {
            // High E, B, G, D, A, low e --> index order
            // Standard Tuning Midi Notes
            NutNotes = new int[] { 64, 59, 55, 50, 45, 40 };
            DefaultNoteNames = "C, C#, D, D#, E, F, F#, G, G#, A, A#, B".Split(',');
            // 6x8 LED Matrix High E to low e - wrapping and repeating pattern
            LedHex = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
        }

        public static void AllLedOff()
        {
            // 00 00 00 00 00 00 01 = LEDS OFF, Frets 0 to 7
            // 00 00 00 00 00 00 02 = LEDS OFF, Frets 8 to 15
            // 00 00 00 00 00 00 03 = LEDS OFF, Frets 16 to 21

            var outBuffer = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06 };
            for (int i = 1; i <= 3; i++)
            {
                outBuffer[6] = Convert.ToByte(i);
                FretlightHID.WriteData(outBuffer);
            }
        }

        public static void AllLedOn()
        {
            // FF FF FF FF FF FF 01 = LEDS ON, Frets 0 to 7
            // FF FF FF FF FF FF 02 = LEDS ON, Frets 8 to 15
            // FF FF FF FF FF FF 03 = LEDS ON, Frets 16 to 21

            var outBuffer = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x06 };
            for (int i = 1; i <= 3; i++)
            {
                outBuffer[6] = Convert.ToByte(i);
                FretlightHID.WriteData(outBuffer);
            }
        }

        /// <summary>
        /// translate the FbArray into three 7 byte data packets for the guitar
        /// inspired by Rodrigo Groppa method found in Fretlight Animator project
        /// at: https://github.com/RGKaizen/FretLight-Animator
        /// this adaptation is for a single dimension fretboard array
        /// </summary>
        public static byte[,] FbArray2Packet(byte[] fbArray)
        {
            var packet = new byte[3, 7];

            // These are the header values for each packet
            packet[0, 6] = 0x01;
            packet[1, 6] = 0x02;
            packet[2, 6] = 0x03;

            // byte and packet counters increment the cur values every 8 frets, and 48 frets respectively
            int curPacket = 0;
            int curByte = 5;
            int byteCounter = 0;
            int packetCounter = 0;

            for (int i = 0; i < 132; i++)
            {
                // LEDs are in reverse order, so 128 is the first led, 64 the second and so on
                if (fbArray[i] == 1)
                {
                    packet[curPacket, curByte] += (Byte)Math.Pow(2, 7 - byteCounter);
                }
                byteCounter++;

                // When we hit 8, switch to the next byte in the packet
                if (byteCounter == 8)
                {
                    byteCounter = 0;
                    curByte--;
                    packetCounter++;
                }

                // When we hit 6, we must move to the next packet
                if (packetCounter == 6)
                {
                    packetCounter = 0;
                    curByte = 5;
                    curPacket++;
                }
            }
            return packet;
        }

        /// <summary>
        /// Converts three 7 byte data packets to a one dimensional FbArray
        /// </summary>
        public static byte[] Packet2FbArray(byte[,] packetData)
        {
            var fbArray = new byte[132];
            var outBuffer = new byte[7];

            for (int i = 0; i < 3; i++)
            {
                Buffer.BlockCopy(packetData, i * 7, outBuffer, 0, 7);
                fbArray = OutBuf2FbArray(outBuffer, fbArray);
            }
            return fbArray;
        }

        /// <summary>
        /// input byte data[0] ... data[6]
        /// returns string like "00-00-00-00-00-00"
        /// </summary>
        public static string Byte2String(byte[] dataByte)
        {
            return BitConverter.ToString(dataByte);
        }
        /// <summary>
        /// input string like "00-00-00-00-00-00"
        /// returns byte data[0] ... data[6]
        /// </summary>
        public static byte[] String2Byte(string dataString)
        {
            return dataString.Split('-').Select(x => Convert.ToByte(x, 16)).ToArray();
        }

        /// <summary>
        /// input byte data[0] ... data[6]
        /// returns string like "00 00 00 00 00 80 01"
        /// </summary>
        public static string Buf2Str(byte[] dataByte)
        {
            var msgText = String.Empty;
            var hexText = String.Empty;

            for (long n = 0; n <= dataByte.GetUpperBound(0); n++)
            {
                if (dataByte[n] < 0x10)
                    hexText = "0" + Convert.ToString(dataByte[n], 16).ToUpper();
                else
                    hexText = Convert.ToString(dataByte[n], 16).ToUpper();

                msgText = msgText + hexText + " ";
            }
            return msgText.Trim(' ');
        }
        /// <summary>
        /// input string like "00 00 00 00 00 80 01"
        /// returns byte data[0] ... data[6]
        /// </summary>
        public static byte[] Str2Buf(string hexString)
        {
            return hexString.Split(' ').Select(x => Convert.ToByte(x, 16)).ToArray();
        }

        /// <summary>
        /// input a FbArray Index Number
        /// returns the relative fret number
        /// </summary>
        public static int FbNdx2FretNum(int mNdx)
        {
            return (int)Math.Floor(mNdx / 6.0);
        }

        /// <summary>
        /// input a FbArray Index Number and the Midi Nut Note Numbers
        /// returns the relative midi note number
        /// </summary>
        public static int FbNdx2NoteNum(int mNdx, int[] nutNotes)
        {
            return nutNotes[(mNdx % 6)] + (int)Math.Floor(mNdx / 6.0);
        }

        /// <summary>
        /// input a FbArray Index Number 
        /// returns the relative guitar string number (zero base)
        /// </summary>
        public static int FbNdx2StrNum(int mNdx)
        {
            return mNdx % 6;
        }

        /// <summary>
        /// input FbArray, Midi Channel Number, Midi Note Number 
        /// returns FbArray
        /// </summary>
        public static byte[] Midi2FbArray(byte[] fbArray, byte chanNum, byte noteNum)
        {
            // this data could come from an actual Midi file just as well
            // given a Midi Channel 10 - 15, aka Guitar Strings 0 - 5
            // and given a Midi Note Number 0 - 127
            // populate the fretboard array
            //
            // if Midi deltatime is 0 between notes then this is chord mode
            // so DO NOT clear the FbArray() between calling the subroutine
            // else single note mode so clear FbArray(0 to 131) between calling
            //
            string txtMsg = String.Empty;
            int gString = chanNum - 10;
            int mNdx = gString + (noteNum - NutNotes[gString]) * 6; // CRITICAL      

            if (mNdx > -1 & mNdx < 132)
            {
                fbArray[mNdx] = 1;
            }
            else // error
            {
                switch (gString)
                {
                    case 0:
                        txtMsg = "1st";
                        break;
                    case 1:
                        txtMsg = "2nd";
                        break;
                    case 2:
                        txtMsg = "3rd";
                        break;
                    case 3:
                        txtMsg = "4th";
                        break;
                    case 4:
                        txtMsg = "5th";
                        break;
                    case 5:
                        txtMsg = "6th";
                        break;
                }

                ErrMsg("Midi note number " + noteNum + " can not" + Environment.NewLine + "be found on the " + txtMsg + " string.");
            }
            return fbArray;
        }

        /// <summary>
        /// input Midi Note Number 
        /// returns Note Name
        /// </summary>
        public static string NoteNum2NoteName(int midiNoteNum)
        {
            string noteName;
            // converts from MIDI note number to note name w/ optional octave number string
            // example: NoteNum2Name(12)='C'
            // alt example: Note2Name(12)='C1'
            if ((midiNoteNum > -1) & (midiNoteNum < 128))
                noteName = DefaultNoteNames[midiNoteNum % 12];
            // Alt Calculation to show note name and octave number
            // NoteNum2Name = DefaultNoteNames(midiNoteNum Mod 12) & str(Int(midiNoteNum / 12))
            else
                noteName = "?";

            return noteName;
        }

        /// <summary>
        /// input OutputBuffer byte data[0] ... data[6] and an existing FbArray (optional)
        /// returns FbArray (additive) if an FbArray was input
        /// </summary>
        public static byte[] OutBuf2FbArray(byte[] outBuffer, byte[] fbArray = null)
        {
            // each OutputBuffer() is capable of controlling 48 Leds
            // accomadates chords that overlap Fretgroups by using additive FbArray
            //
            // this method is key to going backward - DO NOT CHANGE
            //
            byte mFgNum = outBuffer[6];
            if (fbArray == null)
            {
                fbArray = new byte[132];
                for (int i = 0; i < 132; i++)
                    fbArray[i] = 0x00;
            }

            for (int j = 5; j >= 0; j--)
            {
                byte mBufVal = outBuffer[j];

                for (int i = 0; i <= 7; i++)
                {
                    if (mBufVal == 0)
                        break; // speed up process

                    if (mBufVal >= LedHex[i])
                    {
                        mBufVal = Convert.ToByte(mBufVal - LedHex[i]);
                        int mNdx = i + (mFgNum * 48) - ((j + 1) * 8); // CRITICAL

                        if (mNdx < 132) // error check
                            fbArray[mNdx] = 0x01;
                    }
                }
            }
            return fbArray;
        }

        /// <summary>
        /// input Guitar String Number and Fret Number (both zero based)
        /// returns FbArray Index Number
        /// </summary>
        public static int StrFret2FbNdx(int gString, int fretNum)
        {
            // remember ... FbArray(FbNdx) is zero based
            // gString and FretNum are also zero based
            return gString + fretNum * 6; // alternate faster method OK
            // StrFret2FbNdx = (gString + fretNum) + (fretNum * 5) ' also correct
        }

        /// <summary>
        /// input Guitar String Number and Fret Number (both zero based)
        /// returns Midi Note Name
        /// </summary>
        public static string StrFret2NoteName(int gString, int fretNum)
        {
            return Convert.ToString(NoteNum2NoteName(NutNotes[gString] + fretNum)).Trim(' ');
        }

        /// <summary>
        /// input Guitar String Number and Fret Number (both zero based)
        /// returns Midi Note Number
        /// </summary>
        public static int StrFret2NoteNum(int gString, int fretNum)
        {
            // You can do a lot of things with conversions but ...
            // you can not go from Note Number to String and Fret Numbers
            // because a note number occurs in more than one place
            // on the guitar fretboard  ):  Please send me the algo
            // if you know how.  Possibly by targeting note numbers
            // for grouping together by typical hand (chord) positions?
            //
            return NutNotes[gString] + fretNum;
        }

        public static byte[] TestDataHex()
        {
            // usage: FretlightHID.WriteData(FretlightLED.TestDataHex());
            // 00 00 00 00 00 80 01 = E Notes - String 1, Fret 0
            var outBuffer = new byte[7];
            outBuffer[0] = 0x00;
            outBuffer[1] = 0x00;
            outBuffer[2] = 0x00;
            outBuffer[3] = 0x00;
            outBuffer[4] = 0x00;
            outBuffer[5] = 0x80;
            outBuffer[6] = 0x01;
            return outBuffer;
        }

        public static byte[] TestDataString()
        {
            // usage: FretlightHID.WriteData(FretlightLED.TestDataString());
            string dataString = "00 00 00 00 00 80 01"; // E Note - String 1, Fret 0
            return Str2Buf(dataString);
        }

        private static void ErrMsg(string msgText)
        {
            MessageBox.Show(msgText, MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// PB depricated method ... converts fretboard array to fretgroup buffer
        /// returns: the fretgroup buffer data as a byte array
        /// </summary>
        public static byte[,] FbArray2FgBuf(byte[] fbArray)
        {
            byte[,] mFgBuf = new byte[4, 6]; // only elements (1 To 3, 0 To 5) are used
            //
            // this is the second step to going forward - DO NOT CHANGE
            //
            for (var mNdx = 0; mNdx <= 131; mNdx++) // this is good ... only 131 on fretboard
            {
                if (fbArray[mNdx] != 0)
                {
                    long mFgNum = (int)Math.Floor(mNdx / 48.0) + 1; // 1 to 3
                    long mBufNum = 6 - (int)Math.Floor((mNdx % 48.0) / 8) - 1; // 0 to 5
                    byte mHex = LedHex[(int)Math.Floor(mNdx % 8.0)]; // &H1 to &H80
                    mFgBuf[mFgNum, mBufNum] = Convert.ToByte(mFgBuf[mFgNum, mBufNum] + mHex);
                }
            }
            return mFgBuf;
        }

        /// <summary>
        /// PB depricated method ... writes the fretgroup buffer to the Led Guitar (HID)
        /// input: FgBuffer
        /// return: writes data to guitar
        /// </summary>
        /// <param name="fgBuffer"></param>
        public static void FgBuf2Guitar(byte[,] fgBuffer)
        {
            var outBuffer = new byte[7];

            for (int i = 1; i <= 3; i++) // mFgNum
            {
                for (int j = 0; j <= 5; j++) // mBufNum
                {
                    outBuffer[j] = fgBuffer[i, j]; // CRITICAL
                }
                // mFgNum by definition goes from 1 to 3
                outBuffer[6] = Convert.ToByte(i);
                FretlightHID.WriteData(outBuffer);
            }
        }

    }
}
