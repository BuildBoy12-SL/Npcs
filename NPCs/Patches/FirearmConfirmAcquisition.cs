// -----------------------------------------------------------------------
// <copyright file="FirearmConfirmAcquisition.cs" company="Build">
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
    using InventorySystem.Items.Firearms;
    using Mirror;
    using NorthwoodLib.Pools;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="Firearm.ServerConfirmAcqusition"/> to prevent confirming if the connection is null.
    /// </summary>
    [HarmonyPatch(typeof(Firearm), nameof(Firearm.ServerConfirmAcqusition))]
    internal static class FirearmConfirmAcquisition
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label continueLabel = generator.DefineLabel();

            const int offset = 1;
            int index = newInstructions.FindIndex(instruction => instruction.OperandIs(PropertyGetter(typeof(NetworkBehaviour), nameof(NetworkBehaviour.connectionToClient)))) + offset;
            newInstructions[index].labels.Add(continueLabel);

            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Brtrue_S, continueLabel),
                new CodeInstruction(OpCodes.Pop),
                new CodeInstruction(OpCodes.Ret),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}