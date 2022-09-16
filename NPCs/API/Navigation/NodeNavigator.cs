// -----------------------------------------------------------------------
// <copyright file="NodeNavigator.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API.Navigation
{
    using NPCs.API;
    using NPCs.API.Navigation.Nodes;

    /// <summary>
    /// Allows npcs to navigate through the map by following a path of nodes.
    /// </summary>
    public class NodeNavigator : MovementBase
    {
        private NavigationNodeBase currentTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeNavigator"/> class.
        /// </summary>
        /// <param name="npc">The <see cref="Npc"/> to control.</param>
        public NodeNavigator(Npc npc)
            : base(npc)
        {
        }

        /// <summary>
        /// Gets or sets the current navigation target.
        /// </summary>
        public new NavigationNodeBase CurrentTarget
        {
            get => currentTarget;
            set
            {
                currentTarget = value;
                base.CurrentTarget = value.gameObject;
            }
        }
    }
}