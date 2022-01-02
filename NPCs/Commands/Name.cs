// -----------------------------------------------------------------------
// <copyright file="Name.cs" company="Build">
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
    /// Sets the pet's name.
    /// </summary>
    public class Name : ICommand
    {
        /// <inheritdoc />
        public string Command { get; } = "name";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "n" };

        /// <inheritdoc />
        public string Description { get; } = "Sets the pet's name";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("npcs.pets.name"))
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

            if (arguments.Count == 0 || string.IsNullOrWhiteSpace(arguments.At(0)))
            {
                response = "Please specify the pet's new name.";
                return false;
            }

            Pet pet = player.GetPet();
            if (pet == null)
                pet = Pet.Create(player);

            pet.Name = string.Join(" ", arguments);
            response = $"Set your pet's name to {pet.Name}.";
            return true;
        }
    }
}