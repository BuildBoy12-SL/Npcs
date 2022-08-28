// -----------------------------------------------------------------------
// <copyright file="Destroy.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;

    /// <inheritdoc />
    public class Destroy : ICommand
    {
        /// <inheritdoc />
        public string Command => "destroy";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "d" };

        /// <inheritdoc />
        public string Description => "Destroys an NPC.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("npc.destroy"))
            {
                response = "You do not have permission to run this command.";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage: npc destroy <Npc>";
                return false;
            }

            Player player = Player.Get(string.Join(" ", arguments));
            if (player is not Npc npc)
            {
                response = "Could not find an NPC with the specified parameters.";
                return false;
            }

            npc.Destroy();
            response = $"Destroyed NPC: {npc}.";
            return true;
        }
    }
}