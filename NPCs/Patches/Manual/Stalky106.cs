// -----------------------------------------------------------------------
// <copyright file="Stalky106.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Patches.Manual
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using Exiled.API.Features;
    using Exiled.Loader;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using NPCs.API;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Contains the Harmony patches for the Stalky106 plugin.
    /// </summary>
    internal class Stalky106 : IManualPatch
    {
        /// <inheritdoc/>
        public void Patch(Harmony harmony)
        {
            Assembly assembly = Loader.GetPlugin("Stalky106")?.Assembly;
            if (assembly is null)
                return;

            MethodInfo updateActivity = assembly.GetType("Stalky106.Methods+<StalkCoroutine>d__14")?.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
            if (updateActivity is not null)
                harmony.Patch(updateActivity, transpiler: new HarmonyMethod(typeof(Stalky106).GetMethod(nameof(Stalk), BindingFlags.NonPublic | BindingFlags.Static)));
        }

        private static IEnumerable<CodeInstruction> Stalk(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Br);
            Label continueLabel = (Label)newInstructions[index].operand;

            const int offset = 1;
            index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Stloc_S) + offset;
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldloc_S, 4),
                new CodeInstruction(OpCodes.Call, Method(typeof(Extensions), nameof(Extensions.IsNpc), new[] { typeof(Player) })),
                new CodeInstruction(OpCodes.Brtrue_S, continueLabel),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}