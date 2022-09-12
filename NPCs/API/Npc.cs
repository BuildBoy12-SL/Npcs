// -----------------------------------------------------------------------
// <copyright file="Npc.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Mirror;
    using PlayerStatsSystem;
    using UnityEngine;

    /// <summary>
    /// Represents the core of an npc.
    /// </summary>
    public class Npc : Player
    {
        private bool hiddenOnce;
        private bool isSpawned;

        /// <summary>
        /// Initializes a new instance of the <see cref="Npc"/> class.
        /// </summary>
        public Npc()
            : base(CreateHub())
        {
            SetupNpc();
            GameObject.transform.localScale = Vector3.one;
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
            SetupNpc(roleType, name);
            GameObject.transform.localScale = scale;
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
                {
                    NetworkServer.Spawn(GameObject);
                    if (hiddenOnce)
                        RestartModules();

                    return;
                }

                NetworkServer.UnSpawn(GameObject);
                hiddenOnce = true;
            }
        }

        /// <summary>
        /// Gets or sets the position of the npc.
        /// </summary>
        public new Vector3 Position
        {
            get => base.Position;
            set => ReferenceHub.playerMovementSync.OverridePosition(value, null, true);
        }

        /// <summary>
        /// Gets or sets the name of the npc.
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
            if (!IsSpawned)
                return;

            IsSpawned = false;
            IsSpawned = true;
        }

        private void SetupNpc(RoleType roleType = RoleType.Tutorial, string name = "NPC")
        {
            ReferenceHub.characterClassManager.CurClass = roleType;

            ReferenceHub.characterClassManager._privUserId = "npc";
            ReferenceHub.playerMovementSync.NetworkGrounded = true;
            ReferenceHub.nicknameSync.Network_myNickSync = name;
            ReferenceHub.queryProcessor._ipAddress = "127.0.0.WAN";

            SessionVariables.Add("IsNPC", true);
            Dictionary.Add(GameObject, this);
            Player.Dictionary.Add(GameObject, this);
        }

        private void RestartModules()
        {
            foreach (StatBase statBase in ReferenceHub.playerStats.StatModules)
                statBase.Init(ReferenceHub);
        }
    }
}