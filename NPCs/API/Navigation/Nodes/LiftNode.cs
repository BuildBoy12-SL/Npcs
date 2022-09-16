// -----------------------------------------------------------------------
// <copyright file="LiftNode.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API.Navigation.Nodes
{
    /// <inheritdoc />
    public class LiftNode : NavigationNode<Lift>
    {
        /// <inheritdoc />
        public override Lift AttachedObject { get; protected set; }

        /// <summary>
        /// Gets the Exiled wrapper instance for the <see cref="Lift"/> object.
        /// </summary>
        public Exiled.API.Features.Lift ExiledLift => Exiled.API.Features.Lift.Get(AttachedObject);
    }
}