// -----------------------------------------------------------------------
// <copyright file="AddPlayerVSR.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Patches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HarmonyLib;
    using RemoteAdmin;
    using UnityEngine;

    /// <summary>
    /// Patches <see cref="PlayerManager.AddPlayer"/> Fix VSR Issue for NPCs being counted in ServerList.
    /// </summary>
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.AddPlayer))]
    internal static class AddPlayerVSR
    {
        private static bool Prefix(GameObject player, int maxPlayers)
        {
            GameCore.Console.AddDebugLog("PLIST", $"[PlayerManager] AddPlayer: {player.GetComponent<NicknameSync>().MyNick} Max Slots: {maxPlayers}", MessageImportance.LessImportant, false);
            if (!PlayerManager.players.Contains(player))
            {
                PlayerManager.players.Add(player);
                ServerConsole.PlayersAmount = PlayerManager.players.Count(x => x.GetComponent<QueryProcessor>()._ipAddress is not "127.0.0.WAN");
                ServerConsole.PlayersListChanged = true;
            }

            IdleMode.SetIdleMode(false);

            return false;
        }
    }
}
