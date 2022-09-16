﻿// -----------------------------------------------------------------------
// <copyright file="StartQueryProcessor.cs" company="Build">
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
    using NPCs.API.Extensions;
    using RemoteAdmin;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Prevents an NRE caused by <see cref="QueryProcessor.Start"/> from more accessing of connectionToClient on npcs.
    /// </summary>
    [HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.Start))]
    internal static class StartQueryProcessor
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int index = newInstructions.FindIndex(instruction => instruction.OperandIs(Field(typeof(QueryProcessor), nameof(QueryProcessor._ipAddress)))) + 1;

            Label skipConnectionLabel = generator.DefineLabel();
            newInstructions[index].labels.Add(skipConnectionLabel);

            index -= 9;

            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Component), nameof(Component.gameObject))),
                new CodeInstruction(OpCodes.Call, Method(typeof(NpcExtensions), nameof(NpcExtensions.IsNpc), new[] { typeof(GameObject) })),
                new CodeInstruction(OpCodes.Brtrue_S, skipConnectionLabel),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}