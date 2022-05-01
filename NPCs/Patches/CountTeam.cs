// -----------------------------------------------------------------------
// <copyright file="CountTeam.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Patches
{
#pragma warning disable SA1313
    using System.Linq;
    using Exiled.API.Features;
    using HarmonyLib;
    using NPCs.API;

    /// <summary>
    /// Patches <see cref="RoundSummary.CountTeam"/> to ignore npcs.
    /// </summary>
    [HarmonyPatch(typeof(RoundSummary), nameof(RoundSummary.CountTeam))]
    internal static class CountTeam
    {
        private static void Postfix(Team team, ref int __result)
        {
            __result -= Player.List.Count(player => player.IsNpc() && player.Role.Team == team);
        }
    }
}