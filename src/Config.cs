using BepInEx.Configuration;
using ModMenu.Config;

namespace MiscPatches {
    /**
     * <summary>
     * Holds the config for this mod.
     * </summary>
     */
    internal static class Config {
        [Field("Avian Achievements")]
        internal static ConfigEntry<bool> avianAchievements { get; private set; }

        [Field("Custom Level Stamps")]
        internal static ConfigEntry<bool> customLevelStamps { get; private set; }

        [Field("Custom Level TA")]
        internal static ConfigEntry<bool> customLevelTA { get; private set; }

        [Field("Disable Origin Shift")]
        internal static ConfigEntry<bool> disableOriginShift { get; private set; }

        [Field("Northern Cabin TA")]
        internal static ConfigEntry<bool> northernCabinTA { get; private set; }

        [Field("Summit Stats")]
        internal static ConfigEntry<bool> summitStats { get; private set; }

        [Field("Sundown Lights")]
        internal static ConfigEntry<bool> sundownLights { get; private set; }

        [Field("Workshop Globe")]
        internal static ConfigEntry<bool> workshopGlobe { get; private set; }

        /**
         * <summary>
         * Initializes the config by binding to the
         * provided `ConfigFile`.
         * </summary>
         * <param name="configFile">The config file to bind to</param>
         */
        internal static void Init(ConfigFile configFile) {
            avianAchievements = configFile.Bind(
                "Patches", "avianAchievements", true,
                "Whether to fix wally mode and avian chaos rewards so they can use"
                + " your achievements, instead of relying solely on your save data."
            );

            customLevelStamps = configFile.Bind(
                "Patches", "customLevelStamps", true,
                "Whether to fix stamping in custom levels so the stamper doesn't"
                + " stamp both pages sometimes."
            );

            customLevelTA = configFile.Bind(
                "Patches", "customLevelTA", true,
                "Whether TimeAttack in custom levels should be patched so PBs"
                + " get updated correctly."
            );

            disableOriginShift = configFile.Bind(
                "Patches", "disableOriginShift", true,
                "Disables the origin shifter because it's overcomplicated and breaks stuff."
            );

            northernCabinTA = configFile.Bind(
                "Patches", "northernCabinTA", true,
                "Whether to prevent TimeAttack in the northern cabin spamming exceptions"
                + " every frame in the logs."
            );

            summitStats = configFile.Bind(
                "Patches", "summitStats", true,
                "Whether to forcefully load global summit stats. This prevents the"
                + " total summit count (found in stats) from being reset in certain cases."
            );

            sundownLights = configFile.Bind(
                "Patches", "sundownLights", true,
                "Whether to prevent certain lights in sundown from being disabled."
                + " This will still disable the sun, it just prevents other lights from being disabled."
            );

            workshopGlobe = configFile.Bind(
                "Patches", "workshopGlobe", true,
                "Whether to patch the workshop globe so it correctly handles `esc`,"
                + " rather than bringing up the pause menu."
            );
        }
    }
}
