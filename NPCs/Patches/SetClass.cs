// -----------------------------------------------------------------------
// <copyright file="SetClass.cs" company="Build">
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
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="PlayerMovementSync.OnPlayerClassChange"/> to only set the position of npcs.
    /// </summary>
    [HarmonyPatch(typeof(PlayerMovementSync), nameof(PlayerMovementSync.OnPlayerClassChange))]
    internal static class SetClass
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label continueLabel = generator.DefineLabel();
            newInstructions[0].labels.Add(continueLabel);

            newInstructions.InsertRange(0, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(PlayerMovementSync), nameof(PlayerMovementSync.gameObject))),
                new CodeInstruction(OpCodes.Call, Method(typeof(NpcExtensions), nameof(NpcExtensions.IsNpc), new[] { typeof(GameObject) })),
                new CodeInstruction(OpCodes.Brfalse_S, continueLabel),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldarg_2),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Callvirt, Method(typeof(PlayerMovementSync), nameof(PlayerMovementSync.OverridePosition), new[] { typeof(Vector3), typeof(PlayerMovementSync.PlayerRotation?), typeof(bool) })),
                new CodeInstruction(OpCodes.Ret),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}