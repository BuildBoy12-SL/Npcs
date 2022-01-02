// -----------------------------------------------------------------------
// <copyright file="Scp173Update.cs" company="Build">
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
    using PlayableScps;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="Scp173.OnUpdate"/> to prevent 173 from shitting itself.
    /// </summary>
    [HarmonyPatch(typeof(Scp173), nameof(Scp173.OnUpdate))]
    internal static class Scp173Update
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label returnLabel = generator.DefineLabel();
            int index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Ret);
            newInstructions[index].labels.Add(returnLabel);

            index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Ret) + 1;
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(NpcBase), nameof(NpcBase.Dictionary))).MoveLabelsFrom(newInstructions[index]),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(Scp173), nameof(Scp173.Hub))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ReferenceHub), nameof(ReferenceHub.gameObject))),
                new CodeInstruction(OpCodes.Callvirt, Method(typeof(Dictionary<GameObject, NpcBase>), nameof(Dictionary<GameObject, NpcBase>.ContainsKey))),
                new CodeInstruction(OpCodes.Brtrue_S, returnLabel),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}