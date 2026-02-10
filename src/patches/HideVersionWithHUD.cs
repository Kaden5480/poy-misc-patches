using HarmonyLib;
using PlayerPrefs = UnityEngine.PlayerPrefs;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * Hides the version number when the HUD is disabled.
     * </summary>
     */
    internal static class HideVersionWithHUD {
        /**
         * <summary>
         * Determines whether to hide the version number.
         * If it should be hidden, hides it.
         * If it should be shown, shows it.
         * </summary>
         * <param name="enabled">Whether this patch is enabled</param>
         */
        internal static void UpdateVersion(bool enabled) {
            // These are the default options
            bool showHUD = true;
            bool showVersion = false;

            // Determine whether the HUD should be shown
            if (PlayerPrefs.HasKey("HUDSetting") == true) {
                showHUD = PlayerPrefs.GetInt("HUDSetting") != 0;
            }

            // Determine whether the version number should be shown
            if (PlayerPrefs.HasKey("InGameVersionVisibility") == true) {
                showVersion = PlayerPrefs.GetInt("InGameVersionVisibility") != 0;
            }

            // If this patch is disabled, restore the default settings
            if (enabled == false && showVersion == true) {
                Cache.inGameVersionsHolder.SetActive(true);
                return;
            }

            // If the HUD should be shown, show the version number
            if (showHUD == true
                && showVersion == true
                && Cache.inGameVersionsHolder != null
            ) {
                Cache.inGameVersionsHolder.SetActive(true);
            }

            // If the HUD should be hidden, also hide the version number
            if (showHUD == false
                && Cache.inGameVersionsHolder != null
            ) {
                Cache.inGameVersionsHolder.SetActive(false);
            }
        }

        /**
         * <summary>
         * Uses `LoadSavedOptions` and `ToggleHUD` as an entry point for
         * determining current settings.
         * </summary>
         */
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Button), "LoadSavedOptions")]
        [HarmonyPatch(typeof(GraphicsOptions), "ToggleHUD")]
        private static void LoadVersion() {
            UpdateVersion(Config.hideVersionWithHUD.Value);
        }
    }
}
