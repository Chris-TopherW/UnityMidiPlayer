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
using UnityEngine;

namespace cwMidi
{
    public static class Metronome
    {

        private static double BPM = 120;
        private static double metronomeStartTime = 0.0;

        public static double ppqToMs(long p_timestamp)
        {
            double msPerBeat = (60.0 / BPM) * 1000.0;
            double msPerPPQ = msPerBeat / 960.0;
            return msPerPPQ * p_timestamp;
        }

        private static double msToPPQ(double p_ms)
        {
            double msPerBeat = (60.0 / BPM) * 1000.0;
            double msPerPPQ = msPerBeat / 960.0;
            return p_ms / msPerPPQ;
        }

        public static long msToSamps(double p_ms)
        {
            return (long)((p_ms / 1000.0) * AudioSettings.outputSampleRate);
        }

        public static void startMetro(int p_BPM)
        {
            BPM = p_BPM;
            metronomeStartTime = AudioSettings.dspTime;
        }

        public static double ppqDspTime()
        {
            return msToPPQ(AudioSettings.dspTime - metronomeStartTime);
        }

        public static double getMetroStartTime() { return metronomeStartTime; }
        public static void setBPM(double p_BPM) { BPM = p_BPM; }
    }
}
