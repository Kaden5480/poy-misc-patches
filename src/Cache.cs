using UILib.Patches;
using UnityEngine;

namespace MiscPatches {
    internal static class Cache {
        internal static LevelEditorManager levelEditorManager { get; private set; }

        /**
         * <summary>
         * Caches objects required for some patches to work.
         * </summary>
         */
        private static void FindObjects() {
            levelEditorManager = GameObject.FindObjectOfType<LevelEditorManager>();
        }

        /**
         * <summary>
         * Clears the cache.
         * </summary>
         */
        private static void Clear() {
            levelEditorManager = null;
        }

        /**
         * <summary>
         * Initializes entry points for caching.
         * </summary>
         */
        internal static void Init() {
            SceneType types = SceneType.BuiltIn
                | SceneType.Custom | SceneType.Editor;

            SceneLoads.AddLoadListener(delegate {
                FindObjects();
            }, types);

            SceneLoads.AddUnloadListener(delegate {
                Clear();
            }, types);
        }
    }
}
