using System;
using System.Reflection.Emit;
using System.Collections.Generic;

using HarmonyLib;

namespace MiscPatches {
    public class Helper {
        /**
         * <summary>
         * Checks whether two ldloc.s instructions are equivalent.
         * </summary>
         * <param name="a">The first instruction</param>
         * <param name="b">The second instruction</param>
         */
        private static bool LdlocEqual(CodeInstruction a, CodeInstruction b) {
            Type typeA = a.operand.GetType();
            Type typeB = b.operand.GetType();

            if (typeA == typeB && typeA == typeof(LocalBuilder)) {
                return a.operand.Equals(b.operand);
            }

            if (typeA == typeof(LocalBuilder)) {
                return ((LocalBuilder) a.operand).LocalIndex == (int) b.operand;
            }

            if (typeB == typeof(LocalBuilder)) {
                return ((LocalBuilder) b.operand).LocalIndex == (int) a.operand;
            }

            return false;
        }

        /**
         * <summary>
         * Compare two instructions for equivalence.
         * </summary>
         * <param name="a">The first instruction to compare</param>
         * <param name="b">The second instruction to compare</param>
         */
        public static bool InstsEqual(CodeInstruction a, CodeInstruction b) {
            // If either are null, always match
            if (a == null || b == null) {
                return true;
            }

            // Check opcodes
            if (a.opcode != b.opcode) {
                return false;
            }

            // Check null operands
            if (a.operand == null || b.operand == null) {
                return true;
            }

            // Specific check for Ldloc_S
            if (a.opcode == OpCodes.Ldloc_S) {
                return LdlocEqual(a, b);
            }

            // Check operand equivalence
            return a.operand.Equals(b.operand);
        }

        /**
         * <summary>
         * Find the locations of sequences of instructions.
         * </summary>
         * <param name="insts">The instructions to search within</param>
         * <param name="pattern">The sequence to search for</param>
         * <return>The indices the instructions start at</return>
         */
        public static IEnumerable<int> FindSeqs(
            List<CodeInstruction> insts,
            CodeInstruction[] pattern
        ) {
            int beginning = 0;
            int patternIndex = 0;

            for (int i = 0; i < insts.Count; i++) {
                // If fully matched, return beginning of the sequence
                if (patternIndex >= pattern.Length) {
                    yield return beginning;

                    // Also reset
                    beginning = i;
                    patternIndex = 0;
                }

                // Check if this instruction matches the pattern
                if (InstsEqual(pattern[patternIndex], insts[i]) == false) {
                    // Reset values
                    beginning = i + 1;
                    patternIndex = 0;
                }
                else {
                    // Increase pattern index
                    patternIndex++;
                }
            }
        }

        /**
         * <summary>
         * Find the first location of a sequence of instructions.
         * </summary>
         * <param name="insts">The instructions to search within</param>
         * <param name="pattern">The sequence to search for</param>
         * <return>The index the instructions start at, -1 if not found</return>
         */
        public static int FindSeq(
            List<CodeInstruction> instructions,
            CodeInstruction[] pattern
        ) {
            foreach (int index in FindSeqs(instructions, pattern)) {
                return index;
            }

            return -1;
        }

        /**
         * <summary>
         * Given a sequence of instructions, find a pattern and replace
         * it with the provided sequence.
         * </summary>
         * <param name="instructions">The instructions to search in</param>
         * <param name="pattern">The pattern to search for</param>
         * <param name="replacement">What to replace the pattern with</param>
         * <returns>The patched instructions</returns>
         */
        public static IEnumerable<CodeInstruction> Replace(
            IEnumerable<CodeInstruction> instructions,
            CodeInstruction[] pattern,
            CodeInstruction[] replacement
        ) {
            List<CodeInstruction> buffer = new List<CodeInstruction>();
            int patternIndex = 0;

            // If empty pattern, return normally
            if (pattern.Length < 1) {
                foreach (CodeInstruction instruction in instructions) {
                    yield return instruction;
                }

                yield break;
            }

            foreach (var instruction in instructions) {
                // If pattern matched, return the replacement
                if (patternIndex >= pattern.Length) {
                    // Move all labels to first instruction
                    foreach (CodeInstruction buffered in buffer) {
                        buffered.MoveLabelsTo(replacement[0]);
                    }

                    foreach (var replace in replacement) {
                        yield return replace;
                    }

                    yield return instruction;

                    patternIndex = 0;
                    buffer.Clear();

                    continue;
                }

                // If the pattern isn't fully matched, return
                // all buffered instructions normally
                if (InstsEqual(instruction, pattern[patternIndex]) == false) {
                    foreach (var buffered in buffer) {
                        yield return buffered;
                    }

                    yield return instruction;

                    patternIndex = 0;
                    buffer.Clear();

                    continue;
                }

                // Otherwise, store matching instructions
                buffer.Add(instruction);
                patternIndex++;
            }
        }

        /**
         * <summary>
         * Convert an instruction to a string.
         * </summary>
         * <param name="inst">The instruction to convert</param>
         * <return>The instruction as a string</return>
         */
        public static string InstToString(CodeInstruction inst) {
            if (inst == null || inst.opcode == null) {
                return "Inst was null";
            }

            if (inst.operand != null) {
                return $"{inst.opcode}, {inst.operand}";
            }

            return $"{inst.opcode}";
        }
    }
}
