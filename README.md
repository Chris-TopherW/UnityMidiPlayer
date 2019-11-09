# UnityMidiPlayer
Midi file player for unity. Allows import of type zero and one midi files and playing them via midi outputs. Also provides code for creating procedural music using midi.

Some older midi features such as embedded lyrics in midi tracks are not supported by this tool, and trying to import them will often cause an error. (I'll get around to supporting the weirder bits of the standard at some point)

How to use:
1) Drag type 0 or type 1 midi file somewhere into asset folder
2) Change file extension from .mid to .txt
3) Drag .txt midi file into MidiFile input box in MidiSource component
4) Set midi output via Output devices section of MidiEngine component
5) Press play!

Useful functions:
MidiPlayer.PlayTrack(MidiTrack track, MidiSource source); -plays through a midi track


MidiPlayer.PlayNext(MidiMessage message, MidiSource source); -plays a midi note once previous note has finished (calculated via PPQ)


MidiPlayer.PlayScheduled(MidiMessage message, double dsptime); -schedule midi to play at Unity DSPTime (double in seconds)


MidiPlayer.Play(MidiMessage p_message); -Plays midi message as soon as possible

Warning: this code was part of my grad research in 2016 and is a dumpster fire, enter at your own peril. 
