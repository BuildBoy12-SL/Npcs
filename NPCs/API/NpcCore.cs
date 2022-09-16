// -----------------------------------------------------------------------
// <copyright file="NpcCore.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API
{
    using Exiled.API.Features.Core;

    /// <summary>
    /// Defines the contract for an npc core.
    /// </summary>
    public abstract class NpcCore : TickComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NpcCore"/> class.
        /// </summary>
        /// <param name="npc"><inheritdoc cref="Npc"/></param>
        protected NpcCore(Npc npc)
        {
            Npc = npc;
        }

        /// <summary>
        /// Gets the <see cref="API.Npc"/> being controlled.
        /// </summary>
        protected Npc Npc { get; }

        /// <summary>
        /// Destroys the object.
        /// </summary>
        protected new virtual void Destroy() => base.Destroy();
    }
}