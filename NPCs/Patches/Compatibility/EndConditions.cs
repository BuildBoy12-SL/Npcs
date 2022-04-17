// -----------------------------------------------------------------------
// <copyright file="EndConditions.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Patches.Compatibility
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using Exiled.API.Features;
    using Exiled.Loader;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Contains the Harmony patches for the EndConditions plugin.
    /// </summary>
    internal class EndConditions : ICompatibilityClass
    {
        /// <summary>
        /// Attempts to patch the required methods.
        /// </summary>
        /// <param name="harmony">The harmony instance.</param>
        public void Patch(Harmony harmony)
        {
            Assembly assembly = Loader.GetPlugin("EndConditions")?.Assembly;
            if (assembly is null)
                return;

            MethodInfo getRoles = assembly.GetType("EndConditions.Methods+<GetRoles>d__2")?.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
            if (getRoles is not null)
                harmony.Patch(getRoles, transpiler: new HarmonyMethod(typeof(EndConditions).GetMethod(nameof(GetRoles), BindingFlags.NonPublic | BindingFlags.Static)));
        }

        /// <summary>
        /// Patches EndConditions to skip the inclusion of NPC roles.
        /// </summary>
        /// <param name="instructions">The original methods instructions.</param>
        /// <param name="generator">An instance of the <see cref="ILGenerator"/> class.</param>
        /// <returns>The new instructions.</returns>
        private static IEnumerable<CodeInstruction> GetRoles(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            Label continueLabel = generator.DefineLabel();

            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Ldloc_3);
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Npc), nameof(Npc.Dictionary))),
                new CodeInstruction(OpCodes.Ldloc_3),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Player), nameof(Player.GameObject))),
                new CodeInstruction(OpCodes.Callvirt, Method(typeof(Dictionary<GameObject, Npc>), nameof(Dictionary<GameObject, Npc>.ContainsKey))),
                new CodeInstruction(OpCodes.Brtrue_S, continueLabel),
            });

            index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Br);
            newInstructions[index].labels.Add(continueLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}