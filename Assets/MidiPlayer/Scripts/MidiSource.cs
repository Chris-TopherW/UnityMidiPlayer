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
    public class MidiSource : MonoBehaviour
    {

        public TextAsset MidiClip;
        private cwMidi.MidiFile midiFile;
        private MidiTrack midiTrack;
        private long trackPPQAbsolutePos = 0;

        public bool Mute;
        public bool PlayOnAwake;
        public bool ForceToChannel;
        public int Channel = 1;

        [HideInInspector]
        public double startTimeOffset = 0.0;
        private double initialOffset = 0.0;

        [Range(-1.0f, 1.0f)]
        public float volume = 1.0f;

        [Range(-127, 127)]
        public int PitchOffset;

        private bool tempHasPlayed = false;

        private void Awake()
        {
            midiFile = new cwMidi.MidiFile(MidiClip);
            midiTrack = new MidiTrack();
            if (Midi.debugLevel > 3) midiFile.printCookedMidiFile();
        }

        void Start()
        {
            if (Channel < 1 || Channel > 16)
            {
                Debug.Log("<color=red>Error:</color> Channels must be between 1 and 16. Auto set to 1");
                Channel = 1;
            }
            if (PlayOnAwake) Play();

            initialOffset = AudioSettings.dspTime * 1000;
        }

        private void Play()
        {
            startTimeOffset = AudioSettings.dspTime * 1000;
            for (int i = 0; i < midiFile.getNumTracks(); i++)
            {
                MidiPlayer.PlayTrack(midiFile.getMidiTrack(i), this);
            }
            MidiPlayer.reorderQueue();
        }

        public long getTrackPPQAbsolutePos()
        {
            return trackPPQAbsolutePos;
        }

        public void setTrackPPQAbsolutePos(long p_pos)
        {
            trackPPQAbsolutePos = p_pos;
        }
    }
}
