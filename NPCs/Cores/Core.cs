// -----------------------------------------------------------------------
// <copyright file="Core.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Cores
{
    using NPCs.API;

    /// <summary>
    /// Defines the contract for an npc core.
    /// </summary>
    public abstract class Core
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core"/> class.
        /// </summary>
        /// <param name="npc"><inheritdoc cref="Npc"/></param>
        protected Core(Npc npc)
        {
            Npc = npc;
        }

        /// <summary>
        /// Gets the <see cref="API.Npc"/> being controlled.
        /// </summary>
        protected Npc Npc { get; }
    }
}