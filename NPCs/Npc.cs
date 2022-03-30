// -----------------------------------------------------------------------
// <copyright file="Npc.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs
{
    using Exiled.API.Features;
    using Mirror;
    using UnityEngine;

    /// <summary>
    /// Manages the actions of dummy players.
    /// </summary>
    public class Npc : NpcBase
    {
        private ItemType heldItem;
        private bool isSpawned;

        /// <summary>
        /// Initializes a new instance of the <see cref="Npc"/> class.
        /// </summary>
        /// <param name="scale">The size of the npc.</param>
        /// <param name="roleType">The role the npc should spawn as.</param>
        /// <param name="name">The name of the npc.</param>
        public Npc(RoleType roleType, string name, Vector3 scale)
            : base(roleType, name, scale)
        {
            Player.Dictionary.Add(GameObject, Player);
            PlayerManager.AddPlayer(GameObject, CustomNetworkManager.slots);
        }

        /// <summary>
        /// Gets or sets the NPC's held item.
        /// </summary>
        public ItemType HeldItem
        {
            get => heldItem;
            set
            {
                Player.ReferenceHub.inventory.NetworkCurItem = new InventorySystem.Items.ItemIdentifier(value, 0);
                heldItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the npc's name.
        /// </summary>
        public string Name
        {
            get => ReferenceHub.nicknameSync.Network_myNickSync;
            set
            {
                ReferenceHub.nicknameSync.Network_myNickSync = value;
                Respawn();
            }
        }

        /// <summary>
        /// Gets or sets the NPC's role.
        /// </summary>
        public RoleType Role
        {
            get => Player.Role;
            set
            {
                Player.ReferenceHub.characterClassManager.CurClass = value;
                Respawn();
            }
        }

        /// <summary>
        /// Spawns the NPC.
        /// </summary>
        public void Spawn()
        {
            if (isSpawned)
                return;

            NetworkServer.Spawn(GameObject);
            isSpawned = true;
        }

        /// <summary>
        /// Despawns the NPC.
        /// </summary>
        public void Despawn()
        {
            if (!isSpawned)
                return;

            NetworkServer.UnSpawn(GameObject);
            isSpawned = false;
        }

        private void Respawn()
        {
            Despawn();
            Spawn();
        }
    }
}