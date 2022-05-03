using Rage;
using System.Drawing;
using System.Windows.Forms;
using RAGENativeUI;
using RAGENativeUI.Elements;


[assembly: Rage.Attributes.Plugin("Lights and Siren Indicator", Description = "Simple indicator for siren and emergency lights", Author = "Epidurality")]

namespace LightsAndSirenIndicator
{
    public class EntryPoint
    {
        public static void Main()
        {
            // Create and begin iniFile object
            InitializationFile ini = new InitializationFile("Plugins/LightsAndSirenIndicator.ini");
            ini.Create();
            KeysConverter kc = new KeysConverter();

            // Grab ini file settings for Indicator
            bool useIndicator = ini.ReadBoolean("Indicator", "UseIndicator", true);
            string lightsOnText = ini.ReadString("Indicator", "LightsOnText", "***");
            string lightsOnTextAlt = ini.ReadString("Indicator", "LightsOnTextAlt", "----");
            string lightsOffText = ini.ReadString("Indicator", "LightsOffText", "----");
            string sirenOnText = ini.ReadString("Indicator", "SirenOnText", "[S]");
            string sirenOffText = ini.ReadString("Indicator", "SirenOffText", "[  ]");
            float indicatorSize = (float)ini.ReadDouble("Indicator", "IndicatorSize", 0.5);
            int indicatorLocationX = ini.ReadInt16("Indicator", "IndicatorLocationX", 960);
            int indicatorLocationY = ini.ReadInt16("Indicator", "IndicatorLocationY", 0);
            ulong swapFrequency = ini.ReadUInt64("Indicator", "SwapFrequency", 250);
            bool redAndBlue = ini.ReadBoolean("Indicator", "RedAndBlue", true);
            
            // Read ini file settings for Blip and attach keybinds
            Keys blipKey = (Keys)kc.ConvertFromString(ini.ReadString("Blip", "BlipKey", "G"));

            // Read ini file settings for Repair and attach keybinds
            Keys repairKey = (Keys)kc.ConvertFromString(ini.ReadString("Repair", "RepairKey", "None"));
            
            // Set up additional variables for use in loop
            Vehicle vehicle;
            bool swap = true;
            ulong lastSwap = Game.TickCount - swapFrequency - 1; // Set to ensure the "swap" procedure is called on first run to correctly draw the indicator immediately.

            // This is essentially the indicator, implemented as a ResText. This does not immediately Draw the text. Draw() is called later. This is simply setting the indicator defaults/look.
            ResText indicator = new ResText("", new Point(indicatorLocationX, indicatorLocationY), indicatorSize, Color.White, Common.EFont.Pricedown, ResText.Alignment.Centered) { DropShadow = true };

            // Primary loop. Will run repeatedly after plugin load.
            while (true)
            {
                vehicle = Game.LocalPlayer.LastVehicle; // Get the player's last vehicle, which includes any current vehicle.
                if (vehicle.Exists() && vehicle.Driver == Game.LocalPlayer.Character) // If player is driving a vehicle...
                {
                    // Check if the user is requesting a repair.
                    if (Game.IsKeyDown(repairKey))
                    {
                        vehicle.Repair();
                        Game.DisplayNotification($"Vehicle repaired");
                    }
                    // Moving onto the police-specific stuff...
                    if (vehicle.IsPoliceVehicle)
                    {
                        // Check if user is trying to blip.
                        if (Game.IsKeyDown(blipKey))
                        {
                            vehicle.BlipSiren(true); // Using false does not allow blip to occur if emergency lights are on. Using true allows blip to inturrupt siren, but also allows blip with lights active.
                        }

                        if (useIndicator) // Check that the user has enabled the indicator. If not, no sense in dealing with processing it.
                        {
                            if (lastSwap + swapFrequency < Game.TickCount) // Check if we need to update the indicator
                            {
                                indicator.Caption = ""; // Reset the caption to allow new caption to be made
                                if (vehicle.IsSirenOn) // "Siren" in this context is simply the Emergency Lights.
                                {
                                    indicator.Caption += swap ? lightsOnText : lightsOnTextAlt;
                                    indicator.Caption += vehicle.IsSirenSilent ? sirenOffText : sirenOnText; // Note the reversed logic. "IsSirenSilent" seems to return TRUE if the siren wail is active and false if it's silenced.
                                    indicator.Caption += swap ? lightsOnTextAlt : lightsOnText;
                                    indicator.Color = redAndBlue ? (swap ? Color.Red : Color.Blue) : Color.White;
                                }
                                else // Emergency lights are off
                                {
                                    indicator.Caption += lightsOffText;
                                    indicator.Caption += vehicle.IsSirenSilent ? sirenOffText : sirenOnText;
                                    indicator.Caption += lightsOffText;
                                    indicator.Color = Color.Gray;
                                }
                                lastSwap = Game.TickCount;
                                swap = !swap;
                            }
                            // Draw the indicator. Note this will only be drawn when in a police vehicle.
                            // When the user is not in a police vehicle, the indicator will not be re-drawn and is effectively hidden.
                            indicator.Draw();
                        }
                    }
                }
                GameFiber.Yield(); // Yield to other processes. Without yielding, this plugin will take 100% of the processing indefinitely due to the while loop.
            }
        }
    }
}