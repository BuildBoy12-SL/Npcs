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

        /// <summary>
        /// Initializes a new instance of the <see cref="Npc"/> class.
        /// </summary>
        public Npc()
            : base(CreateHub())
        {
            ReferenceHub.characterClassManager.CurClass = RoleType.Tutorial;
            ReferenceHub.playerStats.StatModules[0].CurValue = 100;
            ReferenceHub.nicknameSync.Network_myNickSync = "NPC";
            ReferenceHub.queryProcessor._ipAddress = "127.0.0.WAN";
            ReferenceHub.characterClassManager.IsVerified = true;
            ReferenceHub.playerMovementSync.NetworkGrounded = true;
            StartReferenceHub();

            GameObject.transform.localScale = Vector3.one;

            SessionVariables.Add("IsNPC", true);
            Dictionary.Add(GameObject, this);
            Player.Dictionary.Add(GameObject, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Npc"/> class.
        /// </summary>
        /// <param name="roleType">The <see cref="Player.Role"/> of the <see cref="Npc"/>.</param>
        /// <param name="name">The name of the <see cref="Npc"/>.</param>
        /// <param name="scale">The <see cref="Player.Scale"/> of the <see cref="Npc"/>.</param>
        public Npc(RoleType roleType, string name, Vector3 scale)
            : base(CreateHub())
        {
            ReferenceHub.characterClassManager.CurClass = roleType;
            ReferenceHub.playerStats.StatModules[0].CurValue = 100;
            ReferenceHub.nicknameSync.Network_myNickSync = name;
            ReferenceHub.queryProcessor._ipAddress = "127.0.0.WAN";
            ReferenceHub.characterClassManager.IsVerified = true;
            ReferenceHub.playerMovementSync.NetworkGrounded = true;
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
        /// Gets or sets a value indicating whether the npc is spawned.
        /// </summary>
        public bool IsSpawned
        {
            get => isSpawned;
            set
            {
                if (isSpawned == value)
                    return;

                isSpawned = value;
                if (value)
                    NetworkServer.Spawn(GameObject);
                else
                    NetworkServer.UnSpawn(GameObject);
            }
        }

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
        /// Destroys the npc.
        /// </summary>
        public virtual void Destroy()
        {
            Dictionary.Remove(GameObject);
            NetworkServer.Destroy(GameObject);
        }

        private static ReferenceHub CreateHub()
        {
            GameObject gameObject = Object.Instantiate(NetworkManager.singleton.playerPrefab);
            return gameObject.GetComponent<ReferenceHub>();
        }

        private void Respawn()
        {
            IsSpawned = false;
            IsSpawned = true;
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