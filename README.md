# UnityMidiPlayer
Midi file player for unity. Allows import of type zero and one midi files and playing them via midi outputs. Also provides code for creating procedural music using midi.
Some old school midi features such as embedded lyrics in midi tracks are not supported by this tool, and trying to import them will probably cause an error. (I'll get around to implementing the weird windey bit of the standard one of these days)

How to use:
1) Drag type 0 or type 1 midi file somewhere into asset folder
2) Change file extension from .mid to .txt
3) Drag .txt midi file into MidiFile input box in MidiSource component
4) Set midi output via Output devices section of MidiEngine component
5) Press play!

Useful functions:
MidiPlayer.PlayTrack(MidiTrack, MidiSource); -plays through a midi track
MidiPlayer.PlayNext(MidiMessage, MidiSource); -plays a midi note once previous note has finished (calculated via PPQ)
