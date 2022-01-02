// -----------------------------------------------------------------------
// <copyright file="Role.cs" company="Build">
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
    /// Sets the pet's <see cref="RoleType"/>.
    /// </summary>
    public class Role : ICommand
    {
        private static readonly RoleType[] BlacklistedRoles =
        {
            RoleType.None,
            RoleType.Spectator,
            RoleType.Scp079,
            RoleType.Scp096,
        };

        /// <inheritdoc />
        public string Command { get; } = "role";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "r" };

        /// <inheritdoc />
        public string Description { get; } = "Sets the pet's role.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("npcs.pets.role"))
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
            if (pet == null)
                pet = Pet.Create(player);

            if (arguments.Count == 0)
            {
                response = $"Your pet is currently a {pet.Role}";
                return true;
            }

            if (!Enum.TryParse(arguments.At(0), true, out RoleType roleType) || BlacklistedRoles.Contains(roleType))
            {
                response = "Please specify a valid role.";
                return false;
            }

            pet.Role = roleType;
            response = $"Set the pet's role to a(n) {roleType}";
            return true;
        }
    }
}