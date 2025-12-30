using BepInEx.Configuration;
using ModMenu.Config;

namespace MiscPatches {
    /**
     * <summary>
     * Holds the config for this mod.
     * </summary>
     */
    internal static class Config {
        [Field("Workshop Globe")]
        internal static ConfigEntry<bool> workshopGlobe { get; private set; }

        [Field("Northern Cabin TA")]
        internal static ConfigEntry<bool> northernCabinTA { get; private set; }

        [Field("Custom Level TA")]
        internal static ConfigEntry<bool> customLevelTA { get; private set; }

        /**
         * <summary>
         * Initializes the config by binding to the
         * provided `ConfigFile`.
         * </summary>
         * <param name="configFile">The config file to bind to</param>
         */
        internal static void Init(ConfigFile configFile) {
            workshopGlobe = configFile.Bind(
                "Patches", "workshopGlobe", true,
                "Whether to patch the workshop globe so it correctly handles `esc`,"
                + " rather than bringing up the pause menu."
            );

            northernCabinTA = configFile.Bind(
                "Patches", "northernCabinTA", true,
                "Whether to prevent TimeAttack in the northern cabin spamming exceptions"
                + " every frame in the logs."
            );

            customLevelTA = configFile.Bind(
                "Patches", "customLevelTA", true,
                "Whether TimeAttack in custom levels should be patched so PBs"
                + " get updated correctly."
            );
        }
    }
}
