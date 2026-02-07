using System;
using HarmonyLib;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * Patches `GameManager` to load in all stats correctly.
     * </summary>
     */
    internal static class SummitStats {
        /**
         * <summary>
         * Loads in summits manually, otherwise they keep getting reset.
         * </summary>
         */
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameManager), "LoadAllStats")]
        private static void LoadSummits(GameManager __instance) {
            if (Config.summitStats.Value == false) {
                return;
            }

            const string path = "global_stats.es3";
            ES3Settings settings = new ES3Settings(path, ES3.Location.Cache);

            try {
                ES3.CacheFile(path, settings);
            }
            catch (Exception) {
                return;
            }

            if (ES3.KeyExists("global_stats_total_summits", settings) == true) {
                __instance.global_stats_total_summits = ES3.Load<int>(
                    "global_stats_total_summits", settings
                );
            }
        }
    }
}
