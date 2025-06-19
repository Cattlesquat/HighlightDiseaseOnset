using System;
using System.Collections.Generic;
using HarmonyLib;
using XRL.UI;

namespace Cattlesquat_HighlightDiseaseOnset.HarmonyPatches
{
    [HarmonyPatch(typeof(XRL.Messages.MessageQueue))]
    class Cattlesquat_HighlightDiseaseOnset
    {
        private static List<String> DiseaseMessages = new List<String>()
        {
            "Your skin itches.",
            "You feel a bit better.",
            "Your throat feels sore.",
            "You feel better.",
            "Your legs ache at the joints.",
            "Your vision blurs.",
            "Your vision clears up.",
            "Color starts to seep into the world."
        };

        private static bool LoopProtection = false;
        
        [HarmonyPrefix]
        [HarmonyPatch(nameof(XRL.Messages.MessageQueue.AddPlayerMessage), new Type[] { typeof(string), typeof(string), typeof(bool) } )]
        static bool Prefix(ref string Message, ref string Color, ref bool Capitalize)
        {
            if (LoopProtection) return false; //BR// Popup can sometimes call AddPlayerMessage - we don't want to get trapped in infinite loop
            
            if (DiseaseMessages.Contains(Message))
            {
                Color = "R";

                if (Options.GetOptionBool("OptionHighlightDiseaseOnset"))
                {
                    LoopProtection = true;
                    XRL.UI.Popup.Show(Message);
                    LoopProtection = false;
                }
            }

            return true;
        }
    }
}