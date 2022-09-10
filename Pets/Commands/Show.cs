// -----------------------------------------------------------------------
// <copyright file="Show.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets.Commands
{
    using System;
    using System.ComponentModel;
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
        public string Command { get; set; } = "show";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "s" };

        /// <inheritdoc />
        public string Description { get; set; } = "Shows the user's pet";

        /// <summary>
        /// Gets or sets the permission required to use this command.
        /// </summary>
        [Description("The permission required to use this command.")]
        public string RequiredPermission { get; set; } = "pets.pet";

        /// <summary>
        /// Gets or sets the response to provide to the user that lacks the required permission.
        /// </summary>
        [Description("The response to provide to the user that lacks the required permission.")]
        public string RequiredPermissionResponse { get; set; } = "Insufficient permission. Required permission: pets.pet";

        /// <summary>
        /// Gets or sets the response to provide to the user when they are dead or are Scp079.
        /// </summary>
        [Description("The response to provide to the user when they are dead or are Scp079.")]
        public string CannotSpawnResponse { get; set; } = "You cannot summon your pet at this time.";

        /// <summary>
        /// Gets or sets the response to provide to the user when their pet is already shown.
        /// </summary>
        [Description("The response to provide to the user when their pet is already shown.")]
        public string PetSpawnedResponse { get; set; } = "Your pet has already been spawned!";

        /// <summary>
        /// Gets or sets the response to provide to the user when their pet is successfully shown.
        /// </summary>
        [Description("The response to provide to the user when their pet is successfully shown.")]
        public string SuccessResponse { get; set; } = "Spawned your pet.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(RequiredPermission))
            {
                response = RequiredPermissionResponse;
                return false;
            }

            Player player = Player.Get(sender);
            if (player is null || player == Server.Host)
            {
                response = "Console has no need for menial things such as pets.";
                return false;
            }

            if (player.IsDead || player.Role == RoleType.Scp079)
            {
                response = CannotSpawnResponse;
                return false;
            }

            Pet pet = Pet.Get(player);
            if (pet is not null && pet.IsSpawned)
            {
                response = PetSpawnedResponse;
                return false;
            }

            pet ??= Pet.Create(player);
            pet.IsSpawned = true;
            response = SuccessResponse;
            return true;
        }
    }
}