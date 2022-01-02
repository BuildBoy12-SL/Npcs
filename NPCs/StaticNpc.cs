// -----------------------------------------------------------------------
// <copyright file="StaticNpc.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs
{
    using Mirror;
    using UnityEngine;

    /// <summary>
    /// Represents a npc that is not meant to be respawned.
    /// </summary>
    public class StaticNpc : NpcBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticNpc"/> class.
        /// </summary>
        /// <param name="position">The position the npc should spawn at.</param>
        /// <param name="rotation">The direction the npc should face.</param>
        /// <param name="roleType">The role of the npc.</param>
        /// <param name="name">The name of the npc.</param>
        /// <param name="scale">The size of the npc.</param>
        /// <param name="itemType">The item the npc should hold.</param>
        public StaticNpc(Vector3 position, Quaternion rotation, RoleType roleType, string name, Vector3 scale, ItemType itemType)
            : base(roleType, name, scale)
        {
            Position = position;
            GameObject.transform.rotation = rotation;
            ReferenceHub.inventory.NetworkCurItem = new InventorySystem.Items.ItemIdentifier(itemType, 0);
            NetworkServer.Spawn(GameObject);
        }
    }
}