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

using System.Runtime.InteropServices;
using System;

namespace cwMidi
{
    public static class PortMidi
    {
        [DllImport("portmidi", EntryPoint = "Pm_Initialize")]
        public static extern int Pm_Initialize();

        [DllImport("portmidi", EntryPoint = "Pm_Terminate")]
        public static extern int Pm_Terminate();

        [DllImport("portmidi", EntryPoint = "Pm_CountDevices")]
        public static extern int Pm_CountDevices();

        [DllImport("portmidi", EntryPoint = "main_test_output")]
        public static extern int main_test_output(int p_deviceNum);

        [DllImport("portmidi", EntryPoint = "midiEvent")]
        public static extern void midiEvent(int status, int mess1, int mess2, int delayMs);

        [DllImport("portmidi", EntryPoint = "printOutputDevice")]
        public static extern IntPtr printOutputDevice(int index);

        [DllImport("portmidi", EntryPoint = "getNumDevices")]
        public static extern int getNumDevices();

        [DllImport("portmidi", EntryPoint = "shutdown")]
        public static extern void shutdown();

    }    
}
