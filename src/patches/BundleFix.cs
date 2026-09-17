using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using HarmonyLib;
using UnityEngine;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * A patch which fixes extreme performance issues with custom assets in
     * peak editor levels.
     *
     * Peaks of Yore usually reloads the asset bundle every time a new custom
     * asset is being placed, which tanks performance.
     *
     * This set of patches caches the asset bundles and reuses them instead.
     * </summary>
     */
    internal static class BundleFix {
        /**
         * <summary>
         * The currently loaded custom asset bundles.
         * </summary>
         */
        private static Dictionary<string, AssetBundle> loadedBundles
            = new Dictionary<string, AssetBundle>();

        /**
         * <summary>
         * The injected method which replaces the normal asset bundle
         * loading logic.
         *
         * When BundleFix is enabled, this caches bundles as they're loaded
         * in, to prevent them from being reloaded unnecessarily.
         * </summary>
         */
        private static AssetBundle LoadBundleInject(string path) {
            if (Config.bundleFix.Value == false) {
                return AssetBundle.LoadFromFile(path);
            }

            if (loadedBundles.TryGetValue(path, out AssetBundle bundle)) {
                return bundle;
            }

            bundle = AssetBundle.LoadFromFile(path);
            if (bundle != null) {
                loadedBundles.Add(path, bundle);
            }

            return bundle;
        }

        /**
         * <summary>
         * The injected method which replaces the normal asset bundle
         * unloading logic.
         *
         * This prevents bundles from being unnecessarily unloaded when
         * BundleFix is enabled.
         * </summary>
         */
        private static void UnloadBundleInject(AssetBundle bundle) {
            if (Config.bundleFix.Value == false) {
                bundle.Unload(false);
            }
        }

        /**
         * <summary>
         * Ensures any custom asset bundles are unloaded when the scene is unloaded.
         * Also wipes the dictionary of asset bundles.
         * </summary>
         */
        internal static void SceneUnload() {
            foreach (AssetBundle bundle in loadedBundles.Values) {
                if (bundle == null) {
                    continue;
                }

                bundle.Unload(false);
            }

            loadedBundles.Clear();
        }

        /**
         * <summary>
         * Applies the patches to custom assets and collectibles to
         * ensure that bundles are cached, instead of being reloaded
         * for each object.
         * </summary>
         */
        [HarmonyTranspiler]
        [HarmonyPatch(MethodType.Enumerator)]
        [HarmonyPatch(typeof(BundleObject), "LoadBundle")]
        [HarmonyPatch(typeof(E_CustomCollectibleBundleObject), "LoadBundle")]
        private static IEnumerable<CodeInstruction> InjectMethod(
            IEnumerable<CodeInstruction> insts
        ) {
            MethodInfo loadFromFile = AccessTools.Method(
                typeof(AssetBundle), nameof(AssetBundle.LoadFromFile),
                new Type[] { typeof(string) }
            );

            MethodInfo unload = AccessTools.Method(
                typeof(AssetBundle), nameof(AssetBundle.Unload)
            );

            MethodInfo loadBundleInject = AccessTools.Method(
                typeof(BundleFix), nameof(BundleFix.LoadBundleInject)
            );

            MethodInfo unloadBundleInject = AccessTools.Method(
                typeof(BundleFix), nameof(BundleFix.UnloadBundleInject)
            );

            IEnumerable<CodeInstruction> result = Helper.Replace(insts,
                new[] {
                    new CodeInstruction(OpCodes.Call, loadFromFile),
                },
                new[] {
                    new CodeInstruction(OpCodes.Call, loadBundleInject),
                }
            );

            return Helper.Replace(result,
                new[] {
                    new CodeInstruction(OpCodes.Ldloc_2, null),
                    new CodeInstruction(OpCodes.Ldc_I4_0, null),
                    new CodeInstruction(OpCodes.Callvirt, unload),
                },
                new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldloc_2, null),
                    new CodeInstruction(OpCodes.Call, unloadBundleInject),
                }
            );
        }
    }
}
