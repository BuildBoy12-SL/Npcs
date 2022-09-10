// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets
{
    using System.Collections.Generic;
    using System.ComponentModel;
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
        [Description("The default name of a pet.")]
        public string DefaultName { get; set; } = "{Name}'s Pet";

        /// <summary>
        /// Gets or sets the default size of a pet.
        /// </summary>
        [Description("The default size of a pet.")]
        public Vector3 DefaultSize { get; set; } = new(0.4f, 0.4f, 0.4f);

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
        /// Gets or sets the roles that players should not be able to set their pets to.
        /// </summary>
        [Description("The roles that players should not be able to set their pets to.")]
        public List<RoleType> BlacklistedRoles { get; set; } = new()
        {
            RoleType.None,
            RoleType.Spectator,
            RoleType.Scp079,
            RoleType.Scp096,
        };

        /// <summary>
        /// Gets or sets a collection of strings that pet names cannot contain.
        /// </summary>
        [Description("A collection of strings that pet names cannot contain.")]
        public List<string> BlacklistedNames { get; set; } = new()
        {
            "InsertSlurHere",
        };
    }
}