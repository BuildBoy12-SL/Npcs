// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets
{
    using System.ComponentModel;
    using System.IO;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using Pets.Commands;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the base permission for users to have pets.
        /// </summary>
        [Description("The base permission for users to have pets.")]
        public string BasePermission { get; set; } = "pets.pet";

        /// <summary>
        /// Gets or sets the path to the folder that will store the preferences.
        /// </summary>
        [Description("The path to the folder that will store the preferences.")]
        public string FolderPath { get; set; } = Path.Combine(Paths.Configs, "Pets");

        /// <summary>
        /// Gets or sets the name of the file containing the preferences.
        /// </summary>
        [Description("The name of the file containing the preferences.")]
        public string File { get; set; } = "PetPreferences.yml";

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Hide"/> command.
        /// </summary>
        public Hide HideCommand { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Item"/> command.
        /// </summary>
        public Item ItemCommand { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Name"/> command.
        /// </summary>
        public Name NameCommand { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Role"/> command.
        /// </summary>
        public Role RoleCommand { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Scale"/> command.
        /// </summary>
        public Scale ScaleCommand { get; set; } = new();

        /// <summary>
        /// Gets or sets a configurable instance of the <see cref="Item"/> command.
        /// </summary>
        public Show ShowCommand { get; set; } = new();
    }
}