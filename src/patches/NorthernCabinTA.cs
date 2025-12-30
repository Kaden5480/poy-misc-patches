using HarmonyLib;
using UnityEngine.SceneManagement;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * Patches `TimeAttack` in the northern cabin.
     * Usually, `TimeAttack` spams null reference exceptions
     * constantly, which clogs up the logs.
     *
     * It also has another null reference exception when calling `CheckRecords`.
     * </summary>
     */
    internal static class NorthernCabinTA {
        /**
         * <summary>
         * Patches the null reference exception in the northern cabin
         * due to TimeAttack.Update.
         * </summary>
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TimeAttack), "Update")]
        private static bool TAUpdate(TimeAttack __instance) {
            // Normal execution if disabled
            if (Config.northernCabinTA.Value == false) {
                return true;
            }

            // Prevent execution in the northern cabin
            return __instance.isCabin4 == false;
        }

        /**
         * <summary>
         * Patches the null reference exception in the northern cabin
         * due to TimeAttack.CheckRecords.
         * </summary>
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TimeAttack), "CheckRecords")]
        private static bool TACheck() {
            // Normal execution if disabled
            if (Config.northernCabinTA.Value == false) {
                return true;
            }

            // Prevent execution in the northern cabin
            return "Category4_1_Cabin".Equals(
                SceneManager.GetActiveScene().name
            ) == false;
        }
    }
}
