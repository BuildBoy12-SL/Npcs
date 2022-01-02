// -----------------------------------------------------------------------
// <copyright file="ServerConsoleDisconnect.cs" company="Build">
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
    using Mirror;
    using NorthwoodLib.Pools;

    /// <summary>
    /// Patches <see cref="ServerConsole.Disconnect(NetworkConnection, string)"/> to prevent an NRE from accessing a null connection.
    /// </summary>
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.Disconnect), typeof(NetworkConnection), typeof(string))]
    internal static class ServerConsoleDisconnect
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label disconnectLabel = generator.DefineLabel();
            newInstructions[0].labels.Add(disconnectLabel);

            newInstructions.InsertRange(0, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Brtrue_S, disconnectLabel),
                new CodeInstruction(OpCodes.Ret),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}