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

using System.Collections.Generic;
using System;

namespace cwMidi
{
    public class MidiTrack
    {
        private const int headerLen = 8;
        private byte[] trackHeader;
        //letters MTrk
        private readonly byte[] trkStartMes = { 0x4D, 0x54, 0x72, 0x6B };
        private byte[] trkLenBytes = { 0x00, 0x00, 0x00, 0x00 };
        private ushort trkLen;
        private readonly byte[] trkEndMes = { 0x00, 0xFF, 0x2F, 0x00 };
        private long trackPPQLen = 0;
        public long trackPPQAbsolutePos = 0;
        private int trackNum = -1;

        private List<MidiMessage> trackMessages = new List<MidiMessage>();
        private int numNotes = 0;

        public MidiTrack()
        {
            trackHeader = new byte[headerLen];
            for(int i = 0; i < 4; i++)
                trackHeader[i] = trkStartMes[i];
            for (int i = 4; i < 8; i++)
                trackHeader[i] = trkLenBytes[i - 4];
        }

        public void AddNote(MidiMessage p_message)
        {
            trackMessages.Add(p_message);
            if (p_message == null) UnityEngine.Debug.Log("Message is NULL");
            p_message.setOwnerTrack(this);
            trackPPQLen += p_message.getTimeStamp();
            p_message.setAbsTimestamp(trackPPQLen);
            numNotes++;
            UInt16 numBytesInMes = p_message.getNumBytes();
            trkLen += numBytesInMes;

            trkLenBytes = BitConverter.GetBytes(numBytesInMes);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(trkLenBytes);
        }

        public void clearTrack()
        {
            trackMessages.Clear();
            numNotes = 0;
        }

        public List<MidiMessage> getMessages() { return trackMessages; }

        public byte[] toByteArray()
        {
            List<byte> midiByteArray = new List<byte>();
            for(int i = 0; i < headerLen; i++)
            {
                midiByteArray.Add(trackHeader[i]);
            }
            for(int i = 0; i < trackMessages.Count; i++)
            {
                byte[] rawMessage = trackMessages[i].toByteArray();
                for(int j = 0; j < rawMessage.Length; j++)
                {
                    midiByteArray.Add(rawMessage[j]);
                }
            }
            for(int i = 0; i < trkEndMes.Length; i++)
            {
                midiByteArray.Add(trkEndMes[i]);
            }
            
            return midiByteArray.ToArray();
        }
        
        public MidiMessage getNote(int p_index)
        {
            if (p_index >= numNotes)
            {
                UnityEngine.Debug.Log("<color=red>Error: getNote(arg) must be lower than numNotes - 1</color>");
                return null;
            }   
            else
                return trackMessages[p_index];
        }
        public int getNumNotes() { return numNotes; }
        public int getTrackNum() { return trackNum; }
        public long getTrackPPQReadPos() { return trackPPQAbsolutePos; }
        public void setTrackNum(int p_trackNum) { trackNum = p_trackNum; }
        public long getTrackPPQLen() { return trackPPQLen; }
    }
}
