// -----------------------------------------------------------------------
// <copyright file="PdExitNode.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API.Navigation.Nodes
{
    /// <inheritdoc />
    public class PdExitNode : NavigationNode<PocketDimensionTeleport>
    {
        /// <inheritdoc />
        public override PocketDimensionTeleport AttachedObject { get; protected set; }
    }
}