// -----------------------------------------------------------------------
// <copyright file="Role.cs" company="Build">
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
    /// Sets the pet's <see cref="RoleType"/>.
    /// </summary>
    public class Role : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "role";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "r" };

        /// <inheritdoc />
        public string Description { get; set; } = "Sets the pet's role.";

        /// <summary>
        /// Gets or sets the base permission required to use this command.
        /// </summary>
        [Description("The base permission required to use this command.")]
        public string RequiredPermission { get; set; } = "pets.role";

        /// <summary>
        /// Gets or sets the response to provide to the user when they lack permission for a specific role.
        /// </summary>
        [Description("The response to provide to the user when they lack permission for a specific role.")]
        public string RequiredRolePermissionResponse { get; set; } = "Insufficient permission. Required permission: {0}";

        /// <summary>
        /// Gets or sets the response to provide to the user when no arguments are provided.
        /// </summary>
        [Description("The response to provide to the user when no arguments are provided.")]
        public string NoArgumentsResponse { get; set; } = "Your pet is currently a '{0}'";

        /// <summary>
        /// Gets or sets the response to provide to the user when the specified role is invalid.
        /// </summary>
        [Description("The response to provide to the user when the specified role is invalid.")]
        public string InvalidRoleResponse { get; set; } = "Please specify a valid role.";

        /// <summary>
        /// Gets or sets the response to provide to the user when the pet's role is changed successfully.
        /// </summary>
        [Description("The response to provide to the user when the pet's role is changed successfully.")]
        public string SuccessResponse { get; set; } = "Set the pet's role to a {0}";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player is null || player == Server.Host)
            {
                response = "Console has no need for menial things such as pets.";
                return false;
            }

            PetPreferences preferences = PetPreferences.Get(player) ?? new PetPreferences(player.UserId);
            if (arguments.Count == 0)
            {
                response = string.Format(NoArgumentsResponse, preferences.Role);
                return true;
            }

            if (!Enum.TryParse(arguments.At(0), true, out RoleType roleType))
            {
                response = InvalidRoleResponse;
                return false;
            }

            string requiredPermission = RequiredPermission + '.' + roleType.ToString().ToLower();
            if (!sender.CheckPermission(requiredPermission))
            {
                response = string.Format(RequiredRolePermissionResponse, requiredPermission);
                return false;
            }

            if (Pet.Get(player) is Pet pet)
                pet.Role.Type = roleType;
            else
                preferences.Role = roleType;

            response = string.Format(SuccessResponse, roleType);
            return true;
        }
    }
}