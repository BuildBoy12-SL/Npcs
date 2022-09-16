// -----------------------------------------------------------------------
// <copyright file="RoomNode.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API.Navigation.Nodes
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Features;

    /// <inheritdoc />
    public class RoomNode : NavigationNode<Room>
    {
        /// <inheritdoc />
        public override Room AttachedObject { get; protected set; }

        /// <summary>
        /// Gets all door nodes that are connected to this room node.
        /// </summary>
        public IEnumerable<DoorNode> DoorNodes => AttachedObject == null ? null : AttachedObject.Doors.Select(door => door.GameObject.GetComponent<DoorNode>());
    }
}