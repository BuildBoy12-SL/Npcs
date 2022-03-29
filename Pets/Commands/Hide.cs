// -----------------------------------------------------------------------
// <copyright file="Hide.cs" company="Build">
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
    /// Hides the user's pet.
    /// </summary>
    public class Hide : ICommand
    {
        /// <inheritdoc />
        public string Command => "hide";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "h" };

        /// <inheritdoc />
        public string Description => "Hides the user's pet";

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

            Pet pet = Pet.Create(player);
            if (pet == null || !pet.IsShown)
            {
                response = "You do not have a spawned pet!";
                return false;
            }

            pet.IsShown = false;
            response = "Despawned your pet.";
            return false;
        }
    }
}