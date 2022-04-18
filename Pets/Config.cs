// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets
{
    using System.IO;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using UnityEngine;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the default name of a pet.
        /// </summary>
        public string DefaultName { get; set; } = "{Name}'s Pet";

        /// <summary>
        /// Gets or sets the default size of a pet.
        /// </summary>
        public Vector3 DefaultSize { get; set; } = new(0.4f, 0.4f, 0.4f);

        /// <summary>
        /// Gets or sets the path to the folder that will store the preferences.
        /// </summary>
        public string FolderPath { get; set; } = Path.Combine(Paths.Configs, "Pets");

        /// <summary>
        /// Gets or sets the name of the file containing the preferences.
        /// </summary>
        public string File { get; set; } = "PetPreferences.yml";
    }
}