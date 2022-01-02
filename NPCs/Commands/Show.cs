// -----------------------------------------------------------------------
// <copyright file="Show.cs" company="Build">
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
    /// Shows the user's pet.
    /// </summary>
    public class Show : ICommand
    {
        /// <inheritdoc />
        public string Command { get; } = "show";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "s" };

        /// <inheritdoc />
        public string Description { get; } = "Shows the user's pet";

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

            if (player.IsDead || player.Role == RoleType.Scp079)
            {
                response = "You cannot summon your pet at this time.";
                return false;
            }

            Pet pet = player.GetPet();
            if (pet == null)
            {
                pet = Pet.Create(player);
                pet.Show();
                response = "Spawned your pet.";
                return true;
            }

            if (!pet.IsShown)
            {
                pet.Show();
                response = "Spawned your pet.";
                return true;
            }

            response = "Your pet has already been spawned!";
            return false;
        }
    }
}