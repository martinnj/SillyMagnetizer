# SillyMagnetizer
Collects magnet links in a silly textbox. Because I need something for this.

## Installation

	1. Build using msbuild or straight from Visual Studio. (Or whatever you use, I don't judge ;))
	2. Paste the complete path to your finished executable into the Magnet.reg file. Mine was `D:\\Code\\SillyMagnetizer\\SillyMagentizer\\bin\\Release\\SillyMagentizer.exe`.
	3. Go click some magnet links.

## Quirks

	- I know it might seem a bit slow, instead of real message parsing, it just reads and writes a file in memory, so the textbox only updates every half second.
	- There is a finite capacity of 3MB for storing magnet links. This can be increased by changing `3*1024*1024` in `Program.cs` into whatever you need. But be aware that if the number is too large, the program becomes real sluggish.
	- It's a hhobby/experiment, don't expect it to be flawless or perform well, it works just well enough for me :P