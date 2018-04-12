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
using UnityEngine;

namespace cwMidi
{
    public static class MidiPlayer
    {
        private static double metronomeStartTimeMs = 0.0;
        public static int deviceNum = 0;
        private static double previousEventMS = 0.0;
        public static List<MidiMessage> messOutBuff;
        private static double updateLookAhead = 1000; //ms
        private static bool hasStarted = false;
        private static int midiOutput = 0;

        public static int Start()
        {
            if (!hasStarted)
            {
                hasStarted = true;
                messOutBuff = new List<MidiMessage>();
                return PortMidi.main_test_output(deviceNum);
            }
            else
                return -1;
        }

        public static void PlayScheduled(MidiMessage p_message, double p_time)
        {
            if (p_time < AudioSettings.dspTime)
            {
                Debug.Log("<color=yellow>Warning:</color> time must not be in past!");
                return;
            }
            double theTime = AudioSettings.dspTime;

            PortMidi.midiEvent(p_message.getStatusByte(), p_message.getByteOne(), p_message.getByteTwo(), 
                             (int)((p_time - theTime) * 1000.0));
            previousEventMS = (p_time - theTime) * 1000.0;
        }

        public static void Play(MidiMessage p_message)
        {
            if(Midi.debugLevel > 4) Debug.Log("Add note to play " + p_message.getByteOne()); 

            PortMidi.midiEvent(p_message.getStatusByte(), p_message.getByteOne(), p_message.getByteTwo(), 0);
        }

        public static void PlayTrackNext(MidiTrack p_track, MidiSource p_source)
        {
            for(int _notes = 0; _notes < p_track.getNumNotes(); _notes++)
            {
                PlayNext(p_track.getNote(_notes), p_source);
            }
        }

        public static void PlayNext(MidiMessage p_message, MidiSource p_source)
        {
            if (Midi.debugLevel > 4) Debug.Log("Add note to play " + p_message.getByteOne() + " " + p_message.getByteTwo() + " at time: " + p_message.getAbsTimeStamp());
            p_message.noteSource = p_source;
            messOutBuff.Add(p_message);
            if (p_message.getOwnerTrack() != null)
                p_source.setTrackPPQAbsolutePos(p_message.getAbsTimeStamp()); //this sets write head for ppq
        }

        public static void PlayTrack(MidiTrack p_track, MidiSource p_source)
        {
            p_source.setTrackPPQAbsolutePos(0); 
            long accumulatedTrackLenPPQ = 0;
            for (int _notes = 0; _notes < p_track.getNumNotes(); _notes++)
            {
                //hmm, not quite...
                MidiMessage nextNote = p_track.getNote(_notes);
                PlayNext(nextNote, p_source);
                accumulatedTrackLenPPQ += nextNote.getPPQ();
            }
        }

        public static void Update()
        {

            double currentTime = (AudioSettings.dspTime) * 1000.0;
            if(messOutBuff.Count > 0)
            {
                MidiMessage temporaryMessage = messOutBuff[0];
                double msUntilEvent = temporaryMessage.noteSource.startTimeOffset + Metronome.ppqToMs(temporaryMessage.getAbsTimeStamp()) - currentTime;

                //this while accounts for multiple notes at once
                while (msUntilEvent < updateLookAhead && messOutBuff.Count > 0)
                {
                    long msOffset = (long)(msUntilEvent);
                    if (msOffset < 0) msOffset = 0; //should catch any rogue startup notes
                    
                    MidiMessage p_message = messOutBuff[0];
                    messOutBuff.RemoveAt(0);
                    int statusByte;
                    int amplitude;

                    if (p_message.noteSource.ForceToChannel)
                    {
                        statusByte = p_message.getStatusByte() & 0xF0;
                        statusByte = statusByte += p_message.noteSource.Channel - 1;
                    }
                        
                    else 
                        statusByte = p_message.getStatusByte(); 

                    if (p_message.noteSource.Mute) 
                        amplitude = 0; 
                    else 
                        amplitude = (int)(p_message.getByteTwo() * p_message.getGain());


                    PortMidi.midiEvent(statusByte, p_message.getByteOne() + p_message.noteSource.PitchOffset, amplitude, (int)(msOffset));

                    if (messOutBuff.Count > 0)
                    {
                        temporaryMessage = messOutBuff[0];
                        msUntilEvent =  temporaryMessage.noteSource.startTimeOffset +  Metronome.ppqToMs(temporaryMessage.getAbsTimeStamp()) - currentTime;
                    }
                }
            }
        }

        public static void resetMidiEventClock()
        {
            metronomeStartTimeMs = (AudioSettings.dspTime) * 1000; //m/s since start of program- provides offset
        }

        public static void SetBPM(int p_BPM)
        {
            Metronome.setBPM(p_BPM);
        }

        public static void StartMetronome(int p_BPM)
        {
            Metronome.setBPM(p_BPM);
            metronomeStartTimeMs = AudioSettings.dspTime * 1000;
        }

        public static double getMetronomeStartTime() { return metronomeStartTimeMs; }
        public static void setMetronomeStartTimeMs(double p_time) { metronomeStartTimeMs = p_time; }

        public static void reorderQueue()
        {
            messOutBuff.Sort((a, b) => { return a.getAbsTimeStamp().CompareTo(b.getAbsTimeStamp()); });
        }

        public static int getMidiOutIndex()
        {
            return midiOutput;
        }

        public static void setMidiOutIndex(int p_index)
        {
            midiOutput = p_index;
        }

        public static int Shutdown()
        {
            PortMidi.shutdown();
            PortMidi.Pm_Terminate();
            //metronomeStartTimeMs = 0;
            return 1;
        }
    }
}
