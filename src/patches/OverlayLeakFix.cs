using HarmonyLib;
using Steamworks;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * ControllerConnectEvents registers a callback on every scene
     * load (Awake) for when the steam overlay is activated.
     *
     * It never disposes of these callbacks, so they just keep
     * piling up over time as more scenes are loaded.
     * </summary>
     */
    internal static class OverlayLeakFix {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ControllerConnectEvents), "OnDestroy")]
        private static void DisposeCallback(
            Callback<GameOverlayActivated_t> ___m_GameOverlayActivated
        ) {
            if (Config.overlayLeakFix.Value == false) {
                return;
            }

            if (___m_GameOverlayActivated != null) {
                ___m_GameOverlayActivated.Dispose();
            }
        }
    }
}
