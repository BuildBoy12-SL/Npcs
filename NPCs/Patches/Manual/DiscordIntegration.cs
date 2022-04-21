// -----------------------------------------------------------------------
// <copyright file="DiscordIntegration.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Patches.Manual
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using Exiled.Loader;
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using UnityEngine;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Contains the Harmony patches for the Discord Integration plugin.
    /// </summary>
    internal class DiscordIntegration : ICompatibilityClass
    {
        /// <inheritdoc/>
        public void Patch(Harmony harmony)
        {
            Assembly assembly = Loader.GetPlugin("DiscordIntegration")?.Assembly;
            if (assembly is null)
                return;

            MethodInfo updateActivity = assembly.GetType("DiscordIntegration.API.Configs.Bot+<UpdateActivity>d__28")?.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
            if (updateActivity is not null)
                harmony.Patch(updateActivity, transpiler: new HarmonyMethod(typeof(DiscordIntegration).GetMethod(nameof(ActivityCount), BindingFlags.NonPublic | BindingFlags.Static)));
        }

        /// <summary>
        /// Patches DiscordIntegration to subtract the npc count from the player count in the bot's status.
        /// </summary>
        private static IEnumerable<CodeInstruction> ActivityCount(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);
            int index = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Box);
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