using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeHeartDancePartner
{
    public class ModConfig
    {
        protected static IModHelper Helper => Mod.Instance.Helper;
        protected static IMonitor Monitor => Mod.Instance.Monitor;
        internal static ModConfig Instance { get; private set; }
        internal static void Load()
        {
            Instance = Helper.ReadConfig<ModConfig>();
        }

        // Config option for dance partner acceptance required heart level
        public int HeartsRequiredForDancePartner
        {
            get 
            {
                Monitor.LogOnce($"HeartsRequiredForDancePartner is currently set to [{_heartsRequiredForDancePartner}].\n" +
                        $"The vanilla game requires 4 hearts. The default for this mod is 3.", LogLevel.Info);
                return _heartsRequiredForDancePartner; 
            }
            set
            {
                if (value < 0)
                {
                    Monitor.Log($"Invalid config value [{value}] for HeartsRequiredForDancePartner.\n" +
                        $"You must enter a non-negative integer.\n" +
                        $"HeartsRequiredForDancePartner has been reset to previous value [{_heartsRequiredForDancePartner}] hearts.", LogLevel.Warn);
                }
                else
                {
                    _heartsRequiredForDancePartner = value;
                    if (value >= 10)
                    {
                        Monitor.Log($"ARE YOU SURE? Unusual config value [{value}] for HeartsRequiredForDancePartner.\n" +
                            $"The maximum possible heart level is 10 for villagers, 14 for spouses.\n" +
                            $"If you're sure, you can safely ignore this message.", LogLevel.Warn);
                    }
                }
            }
        }
        private static readonly int _heartsRequiredDefault = 3;
        private int _heartsRequiredForDancePartner = _heartsRequiredDefault;
    }
}
