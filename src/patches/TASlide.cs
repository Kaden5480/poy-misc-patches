using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using HarmonyLib;
using UnityEngine;

namespace MiscPatches.Patches {
    /**
     * <summary>
     * Patches Time Attack to prevent sliding at the summit
     * after completing a run without bringing up the scoreboard.
     * </summary>
     */
    internal static class TASlide {
        private static void DisableMovementInject(TimeAttack timeAttack) {
            if (Config.taSlide.Value == true) {
                return;
            }

            PeakSummited summit = Helper.GetFieldValue<TimeAttack, PeakSummited>(
                timeAttack, "summit"
            );

            summit.DisablePlayerMovement(false);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(TimeAttack), "Update")]
        private static IEnumerable<CodeInstruction> Patch(
            IEnumerable<CodeInstruction> insts
        ) {
            FieldInfo summitInfo = AccessTools.Field(
                typeof(TimeAttack), "summit"
            );

            MethodInfo disableMovementInfo = AccessTools.Method(
                typeof(PeakSummited),
                nameof(PeakSummited.DisablePlayerMovement)
            );

            MethodInfo disableMovementInjectInfo = AccessTools.Method(
                typeof(TASlide), nameof(DisableMovementInject)
            );

            return Helper.Replace(insts,
                new[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, summitInfo),
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Callvirt, disableMovementInfo),
                },
                new[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, disableMovementInjectInfo),
                }
            );
        }
    }
}
