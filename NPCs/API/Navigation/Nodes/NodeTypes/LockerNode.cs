// -----------------------------------------------------------------------
// <copyright file="LockerNode.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API.Navigation.Nodes.NodeTypes
{
    using MapGeneration.Distributors;

    /// <inheritdoc />
    public class LockerNode : NavigationNode<Locker>
    {
        /// <inheritdoc />
        public override Locker AttachedObject { get; protected set; }
    }
}