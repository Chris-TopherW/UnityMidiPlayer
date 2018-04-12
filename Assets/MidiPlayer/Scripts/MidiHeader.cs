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
using System;
using UnityEngine;

namespace cwMidi
{
    public class MidiHeader
    {
        protected ushort headerSize = 14;
        protected byte[] headerFile = new byte[14];
        private ushort bpm = 0;
        private ushort midiType = 0;
        private ushort numTracks = 0;
        public bool headerIsOK = false;

        public MidiHeader(int p_midiType, int p_numTracks, int p_bpm)
        {
            if (p_midiType >= 2 || p_numTracks >= 128)
            {
                Debug.Log("Error: only accepts type 0 midi files");
                headerIsOK = false;
                return; 
            }
            byte[] MThd = { 0x4d, 0x54, 0x68, 0x64 };
            for (int i = 0; i < MThd.Length; i++)
                headerFile[i] = MThd[i];

            byte[] headerSize = { 0x00, 0x00, 0x00, 0x06 };
            for (int i = 0; i < headerSize.Length; i++)
                headerFile[i + MThd.Length] = headerSize[i];

            midiType = (ushort)p_midiType;
            byte[] type = { 0x00, (byte)midiType };
            for (int i = 0; i < type.Length; i++)
                headerFile[i + MThd.Length + headerSize.Length] = type[i];

            numTracks = (ushort)p_numTracks;
            byte[] tracks = { 0x00, (byte)numTracks };
            for (int i = 0; i < tracks.Length; i++)
                headerFile[i + MThd.Length + headerSize.Length + type.Length] = tracks[i];

            bpm = (ushort)p_bpm;
            ushort bpmAt16Bit = bpm;
            byte[] trackBpm = BitConverter.GetBytes(bpmAt16Bit);
            if (trackBpm.Length != 2) return; //Put debug error message here

            //check for system byte organisation
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < 2; i++)
                    headerFile[i + MThd.Length + headerSize.Length + type.Length + tracks.Length] = trackBpm[1 - i];
            }
            else
            {
                for (int i = 0; i < 2; i++)
                    headerFile[i + MThd.Length + headerSize.Length + type.Length + tracks.Length] = trackBpm[i];
            }
            headerIsOK = true;
        }

        public MidiHeader()
        {
            for(int i = 0; i < headerFile.Length; i++)
            {
                headerFile[i] = 0x00;
            }
            byte[] MThd = { 0x4d, 0x54, 0x68, 0x64 };
            for (int i = 0; i < MThd.Length; i++)
                headerFile[i] = MThd[i];

            byte[] headerSize = { 0x00, 0x00, 0x00, 0x06 };
            for (int i = 0; i < headerSize.Length; i++)
                headerFile[i + MThd.Length] = headerSize[i];
            setBpm(120);
            headerIsOK = true;
        }

        protected void replaceHeader(byte[] p_header)
        {
            if(p_header.Length == headerFile.Length)
                headerFile = p_header;
        }

        public void setMidiType(int p_midiType)
        {
            if (p_midiType > 1)
            {
                headerIsOK = false;
                return; //put error catch here
            }
            midiType = (ushort)p_midiType;
            headerFile[9] = (byte)midiType;
        }

        public void setNumTracks(int p_numTracks)
        {
            if (p_numTracks > 1) setMidiType(1);
            if (p_numTracks > 0xFF) return; //catch error oi
            numTracks = (ushort)p_numTracks;
            int i = 0;
        }

        public void setBpm(int p_bpm)
        {
            //if (p_bpm > 0xFFFF) return; //CATCH OVERFLOW?
            bpm = (ushort)p_bpm;
            ushort bpmAt16Bit = bpm;
            byte[] trackBpm = BitConverter.GetBytes(bpmAt16Bit);
            if (trackBpm.Length != 2) return; //Put debug error message here

            //check for system byte organisation
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < 2; i++)
                    headerFile[i + 12] = trackBpm[1 - i];
            }
            else
            {
                for (int i = 0; i < 2; i++)
                    headerFile[i + 12] = trackBpm[i];
            }
        }
        public int getMidiType()    { return midiType; }

        public int getNumTracks()   { return numTracks; }

        public int getBpm()         { return bpm; }

    }
}
