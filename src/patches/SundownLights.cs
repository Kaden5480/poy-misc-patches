using HarmonyLib;
using UnityEngine;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * Patches the sundown idol to only affect the sun, not every light
     * in the scene.
     * </summary>
     */
    internal static class SundownLights {
        /**
         * <summary>
         * Restore previous intensities for most lights.
         * </summary>
         */
        [HarmonyPostfix]
        [HarmonyPatch(typeof(EnterPeakScene), "CustomLevel_SetSundown")]
        private static void RestoreLights(float[] ___custom_originalLightIntensities) {
            if (Config.sundownLights.Value == false) {
                return;
            }

            if (___custom_originalLightIntensities == null) {
                return;
            }

            Light[] lights = GameObject.FindObjectsOfType<Light>();
            for (int i = 0; i < lights.Length; i++) {
                Light light = lights[i];

                // Ignore the sun
                if (light.type == LightType.Directional) {
                    continue;
                }

                light.intensity = ___custom_originalLightIntensities[i];
            }
        }
    }
}
