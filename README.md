# LightsAndSirenIndicator
Simple RageHook plugin to show light and siren status, especially when in first person view. Also includes Repair and Blip functions.

# Prerequisities
This plugin requires the following:
* Rage Hook Plugin (https://ragepluginhook.net/).
* Rage Native UI (https://github.com/alexguirre/RAGENativeUI).

# Installation
1. Install prerequisites according to the latest release instructions. Ensure these are working correctly before troubleshooting issues with this plugin.
2. Drag and drop the contents of the Zip file from the latest Release into your GTAV directory. Files must be put into the RageHookPlugin 'plugins' folder.
* [GTAV Root Directory]/plugins/LightsAndSirenIndicator.ini
* [GTAV Root Directory]/plugins/LightsAndSirenIndicator.dll
3. Edit the .ini file, which is commented and should be straight forward to modify to your liking.
4. Ensure the plugin is set to load in the RageHookPlugin (RPH) launch settings. Alternatively, use *LoadPlugin LightsAndSirenIndicator.dll* in the RPH Console in-game (F4 by default while RPH is active).

# Use
* The Indicator will appear at the top middle of your screen whenever you are in a vehicle the game recognizes as a Police vehicle.
* When the siren wail is silenced, the middle of the indicator will be blank. When the siren wail is active, the indicator will show "S".
* When the lights are active, indicator will flash Red/Blue and alternate.
* When lights are inactive, indicator will turn Gray and be static.
* To 'blip' the siren (same sound as quickly turning on and off your siren+lights, but without the lights), use the "G" key by default.
* To repair your vehicle, *you must assign a key in the .INI file*. By default it has been disabled, as many other plugins already perform this function.
* Most of the above behavior can be configured or modified in the .INI file.

# Support
Please use the github Issues system to report issues.
Feel free to modify the code for your own learning or to change colors/etc, but note that I will NOT be providing support for modified code.

![Lights On while Siren is Silent](https://raw.githubusercontent.com/Epidurality/LightsAndSirenIndicator/master/Extra/LightsOffSirenOn.png)
![Lights Off while Siren is Available](https://raw.githubusercontent.com/Epidurality/LightsAndSirenIndicator/master/Extra/LightsOnSirenOff.png)
