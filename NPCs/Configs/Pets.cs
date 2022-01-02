// -----------------------------------------------------------------------
// <copyright file="Pets.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Configs
{
    using UnityEngine;

    /// <summary>
    /// Handles configs for pet modifications.
    /// </summary>
    public class Pets
    {
        /// <summary>
        /// Gets or sets the default name of a pet.
        /// </summary>
        public string DefaultName { get; set; } = "{Name}'s Pet";

        /// <summary>
        /// Gets or sets the size of all pets.
        /// </summary>
        public Vector3 Size { get; set; } = new Vector3(0.4f, 0.4f, 0.4f);
    }
}