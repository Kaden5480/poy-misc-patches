using HarmonyLib;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * Prevents the distance activator from running, since it causes immense lag
     * and makes some levels unplayable.
     *
     * Currently this is limited to the custom level distance activator.
     * </summary>
     */
    internal static class DisableDistanceActivator {
        /**
         * <summary>
         * Disables the distance activator before it can do anything.
         * </summary>
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CustomLevel_DistanceActivator), "InitializeObjects")]
        private static void DisableActivator(CustomLevel_DistanceActivator __instance) {
            if (Config.disableDistanceActivator.Value == false) {
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
        private static bool DisableUpdates() {
            if (Config.disableDistanceActivator.Value == false) {
                return true;
            }

            return false;
        }
    }
}
