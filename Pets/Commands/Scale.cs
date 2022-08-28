// -----------------------------------------------------------------------
// <copyright file="Scale.cs" company="Build">
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
    using UnityEngine;

    /// <inheritdoc />
    public class Scale : ICommand
    {
        /// <inheritdoc />
        public string Command => "scale";

        /// <inheritdoc />
        public string[] Aliases { get; } = Array.Empty<string>();

        /// <inheritdoc />
        public string Description => "Scales the pet.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("pets.scale"))
            {
                response = "Insufficient permission. Required: pets.scale";
                return false;
            }

            Player player = Player.Get(sender);
            if (player is null)
            {
                response = "Console has no need for menial things such as pets.";
                return false;
            }

            Pet pet = Pet.GetOrCreate(player);
            if (arguments.Count == 0)
            {
                response = $"Your pet's scale is currently {pet.Scale}";
                return true;
            }

            if (arguments.Count < 3)
            {
                response = "Usage: pet scale <x> <y> <z>";
                return false;
            }

            if (!float.TryParse(arguments.At(0), out float x))
            {
                response = $"Invalid value for x size: {arguments.At(1)}";
                return false;
            }

            if (!float.TryParse(arguments.At(1), out float y))
            {
                response = $"Invalid value for y size: {arguments.At(2)}";
                return false;
            }

            if (!float.TryParse(arguments.At(2), out float z))
            {
                response = $"Invalid value for z size: {arguments.At(3)}";
                return false;
            }

            Vector3 clampedScale = ClampScale(x, y, z);
            pet.Scale = clampedScale;
            response = $"Set the pet's scale to {clampedScale}";
            return true;
        }

        private static Vector3 ClampScale(float x, float y, float z)
        {
            x = Mathf.Clamp(x, 0.3f, 0.6f);
            y = Mathf.Clamp(y, 0.3f, 0.6f);
            z = Mathf.Clamp(z, 0.3f, 0.6f);
            return new Vector3(x, y, z);
        }
    }
}