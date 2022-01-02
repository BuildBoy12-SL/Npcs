// -----------------------------------------------------------------------
// <copyright file="TargetLostBehavior.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Enums
{
    /// <summary>
    /// Indicates the behavior an npc should take when a given target is lost.
    /// </summary>
    public enum TargetLostBehavior
    {
        /// <summary>
        /// Indicates the npc should stop moving.
        /// </summary>
        Stop,

        /// <summary>
        /// Indicates the npc should teleport to its target.
        /// </summary>
        Teleport,

        /// <summary>
        /// Indicates the npc should wander to search for its target.
        /// </summary>
        Search,
    }
}