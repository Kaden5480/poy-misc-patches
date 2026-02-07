using System;

using HarmonyLib;

using MiscPatches.Patches;

namespace MiscPatches {
    /**
     * <summary>
     * Handles applying patches.
     * </summary>
     */
    internal static class Patcher {
        private static Logger logger = new Logger(typeof(Patcher));

        /**
         * <summary>
         * Applies a patch of type logging if successful.
         * </summary>
         */
        private static void Apply(Type type) {
            Harmony.CreateAndPatchAll(type);
            logger.LogDebug($"Successfully applied patch: {type}");
        }

        /**
         * <summary>
         * Applies patches.
         * </summary>
         */
        internal static void Patch() {
            Apply(typeof(AvianAchievements));
            Apply(typeof(CustomLevelStamps));
            Apply(typeof(CustomLevelTA));
            Apply(typeof(DisableOriginShift));
            Apply(typeof(NorthernCabinTA));
            Apply(typeof(SummitStats));
            Apply(typeof(SundownLights));
            Apply(typeof(WorkshopGlobe));
        }
    }
}
