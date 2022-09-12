// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API
{
    using Exiled.API.Features;
    using UnityEngine;

    /// <summary>
    /// Various extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Checks if a <see cref="Player"/> is an <see cref="Npc"/>.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> to check.</param>
        /// <returns>A value indicating the <see cref="Player"/> is an <see cref="Npc"/>.</returns>
        public static bool IsNpc(this Player player) => player is Npc;

        /// <summary>
        /// Checks if a <see cref="ReferenceHub"/> is an <see cref="Npc"/>.
        /// </summary>
        /// <param name="referenceHub">The <see cref="Player"/> to check.</param>
        /// <returns>A value indicating the <see cref="Player"/> is an <see cref="Npc"/>.</returns>
        public static bool IsNpc(this ReferenceHub referenceHub) => referenceHub.gameObject.IsNpc();

        /// <summary>
        /// Checks a <see cref="GameObject"/> to see if it belongs to an <see cref="Npc"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to check.</param>
        /// <returns>A value indicating the <see cref="GameObject"/> is an <see cref="Npc"/>.</returns>
        public static bool IsNpc(this GameObject gameObject) => Npc.Dictionary.ContainsKey(gameObject);
    }
}