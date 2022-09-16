﻿// -----------------------------------------------------------------------
// <copyright file="SetScale.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Patches
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using Exiled.API.Features;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using NPCs.API.Extensions;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="Player.Scale"/> to prevent network garbage with npcs.
    /// </summary>
    [HarmonyPatch(typeof(Player), nameof(Player.Scale), MethodType.Setter)]
    internal static class SetScale
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Br_S);
            Label continueLabel = (Label)newInstructions[index].operand;

            const int offset = 1;
            index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Stloc_1) + offset;
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldloc_1),
                new CodeInstruction(OpCodes.Call, Method(typeof(NpcExtensions), nameof(NpcExtensions.IsNpc), new[] { typeof(Player) })),
                new CodeInstruction(OpCodes.Brtrue_S, continueLabel),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}