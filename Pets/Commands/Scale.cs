// -----------------------------------------------------------------------
// <copyright file="Scale.cs" company="Build">
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
    using UnityEngine;

    /// <inheritdoc />
    public class Scale : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "scale";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = Array.Empty<string>();

        /// <inheritdoc />
        public string Description { get; set; } = "Scales the pet.";

        /// <summary>
        /// Gets or sets the permission required to use this command.
        /// </summary>
        [Description("The permission required to use this command.")]
        public string RequiredPermission { get; set; } = "pets.scale";

        /// <summary>
        /// Gets or sets the response to provide to the user that lacks the required permission.
        /// </summary>
        [Description("The response to provide to the user that lacks the required permission.")]
        public string RequiredPermissionResponse { get; set; } = "Insufficient permission. Required permission: pets.scale";

        /// <summary>
        /// Gets or sets the response to provide to the user when no arguments are provided.
        /// </summary>
        [Description("The response to provide to the user when no arguments are provided.")]
        public string NoArgumentsResponse { get; set; } = "Your pet's scale is currently {0}";

        /// <summary>
        /// Gets or sets the response to provide to the user when an insufficient amount of arguments are provided.
        /// </summary>
        [Description("The response to provide to the user when an insufficient amount of arguments are provided.")]
        public string UsageResponse { get; set; } = "Usage: pet scale <x> <y> <z>";

        /// <summary>
        /// Gets or sets the response to provide to the user when a non-numeric value is provided for the x value.
        /// </summary>
        [Description("The response to provide to the user when a non-numeric value is provided for the x value.")]
        public string InvalidXResponse { get; set; } = "Invalid value for x size: {0}";

        /// <summary>
        /// Gets or sets the response to provide to the user when a non-numeric value is provided for the y value.
        /// </summary>
        [Description("The response to provide to the user when a non-numeric value is provided for the y value.")]
        public string InvalidYResponse { get; set; } = "Invalid value for y size: {0}";

        /// <summary>
        /// Gets or sets the response to provide to the user when a non-numeric value is provided for the z value.
        /// </summary>
        [Description("The response to provide to the user when a non-numeric value is provided for the z value.")]
        public string InvalidZResponse { get; set; } = "Invalid value for z size: {0}";

        /// <summary>
        /// Gets or sets the minimum scale a pet can be set to.
        /// </summary>
        [Description("The minimum scale a pet can be set to.")]
        public float MinimumScale { get; set; } = 0.3f;

        /// <summary>
        /// Gets or sets the maximum scale a pet can be set to.
        /// </summary>
        [Description("The maximum scale a pet can be set to.")]
        public float MaximumScale { get; set; } = 0.6f;

        /// <summary>
        /// Gets or sets the response to provide to the user when their pet's scale is set successfully.
        /// </summary>
        [Description("The response to provide to the user when their pet's scale is set successfully.")]
        public string SuccessResponse { get; set; } = "Your pet's scale has been set to {0}";

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

            PetPreferences preferences = PetPreferences.Get(player.UserId) ?? new PetPreferences(player.UserId);
            if (arguments.Count == 0)
            {
                response = string.Format(NoArgumentsResponse, preferences.Scale);
                return true;
            }

            if (arguments.Count < 3)
            {
                response = UsageResponse;
                return false;
            }

            if (!float.TryParse(arguments.At(0), out float x))
            {
                response = string.Format(InvalidXResponse, arguments.At(1));
                return false;
            }

            if (!float.TryParse(arguments.At(1), out float y))
            {
                response = string.Format(InvalidYResponse, arguments.At(2));
                return false;
            }

            if (!float.TryParse(arguments.At(2), out float z))
            {
                response = string.Format(InvalidZResponse, arguments.At(3));
                return false;
            }

            Vector3 clampedScale = ClampScale(x, y, z);
            if (Pet.Get(player) is Pet pet)
                pet.Scale = clampedScale;
            else
                preferences.Scale = clampedScale;

            response = string.Format(SuccessResponse, clampedScale);
            return true;
        }

        private Vector3 ClampScale(float x, float y, float z)
        {
            x = Mathf.Clamp(x, MinimumScale, MaximumScale);
            y = Mathf.Clamp(y, MinimumScale, MaximumScale);
            z = Mathf.Clamp(z, MinimumScale, MaximumScale);
            return new Vector3(x, y, z);
        }
    }
}