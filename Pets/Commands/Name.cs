// -----------------------------------------------------------------------
// <copyright file="Name.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets.Commands
{
    using System;
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
        public string Command => "name";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "n" };

        /// <inheritdoc />
        public string Description => "Sets the pet's name";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("pets.name"))
            {
                response = "Insufficient permission. Required permission: pets.name";
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
                response = $"Your pets name is currently '{preferences.Name}'";
                return true;
            }

            string name = string.Join(" ", arguments);
            if (Plugin.Instance.Config.BlacklistedNames.Any(blacklistedString => name.Contains(blacklistedString)))
            {
                response = "This name is blacklisted.";
                return false;
            }

            if (Pet.Get(player) is Pet pet)
                pet.Name = string.Join(" ", arguments);
            else
                preferences.Name = name;

            response = $"Set your pet's name to '{preferences.Name}'";
            return true;
        }
    }
}