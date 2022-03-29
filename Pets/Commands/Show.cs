// -----------------------------------------------------------------------
// <copyright file="Show.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;

    /// <summary>
    /// Shows the user's pet.
    /// </summary>
    public class Show : ICommand
    {
        /// <inheritdoc />
        public string Command => "show";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "s" };

        /// <inheritdoc />
        public string Description => "Shows the user's pet";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("pets.pet"))
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

            Pet pet = Pet.Create(player);
            if (!pet.IsShown)
            {
                pet.IsShown = true;
                response = "Spawned your pet.";
                return true;
            }

            response = "Your pet has already been spawned!";
            return false;
        }
    }
}