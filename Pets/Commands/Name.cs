// -----------------------------------------------------------------------
// <copyright file="Name.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using Pets.API;

    /// <summary>
    /// Sets the pet's name.
    /// </summary>
    public class Name : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "name";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "n" };

        /// <inheritdoc />
        public string Description { get; set; } = "Sets the pet's name";

        /// <summary>
        /// Gets or sets the permission required to use this command.
        /// </summary>
        [Description("The permission required to use this command.")]
        public string RequiredPermission { get; set; } = "pets.name";

        /// <summary>
        /// Gets or sets the response to provide to the user that lacks the required permission.
        /// </summary>
        [Description("The response to provide to the user that lacks the required permission.")]
        public string RequiredPermissionResponse { get; set; } = "Insufficient permission. Required permission: pets.name";

        /// <summary>
        /// Gets or sets the response to provide to the user when no arguments are provided.
        /// </summary>
        [Description("The response to provide to the user when no arguments are provided.")]
        public string NoArgumentsResponse { get; set; } = "Your pet's name is currently '{0}'";

        /// <summary>
        /// Gets or sets a collection of strings that pet names cannot contain.
        /// </summary>
        [Description("A collection of strings that pet names cannot contain.")]
        public List<string> BlacklistedNames { get; set; } = new()
        {
            "InsertSlurHere",
        };

        /// <summary>
        /// Gets or sets the response to the user when the name they provided is blacklisted.
        /// </summary>
        [Description("The response to the user when the name they provided is blacklisted.")]
        public string BlacklistedResponse { get; set; } = "This name is blacklisted.";

        /// <summary>
        /// Gets or sets the response to the user when the pet's name is set successfully.
        /// </summary>
        [Description("The response to the user when the pet's name is set successfully.")]
        public string SuccessResponse { get; set; } = "Your pet's name has been set to '{0}'";

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

            PetPreferences preferences = PetPreferences.Get(player) ?? new PetPreferences(player.UserId);
            if (arguments.Count == 0)
            {
                response = string.Format(NoArgumentsResponse, preferences.Name);
                return true;
            }

            string name = string.Join(" ", arguments);
            if (BlacklistedNames.Any(blacklistedString => name.Contains(blacklistedString)))
            {
                response = BlacklistedResponse;
                return false;
            }

            if (Pet.Get(player) is Pet pet)
                pet.Name = string.Join(" ", arguments);
            else
                preferences.Name = name;

            response = string.Format(SuccessResponse, name);
            return true;
        }
    }
}