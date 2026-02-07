using Galaxy.Api;
using HarmonyLib;
using Steamworks;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * Patches wally mode/avian chaos rewards so they don't depend on a save file
     * and can instead use achievements.
     * </summary>
     */
    internal static class AvianAchievements {
        // The maximum progress for wally mode and avian chaos
        const int avianMax = 37;
        const int wallyMax = 37;

        // The names of the wally mode and avian chaos achievements
        const string wallyKey = "ACH_BIRDHUNT";
        const string avianKey = "ACH_CROWHUNT";

        /**
         * <summary>
         * Gets an achievement from either GOG or Steam.
         * </summary>
         */
        private static bool GetAchievement(string key) {
            bool unlocked = false;

            if (GOGAchievements.galaxyManagerActive == true) {
                uint unlockTime = 0u;
                GalaxyInstance.Stats().GetAchievement(key, ref unlocked, ref unlockTime);
            }
            else if (SteamManager.Initialized == true) {
                SteamUserStats.GetAchievement(key, out unlocked);
            }

            return unlocked;
        }

        /**
         * <summary>
         * Looks at achievements to determine whether wally mode/avian chaos
         * has been completed.
         * </summary>
         */
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CheckBirdProgress), "CheckProgress")]
        private static void CheckAchievements() {
            if (Config.avianAchievements.Value == false) {
                return;
            }

            if (GetAchievement(wallyKey) == true) {
                CheckBirdProgress.birdProgress = wallyMax;
            }

            if (GetAchievement(avianKey) == true) {
                CheckBirdProgress.crowProgress = avianMax;
            }
        }
    }
}
