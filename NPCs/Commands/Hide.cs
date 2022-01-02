// -----------------------------------------------------------------------
// <copyright file="Hide.cs" company="Build">
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
    using NPCs.API;

    /// <summary>
    /// Hides the user's pet.
    /// </summary>
    public class Hide : ICommand
    {
        /// <inheritdoc />
        public string Command { get; } = "hide";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "h" };

        /// <inheritdoc />
        public string Description { get; } = "Hides the user's pet";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("npcs.pets"))
            {
                response = "Insufficient permission.";
                return false;
            }

            Player player = Player.Get(sender);
            if (player == null)
            {
                response = "Console has no need for menial things such as pets.";
                return false;
            }

            Pet pet = player.GetPet();
            if (pet == null || !pet.IsShown)
            {
                response = "You do not have a spawned pet!";
                return false;
            }

            pet.Hide();
            response = "Despawned your pet.";
            return false;
        }
    }
}