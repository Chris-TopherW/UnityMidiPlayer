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
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace cwMidi
{
    public class PlaySchedMIDI : MonoBehaviour
    {
        private double startTime;
        private const double timeOffsetMs = 0.5;
        private double nextScheduledTimeMs = 0.0;

        MidiMessage noteOn;
        MidiMessage noteOff;

        private void Start()
        {
            startTime = AudioSettings.dspTime;
            noteOn = new MidiMessage(0x90, 60, 127);
            noteOff = new MidiMessage(0x80, 60, 0);
            noteOn.setChannel(1);
            noteOff.setChannel(1);
            MidiPlayer.PlayScheduled(noteOn, startTime);
            MidiPlayer.PlayScheduled(noteOff, startTime + 0.1);

            Debug.Log("Delay until event: " + (int)(((startTime + nextScheduledTimeMs) - AudioSettings.dspTime) * 1000.0) + " ms");
        }

        private void Update()
        {
            if (AudioSettings.dspTime > (startTime + nextScheduledTimeMs + (timeOffsetMs - Time.deltaTime * 2)))
            {
                nextScheduledTimeMs += timeOffsetMs;
                MidiPlayer.PlayScheduled(noteOn, startTime + nextScheduledTimeMs);
                MidiPlayer.PlayScheduled(noteOff, startTime + nextScheduledTimeMs + 0.1);

                Debug.Log("Delay until event: " + (int)(((startTime + nextScheduledTimeMs) - AudioSettings.dspTime) * 1000.0) + " ms");
            }
        }
    }
}
