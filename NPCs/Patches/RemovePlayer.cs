﻿// -----------------------------------------------------------------------
// <copyright file="RemovePlayer.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Patches
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using NPCs.API;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="PlayerManager.RemovePlayer"/> to reduce the total player count from the amount of spawned npcs.
    /// </summary>
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.RemovePlayer))]
    internal static class RemovePlayer
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);
            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Stsfld);
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Npc), nameof(Npc.Dictionary))),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Dictionary<GameObject, Npc>), nameof(Dictionary<GameObject, Npc>.Count))),
                new CodeInstruction(OpCodes.Sub),
            });

            index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Brtrue_S);
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Npc), nameof(Npc.Dictionary))),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Dictionary<GameObject, Npc>), nameof(Dictionary<GameObject, Npc>.Count))),
                new CodeInstruction(OpCodes.Sub),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}