// -----------------------------------------------------------------------
// <copyright file="LiftNode.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API.Navigation.Nodes.NodeTypes
{
    using Exiled.API.Features;

    /// <inheritdoc />
    public class LiftNode : NavigationNode<global::Lift>
    {
        private Lift exiledLift;

        /// <inheritdoc />
        public override global::Lift AttachedObject { get; protected set; }

        /// <summary>
        /// Gets the Exiled wrapper instance for the <see cref="Lift"/> object.
        /// </summary>
        public Lift ExiledLift => exiledLift ??= Lift.Get(AttachedObject);
    }
}