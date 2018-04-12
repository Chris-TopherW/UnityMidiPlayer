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

using System;

namespace cwMidi
{
    public class MidiMessage
    {

        public MidiSource noteSource;

        protected byte[] messageAsBytes;
        protected UInt16 numBytes = 0;
        protected int bytesInTimeStamp;
        protected int PPQDivision = 960;
        protected int timeStamp = -1;
        protected long absoluteTimeStamp = -1;
        protected int status = 0;
        protected int midiEvent = 0;
        protected int channel = 0;
        protected int controlByte1 = 0;
        protected int controlByte2 = 0;
        protected MidiTrack ownerTrack;

        public MidiMessage(byte[] p_messageAsBytes, int p_bytesInTimeStamp = 1)
        {
            messageAsBytes = p_messageAsBytes;
            numBytes = (ushort)messageAsBytes.Length;

            bytesInTimeStamp = p_bytesInTimeStamp;
            byte[] timeStampRaw = new byte[bytesInTimeStamp];
            for (int i = 0; i < bytesInTimeStamp; i++)
            {
                timeStampRaw[i] = messageAsBytes[i];
            }
            timeStamp = midiHexTimeToNormalTime(timeStampRaw);
            status = messageAsBytes[p_bytesInTimeStamp];
            midiEvent = messageAsBytes[p_bytesInTimeStamp] & 0xF0;
            channel = messageAsBytes[p_bytesInTimeStamp] & 0x0F;
            controlByte1 = messageAsBytes[p_bytesInTimeStamp + 1];
            controlByte2 = messageAsBytes[p_bytesInTimeStamp + 2];
        }

        public MidiMessage(byte stat, byte data1, byte data2)
        {
            messageAsBytes = new byte[4];
            messageAsBytes[0] = 0x00;
            messageAsBytes[1] = stat;
            messageAsBytes[2] = data1;
            messageAsBytes[3] = data2;

            status = stat;
            midiEvent = status & 0xF0;
            channel = status & 0x0F;
            controlByte1 = data1;
            controlByte2 = data2;
        }

        public MidiMessage() { ; }

        public void print()
        {
            UnityEngine.Debug.Log("Time = " + timeStamp +"  Event = " + midiEvent.ToString("X") + "  channel = " + channel.ToString("X") +
                "  control byte 1 = " + controlByte1 + "  control byte 2 = " + controlByte2);
        }

        public byte[] toByteArray() { return messageAsBytes; }
        public void setTimestamp(int p_timestamp) { timeStamp = p_timestamp; }
        public void setAbsTimestamp(long p_timestamp) { absoluteTimeStamp = p_timestamp; }
        public void setOwnerTrack(MidiTrack p_owner) { ownerTrack = p_owner; }
        public byte[] getMessageAsBytes() { return messageAsBytes; }
        public ushort getNumBytes() { return (ushort)messageAsBytes.Length; }
        public int getByteOne() { return controlByte1; }
        public int getByteTwo() { return controlByte2; }
        public int getTimeStamp() { return timeStamp; }
        public long getAbsTimeStamp() { return absoluteTimeStamp; }
        public int getPPQ() { return PPQDivision; }
        public int getStatusByte() { return status;  }
        public int getMidiEvent() { return midiEvent; }
        public MidiTrack getOwnerTrack() { return ownerTrack; }

        public float getGain() {
            if (noteSource != null) return noteSource.volume;
            else
            {
                return 1.0f;
            }
        }

        private int midiHexTimeToNormalTime(byte[] n)
        {
            int len = n.Length;
            int t = 0;
            for (int i = 0; i < len - 1; i++)
            {
                t += (n[i] - 128) * (int)UnityEngine.Mathf.Pow(2, 7 * (len - i - 1));
            }
            t += n[len - 1];
            return t;
        }

        public MidiMessage copy()
        {
            MidiMessage other = (MidiMessage)this.MemberwiseClone();
            return other;
        }
    }
}
