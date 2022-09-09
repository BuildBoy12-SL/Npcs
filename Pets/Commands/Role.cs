// -----------------------------------------------------------------------
// <copyright file="Role.cs" company="Build">
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
    /// Sets the pet's <see cref="RoleType"/>.
    /// </summary>
    public class Role : ICommand
    {
        /// <inheritdoc />
        public string Command => "role";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "r" };

        /// <inheritdoc />
        public string Description => "Sets the pet's role.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player is null)
            {
                response = "Console has no need for menial things such as pets.";
                return false;
            }

            Pet pet = Pet.Get(player);
            if (pet is null)
            {
                response = "You do not have a spawned pet.";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = $"Your pet is currently a {pet.Role}";
                return true;
            }

            if (!Enum.TryParse(arguments.At(0), true, out RoleType roleType) || Plugin.Instance.Config.BlacklistedRoles.Contains(roleType))
            {
                response = "Please specify a valid role.";
                return false;
            }

            string requiredPermission = "pets.role." + roleType.ToString().ToLower();
            if (!sender.CheckPermission(requiredPermission))
            {
                response = $"Insufficient permission. Required permission: {requiredPermission}";
                return false;
            }

            pet.Role = roleType;
            response = $"Set the pet's role to a(n) {roleType}";
            return true;
        }
    }
}