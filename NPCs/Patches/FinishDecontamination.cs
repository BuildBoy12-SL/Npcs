// -----------------------------------------------------------------------
// <copyright file="FinishDecontamination.cs" company="Build">
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
    using LightContainmentZoneDecontamination;
    using NorthwoodLib.Pools;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="DecontaminationController.FinishDecontamination"/> in an attempt to prevent an NRE.
    /// </summary>
    // [HarmonyPatch(typeof(DecontaminationController), nameof(DecontaminationController.FinishDecontamination))]
    internal static class FinishDecontamination
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);
            const int offset = -2;
            int index = newInstructions.FindIndex(instruction => instruction.OperandIs(Field(typeof(ReferenceHub), nameof(ReferenceHub.characterClassManager)))) + offset;

            Label continueLabel = (Label)newInstructions[index].operand;

            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(NpcBase), nameof(NpcBase.Dictionary))),
                new CodeInstruction(OpCodes.Ldloc_3),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(ReferenceHub), nameof(ReferenceHub.gameObject))),
                new CodeInstruction(OpCodes.Callvirt, Method(typeof(Dictionary<GameObject, NpcBase>), nameof(Dictionary<GameObject, NpcBase>.ContainsKey))),
                new CodeInstruction(OpCodes.Brtrue_S, continueLabel),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}