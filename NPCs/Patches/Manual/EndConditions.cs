// -----------------------------------------------------------------------
// <copyright file="EndConditions.cs" company="Build">
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
    /// Contains the Harmony patches for the EndConditions plugin.
    /// </summary>
    internal class EndConditions : IManualPatch
    {
        /// <inheritdoc/>
        public void Patch(Harmony harmony)
        {
            Assembly assembly = Loader.GetPlugin("EndConditions")?.Assembly;
            if (assembly is null)
                return;

            MethodInfo getRoles = assembly.GetType("EndConditions.Methods+<GetRoles>d__2")?.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
            if (getRoles is not null)
                harmony.Patch(getRoles, transpiler: new HarmonyMethod(typeof(EndConditions).GetMethod(nameof(GetRoles), BindingFlags.NonPublic | BindingFlags.Static)));
        }

        /// <summary>
        /// Patches EndConditions to skip the inclusion of NPC roles.
        /// </summary>
        private static IEnumerable<CodeInstruction> GetRoles(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Br);
            Label continueLabel = (Label)newInstructions[index].operand;

            index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Ldloc_3);
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldloc_3),
                new CodeInstruction(OpCodes.Call, Method(typeof(Extensions), nameof(Extensions.IsNpc), new[] { typeof(Player) })),
                new CodeInstruction(OpCodes.Brtrue_S, continueLabel),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}