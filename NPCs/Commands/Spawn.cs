// -----------------------------------------------------------------------
// <copyright file="Spawn.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Commands
{
    using System;
    using System.Linq;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using MEC;
    using UnityEngine;

    /// <inheritdoc />
    public class Spawn : ICommand
    {
        /// <inheritdoc />
        public string Command => "spawn";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "s" };

        /// <inheritdoc />
        public string Description => "Spawns an NPC.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("npc.spawn"))
            {
                response = "You do not have permission to run this command.";
                return false;
            }

            if (Player.Get(sender) is not Player player || player == Server.Host)
            {
                response = "This command must be executed at the game level.";
                return false;
            }

            if (arguments.Count < 1 || !Enum.TryParse(arguments.At(0), true, out RoleType roleType))
                roleType = RoleType.ClassD;

            string name = "NPC";
            if (arguments.Count > 1)
                name = string.Join(" ", arguments.Skip(1));

            Npc npc = new(roleType, name, Vector3.one)
            {
                IsSpawned = true,
            };

            Timing.CallDelayed(0.1f, () =>
            {
                npc.Position = player.Position;
                npc.Rotation = player.Rotation;
            });

            response = "Done.";
            return true;
        }
    }
}