// -----------------------------------------------------------------------
// <copyright file="Npc.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Mirror;
    using UnityEngine;

    /// <summary>
    /// Represents the core of an npc.
    /// </summary>
    public class Npc : Player
    {
        private bool isSpawned;

        private Npc(ReferenceHub referenceHub)
            : base(referenceHub)
        {
            ReferenceHub.characterClassManager.CurClass = RoleType.Tutorial;
            ReferenceHub.playerStats.StatModules[0].CurValue = 100;
            ReferenceHub.nicknameSync.Network_myNickSync = "NPC";
            ReferenceHub.queryProcessor._ipAddress = "127.0.0.WAN";
            ReferenceHub.characterClassManager.IsVerified = true;
            StartReferenceHub();

            GameObject.transform.localScale = Vector3.one;

            SessionVariables.Add("IsNPC", true);
            Dictionary.Add(GameObject, this);
            Player.Dictionary.Add(GameObject, this);
        }

        private Npc(ReferenceHub referenceHub, RoleType roleType, string name, Vector3 scale)
            : base(referenceHub)
        {
            ReferenceHub.characterClassManager.CurClass = roleType;
            ReferenceHub.playerStats.StatModules[0].CurValue = 100;
            ReferenceHub.nicknameSync.Network_myNickSync = name;
            ReferenceHub.queryProcessor._ipAddress = "127.0.0.WAN";
            ReferenceHub.characterClassManager.IsVerified = true;
            StartReferenceHub();

            GameObject.transform.localScale = scale;

            SessionVariables.Add("IsNPC", true);
            Dictionary.Add(GameObject, this);
            Player.Dictionary.Add(GameObject, this);
        }

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey,TValue}"/> containing all <see cref="Npc"/>'s on the server.
        /// </summary>
        public static new Dictionary<GameObject, Npc> Dictionary { get; } = new();

        /// <summary>
        /// Gets a list of all <see cref="Npc"/>s on the server.
        /// </summary>
        public static new IEnumerable<Npc> List => Dictionary.Values;

        /// <summary>
        /// Gets or sets the NPC's position.
        /// </summary>
        public new Vector3 Position
        {
            get => base.Position;
            set => ReferenceHub.playerMovementSync.OverridePosition(value, null, true);
        }

        /// <summary>
        /// Gets or sets the NPC's name.
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
        public new RoleType Role
        {
            get => base.Role.Type;
            set
            {
                SetRole(value, SpawnReason.ForceClass, true);
                Respawn();
            }
        }

        /// <summary>
        /// Creates a <see cref="Npc"/>.
        /// </summary>
        /// <returns>The created <see cref="Npc"/> instance.</returns>
        public static Npc Create()
        {
            GameObject gameObject = Object.Instantiate(NetworkManager.singleton.playerPrefab);
            ReferenceHub referenceHub = gameObject.GetComponent<ReferenceHub>();
            return new Npc(referenceHub);
        }

        /// <summary>
        /// Creates a <see cref="Npc"/> with the specified name, role, and scale.
        /// </summary>
        /// <param name="roleType">The role of the npc.</param>
        /// <param name="name">The name of the npc.</param>
        /// <param name="scale">The scale of the npc.</param>
        /// <returns>The created <see cref="Npc"/> instance.</returns>
        public static Npc Create(RoleType roleType, string name, Vector3 scale)
        {
            GameObject gameObject = Object.Instantiate(NetworkManager.singleton.playerPrefab);
            ReferenceHub referenceHub = gameObject.GetComponent<ReferenceHub>();
            return new Npc(referenceHub, roleType, name, scale);
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

        /// <summary>
        /// Destroys the fake player.
        /// </summary>
        public void Destroy()
        {
            Dictionary.Remove(GameObject);
            PlayerManager.RemovePlayer(GameObject, CustomNetworkManager.slots);
            NetworkServer.Destroy(GameObject);
        }

        private void Respawn()
        {
            Despawn();
            Spawn();
        }

        private void StartReferenceHub()
        {
            ReferenceHub.characterClassManager.Start();
            ReferenceHub.playerStats.Start();
            ReferenceHub.nicknameSync.Start();
            ReferenceHub.playerMovementSync.Start();
            ReferenceHub.inventory.Start();
            ReferenceHub.serverRoles.Start();
        }
    }
}