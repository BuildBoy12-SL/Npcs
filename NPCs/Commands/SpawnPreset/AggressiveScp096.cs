// -----------------------------------------------------------------------
// <copyright file="AggressiveScp096.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Commands.SpawnPreset
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using UnityEngine;

    /// <summary>
    /// Spawns an <see cref="NPCs.Presets.AggressiveScp096"/> on a player.
    /// </summary>
    public class AggressiveScp096 : ICommand
    {
        /// <inheritdoc />
        public string Command { get; } = "scp096";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "096" };

        /// <inheritdoc />
        public string Description { get; } = "Spawns an aggressive Scp096 on a player.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("rr.spawnpresets"))
            {
                response = "Insufficient permission.";
                return false;
            }

            Player player;
            if (arguments.Count == 0)
                player = Player.Get(sender);
            else
                player = Player.Get(arguments.At(0));

            if (player == null)
            {
                response = "Could not find the specified player.";
                return false;
            }

            Presets.AggressiveScp096 scp096 = new Presets.AggressiveScp096(RoleType.Scp096, "Aggressive Scp096", Vector3.one);
            scp096.Position = player.Position;
            response = $"Spawned an {nameof(AggressiveScp096)}";
            return true;
        }
    }
}