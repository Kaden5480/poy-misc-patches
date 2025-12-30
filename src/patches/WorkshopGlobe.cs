using HarmonyLib;
using UILib.Patches;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * Patches the globe in the workshop so it correctly
     * handles `esc`, without bringing up the `InGameMenu`.
     *
     * This hooks into UILib's navigation lock.
     * </summary>
     */
    internal static class WorkshopGlobe {
        private static Lock @lock;

        /**
         * <summary>
         * Frees the lock.
         * </summary>
         */
        private static void Unlock() {
            if (@lock == null) {
                return;
            }

            @lock.Close();
            @lock = null;
        }

        /**
         * <summary>
         * Opens/closes navigation locks depending on
         * whether the player is in the globe or not.
         * </summary>
         */
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CustomPeakJournal), "ToggleCompendiumLibraryUIPanel")]
        private static void GlobeFix(CustomPeakJournal __instance) {
            // Check if disabled
            if (Config.workshopGlobe.Value == false) {
                // Also free the lock
                Unlock();
                return;
            }

            bool panelActive = __instance.compendiumUIPanel.activeInHierarchy;

            // Acquire lock
            if (panelActive == true && @lock == null) {
                @lock = new Lock(LockMode.Navigation);
            }

            // Free lock
            if (panelActive == false && @lock != null) {
                Unlock();
            }
        }
    }
}
