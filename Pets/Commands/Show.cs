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
    using Pets.API;

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
                response = "Insufficient permission. Required permission: pets.pet";
                return false;
            }

            Player player = Player.Get(sender);
            if (player is null)
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
            if (pet is not null && pet.IsSpawned)
            {
                response = "Your pet has already been spawned!";
                return false;
            }

            pet ??= Pet.Create(player);
            pet.IsSpawned = true;
            response = $"Spawned your pet named {pet.Name}.";
            return true;
        }
    }
}