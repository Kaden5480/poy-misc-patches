using HarmonyLib;
using UnityEngine;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * Disables the origin shifter to deal with some really broken behaviour.
     * </summary>
     */
    internal static class DisableOriginShift {
        /**
         * <summary>
         * Prevents the origin shifter from recentering.
         * </summary>
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(OriginShift), "DoRecenter")]
        private static bool PreventRecenter() {
            if (Config.disableOriginShift.Value == false) {
                return true;
            }

            return false;
        }
    }
}
