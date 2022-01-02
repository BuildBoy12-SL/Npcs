// -----------------------------------------------------------------------
// <copyright file="AggressiveScp096.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Presets
{
    using NPCs.Cores.NavigationCore;
    using UnityEngine;

    /// <summary>
    /// A preset npc for an Scp096.
    /// </summary>
    public class AggressiveScp096 : NpcBase
    {
        private readonly NavigationCore navigationCore;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggressiveScp096"/> class.
        /// </summary>
        /// <param name="scale">The size of the npc.</param>
        /// <param name="roleType">The role the npc should spawn as.</param>
        /// <param name="name">The name of the npc.</param>
        public AggressiveScp096(RoleType roleType, string name, Vector3 scale)
            : base(roleType, name, scale)
        {
            navigationCore = new NavigationCore(this);
        }
    }
}