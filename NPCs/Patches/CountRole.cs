// -----------------------------------------------------------------------
// <copyright file="CountRole.cs" company="Build">
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
    /// Patches <see cref="RoundSummary.CountRole"/> to skip npcs.
    /// </summary>
    [HarmonyPatch(typeof(RoundSummary), nameof(RoundSummary.CountRole))]
    internal static class CountRole
    {
        private static void Postfix(RoleType role, ref int __result)
        {
            __result -= Player.List.Count(player => player.IsNpc() && player.Role.Type == role);
        }
    }
}