using HarmonyLib;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * Patches TimeAttack in custom levels so PBs are set
     * correctly.
     * </summary>
     */
    internal static class CustomLevelTA {
        /**
         * <summary>
         * Patches `ParseTimeToSeconds` which currently misses
         * the 100s/10s of milliseconds in the time achieved.
         *
         * This is why PBs don't always update.
         * </summary>
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TimeAttack), "ParseTimeToSeconds")]
        private static bool CheckFix(string timeString, ref float __result) {
            // Normal execution if disabled
            if (Config.customLevelTA.Value == false) {
                return true;
            }

            string[] split = timeString.Split(':');

            if (split.Length < 4) {
                __result = float.MaxValue;
            }
            else {
                int hours    = int.Parse(split[0]);
                int minutes  = int.Parse(split[1]);
                int seconds  = int.Parse(split[2]);
                float millis = float.Parse(split[3]);

                __result = (hours*3600)
                    + (minutes*60)
                    + seconds
                    + millis/100f;
            }

            return false;
        }
    }
}
