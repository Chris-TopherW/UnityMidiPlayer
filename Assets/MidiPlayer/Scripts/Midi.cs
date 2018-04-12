//Copyright- 2018 Chris Wratt and Victoria University

//Permission is hereby granted, free of charge, to any person obtaining 
//a copy of this software ('MidiPlayer') and associated documentation files 
//, to deal in the Software without restriction,
//including without limitation the rights to use, copy, modify, merge,
//publish, distribute, sublicense, and/or sell copies of the Software,
//and to permit persons to whom the Software is furnished to do so, 
//subject to the following conditions:

//The above copyright notice and this permission notice shall be included 
//in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
//THE SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace cwMidi
{
    public static class Midi
    {
        public static int debugLevel;

        public static Dictionary<int, string> MessType = new Dictionary<int, string>()
        {
            { 0x0002 , "sequenceNum" },
            { 0x01 , "textEvent" },
            /*{ 0x02 , "copyrightMes" }, */
            { 0x03 , "trackName" },
            { 0x04 , "instName" },
            { 0x05 , "lyrics" },
            { 0x06 , "textMarker" },
            { 0x07 , "cuePoint" },
            { 0x2001 , "midiChannelPrefix" },
            { 0x2f00 , "endOfTrack" },
            { 0x5103 , "setTempo" },
            { 0x5405 , "SMPTEOffset" },
            { 0x5804 , "timeSignature" },
            { 0x5902 , "keySignature" }
        };

        public const int wholeNote = 960 * 4;
        public const int halfNote = 960 * 2;
        public const int quarterNote = 960;
        public const int eighthNote = 960 / 2;
        public const int sixteenthNote = 960 / 4;
        public const int halfNoteTri = (960 * 4) / 3;
        public const int halfNoteQuint = (960 * 4) / 5;
        public const int quarterNoteTri = (960 * 2) / 3;
        public const int quarterNoteQuint = (960 * 2) / 5;
        public const int eighthNoteTri = 960 / 3;
        public const int eighthNoteQuint = 960 / 5;
        public const int sixteenthNoteTri = 960 / (2 * 3);
        public const int sixteenthNoteQuint = 960 / (2 * 5);

        public static int midiHexTimeToNormalTime(int[] n)
        {
            int len = n.Length;
            int t = 0;
            for (int i = 0; i < len - 1; i++)
            {
                t += (n[i] - 128) * (int)Mathf.Pow(2, 7 * (len - i - 1));
            }
            t += n[len - 1];
            return t;
        }

        public static byte[] midiFileToByteArray(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }
    }
}
