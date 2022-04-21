// -----------------------------------------------------------------------
// <copyright file="RoundEnd.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Patches.Manual
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using Exiled.Loader;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using NPCs.API;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="RoundSummary.Start"/> to implement <see cref="Process"/> to ignore npcs when ending the round.
    /// </summary>
    internal class RoundEnd : IManualPatch
    {
        private static int PlayerCount => PlayerManager.players.Count(gameObject => !gameObject.IsNpc());

        /// <inheritdoc/>
        public void Patch(Harmony harmony)
        {
            Assembly assembly = Loader.GetPlugin("Exiled.Events")?.Assembly;
            if (assembly is null)
                return;

            MethodInfo updateActivity = assembly.GetType("Exiled.Events.Patches.Events.Server.RoundEnd+<Process>d__0")?.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
            if (updateActivity is not null)
                harmony.Patch(updateActivity, transpiler: new HarmonyMethod(typeof(RoundEnd).GetMethod(nameof(Process), BindingFlags.NonPublic | BindingFlags.Static)));
        }

        private static IEnumerable<CodeInstruction> Process(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            int index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Br);
            Label continueLabel = (Label)newInstructions[index].operand;

            const int offset = -1;
            index = newInstructions.FindIndex(instruction => instruction.OperandIs(PropertyGetter(typeof(KeyValuePair<GameObject, ReferenceHub>), nameof(KeyValuePair<GameObject, ReferenceHub>.Value)))) + offset;
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldloca_S, 9),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(KeyValuePair<GameObject, ReferenceHub>), nameof(KeyValuePair<GameObject, ReferenceHub>.Key))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Extensions), nameof(Extensions.IsNpc), new[] { typeof(GameObject) })),
                new CodeInstruction(OpCodes.Brtrue_S, continueLabel),
            });

            index = newInstructions.FindIndex(instruction => instruction.OperandIs(Field(typeof(PlayerManager), nameof(PlayerManager.players))));
            newInstructions.RemoveRange(index, 2);
            newInstructions.Insert(index, new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(RoundEnd), nameof(PlayerCount))));

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}