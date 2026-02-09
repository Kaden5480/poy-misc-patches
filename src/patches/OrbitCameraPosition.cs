using System.Reflection;

using HarmonyLib;
using UnityEngine;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * Patches the orbit camera in the level editor to restore
     * to the expected position.
     * </summary>
     */
    internal static class OrbitCameraPosition {
        private static FieldInfo cachedPositionInfo = AccessTools.Field(
            typeof(LevelEditorManager), "cachedOrbitCameraPosition"
        );

        /**
         * <summary>
         * Just use the player's camera holder position, don't apply an offset,
         * that's just silly.
         * </summary>
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(LevelEditorManager), "LerpPlayerToOrbitCamera", MethodType.Enumerator)]
        private static void ResetOffset() {
            if (Config.orbitCameraPosition.Value == false) {
                return;
            }

            if (Cache.levelEditorManager == null) {
                return;
            }

            cachedPositionInfo.SetValue(Cache.levelEditorManager, Vector3.zero);
        }
    }
}
