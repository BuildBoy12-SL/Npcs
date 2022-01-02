// -----------------------------------------------------------------------
// <copyright file="CombatCore.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Cores.CombatCore
{
    using NPCs.Cores.NavigationCore;

    /// <summary>
    /// Handles combat logic.
    /// </summary>
    public class CombatCore
    {
        private readonly NpcBase npcBase;
        private readonly NavigationCore navigationCore;

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatCore"/> class.
        /// </summary>
        /// <param name="npcBase">The npc to control.</param>
        /// <param name="navigationCore">The navigation core to use or none if navigation is not required.</param>
        public CombatCore(NpcBase npcBase, NavigationCore navigationCore = null)
        {
            this.npcBase = npcBase;
            this.navigationCore = navigationCore;
        }
    }
}