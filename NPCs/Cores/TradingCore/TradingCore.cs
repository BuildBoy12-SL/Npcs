// -----------------------------------------------------------------------
// <copyright file="TradingCore.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Cores.TradingCore
{
    /// <summary>
    /// Handles trading for an npc.
    /// </summary>
    public class TradingCore
    {
        private readonly NpcBase npcBase;

        /// <summary>
        /// Initializes a new instance of the <see cref="TradingCore"/> class.
        /// </summary>
        /// <param name="npcBase">The npc to control.</param>
        public TradingCore(NpcBase npcBase)
        {
            this.npcBase = npcBase;
        }
    }
}