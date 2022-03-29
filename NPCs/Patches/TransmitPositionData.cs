// -----------------------------------------------------------------------
// <copyright file="TransmitPositionData.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Patches
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="PlayerPositionManager.TransmitData"/> to prevent the updating of networked positions on npcs.
    /// </summary>
    [HarmonyPatch(typeof(PlayerPositionManager), nameof(PlayerPositionManager.TransmitData))]
    internal static class TransmitPositionData
    {
        private static List<GameObject> GetPlayers => PlayerManager.players.Where(gameObject => !NpcBase.Dictionary.ContainsKey(gameObject)).ToList();

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Ldsfld);

            newInstructions.RemoveAt(index);
            newInstructions.Insert(index, new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(TransmitPositionData), nameof(GetPlayers))));

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}