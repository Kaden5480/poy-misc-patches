using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using HarmonyLib;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * Patches stamps in custom levels to behave properly.
     * </summary>
     */
    internal static class CustomLevelStamps {
        private static string customLevelName = null;

        /**
         * <summary>
         * Allows injecting a custom path to load stamp data from.
         * </summary>
         */
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(StamperPeakSummit), "SetStampForPage")]
        private static IEnumerable<CodeInstruction> InjectPath(
            IEnumerable<CodeInstruction> insts
        ) {
            FieldInfo controlInfo = AccessTools.Field(
                typeof(CustomLevelManager), nameof(CustomLevelManager.control)
            );
            FieldInfo peakNameInfo = AccessTools.Field(
                typeof(CustomLevelManager), nameof(CustomLevelManager.peakName)
            );

            FieldInfo customLevelInfo = AccessTools.Field(
                typeof(CustomLevelStamps), nameof(customLevelName)
            );

            IEnumerable<CodeInstruction> newInsts = Helper.Replace(insts,
                new[] {
                    new CodeInstruction(OpCodes.Ldsfld, controlInfo),
                    new CodeInstruction(OpCodes.Ldfld, peakNameInfo),
                    new CodeInstruction(OpCodes.Ldstr, ".es3"),
                },
                new[] {
                    new CodeInstruction(OpCodes.Ldsfld, customLevelInfo),
                    new CodeInstruction(OpCodes.Ldstr, ".es3"),
                }
            );

            foreach (CodeInstruction inst in newInsts) {
                Plugin.LogDebug(Helper.InstToString(inst));
                yield return inst;
            }
        }

        /**
         * <summary>
         * Fixes the stamping so it doesn't decide to stamp both pages.
         * </summary>
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StamperPeakSummit), "SetStampForPage")]
        private static void FixStamping(
            string levelName, bool isRightPage, ref bool isStamping
        ) {
            customLevelName = levelName;

            if (Config.customLevelStamps.Value == false) {
                return;
            }

            // If this page is the right one, do nothing else
            // If not stamping, also ignore
            if (isRightPage == true || isStamping == false) {
                return;
            }

            // Check the level name
            if (levelName == null) {
                return;
            }

            // Update isStamping
            isStamping = levelName.Equals(CustomLevelManager.control.peakName);
        }
    }
}
