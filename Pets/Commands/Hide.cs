// -----------------------------------------------------------------------
// <copyright file="Hide.cs" company="Build">
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
    /// Hides the user's pet.
    /// </summary>
    public class Hide : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "hide";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "h" };

        /// <inheritdoc />
        public string Description { get; set; } = "Hides the user's pet";

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
        /// Gets or sets the response to provide to the user when the pet is already hidden.
        /// </summary>
        [Description("The response to provide to the user when the pet is already hidden.")]
        public string PetNotSpawnedResponse { get; set; } = "You do not have a spawned pet!";

        /// <summary>
        /// Gets or sets the response to provide to the user when the pet is successfully hidden.
        /// </summary>
        [Description("The response to provide to the user when the pet is successfully hidden.")]
        public string SuccessResponse { get; set; } = "Despawned your pet.";

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

            Pet pet = Pet.Get(player);
            if (pet is null || !pet.IsSpawned)
            {
                response = PetNotSpawnedResponse;
                return false;
            }

            pet.IsSpawned = false;
            response = SuccessResponse;
            return false;
        }
    }
}