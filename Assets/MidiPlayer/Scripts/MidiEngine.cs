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

using UnityEngine;
using System.Runtime.InteropServices;

namespace cwMidi
{
    [ExecuteInEditMode]
    public class MidiEngine : MonoBehaviour
    {
        public int bpm = 120;
        [Space]
        public int midiOutputDevice;
        public string[] outputDevices;

        private int previousBpm;
        private int numDevices;

        private void OnEnable()
        {
            numDevices = PortMidi.Pm_CountDevices();
            outputDevices = new string[numDevices];
            for (int i = 0; i < numDevices; i++)
            {
                outputDevices[i] = Marshal.PtrToStringAnsi(PortMidi.printOutputDevice(i));
            }
        }

        void Awake()
        {
            if (!Application.isPlaying) return;
            MidiPlayer.deviceNum = midiOutputDevice;
            MidiPlayer.Start();
            Metronome.setBPM(bpm);
            previousBpm = bpm;
        }

        private void Start()
        {
            MidiPlayer.resetMidiEventClock();
        }

        void Update()
        {
            if (!Application.isPlaying) return;
            MidiPlayer.Update();

            if (bpm != previousBpm)
            {
                Metronome.setBPM(bpm);
                previousBpm = bpm;
            }
        }

        private void OnApplicationQuit() { MidiPlayer.Shutdown(); }
    }
}
