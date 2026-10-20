using HarmonyLib;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * Prevents the distance activator from running, since it causes immense lag
     * and makes some levels unplayable.
     * </summary>
     */
    internal static class DisableDistanceActivator {
        /**
         * <summary>
         * Disables the distance activator on ST and eldris.
         * </summary>
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DistanceActivator), "Start")]
        [HarmonyPatch(typeof(DistanceActivator), "ForceCheck")]
        private static bool DisableDA() {
            return !Config.disableDA.Value;
        }


        /**
         * <summary>
         * Disables the distance activator before it can do anything.
         * </summary>
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CustomLevel_DistanceActivator), "InitializeObjects")]
        private static void DisableDACustom(CustomLevel_DistanceActivator __instance) {
            if (Config.disableDACustom.Value == false) {
                return;
            }

            __instance.UseDistanceActivator = false;
        }

        /**
         * <summary>
         * Prevents the distance activator from modifying object states.
         * </summary>
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CustomLevel_DistanceActivator), "UpdateObjectStates")]
        private static bool DisableDACustomUpdates() {
            if (Config.disableDACustom.Value == false) {
                return true;
            }

            return false;
        }
    }
}
