// -----------------------------------------------------------------------
// <copyright file="DoorNode.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API.Navigation.Nodes.NodeTypes
{
    using Exiled.API.Features.Items;
    using Interactables.Interobjects.DoorUtils;

    /// <inheritdoc />
    public class DoorNode : NavigationNode<DoorVariant>
    {
        /// <inheritdoc />
        public override DoorVariant AttachedObject { get; protected set; }

        /// <summary>
        /// Attempts to open the door.
        /// </summary>
        /// <param name="npc">The <see cref="Npc"/> attempting to open the door.</param>
        /// <returns>Whether the door was opened successfully.</returns>
        public bool TryOpen(Npc npc)
        {
            if (!AttachedObject.RequiredPermissions.CheckPermissions(npc.CurrentItem.Base, npc.ReferenceHub))
            {
                foreach (Item item in npc.Items)
                {
                    if (AttachedObject.RequiredPermissions.CheckPermissions(item.Base, npc.ReferenceHub))
                    {
                        npc.CurrentItem = item;
                        break;
                    }
                }
            }

            AttachedObject.ServerInteract(npc.ReferenceHub, 0);
            return AttachedObject.IsConsideredOpen();
        }
    }
}