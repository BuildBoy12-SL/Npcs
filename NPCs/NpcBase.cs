// -----------------------------------------------------------------------
// <copyright file="NpcBase.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Mirror;
    using UnityEngine;

    /// <summary>
    /// Represents the core of an npc.
    /// </summary>
    public abstract class NpcBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NpcBase"/> class.
        /// </summary>
        /// <param name="roleType">The role of the npc.</param>
        /// <param name="name">The name of the npc.</param>
        /// <param name="scale">The size of the npc.</param>
        protected NpcBase(RoleType roleType, string name, Vector3 scale)
        {
            GameObject = Object.Instantiate(NetworkManager.singleton.playerPrefab);
            ReferenceHub = GameObject.GetComponent<ReferenceHub>();

            GameObject.transform.localScale = scale;

            ReferenceHub.queryProcessor.PlayerId = 9999;
            ReferenceHub.queryProcessor.NetworkPlayerId = 9999;
            ReferenceHub.queryProcessor._ipAddress = "127.0.0.WAN";

            ReferenceHub.characterClassManager.CurClass = roleType;
            ReferenceHub.playerStats.StatModules[0].CurValue = 100;
            ReferenceHub.nicknameSync.Network_myNickSync = name;

            Player = new Player(GameObject);
            Player.SessionVariables.Add("IsNPC", true);

            Dictionary.Add(GameObject, this);
        }

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey,TValue}"/> containing all <see cref="Npc"/>'s on the server.
        /// </summary>
        public static Dictionary<GameObject, NpcBase> Dictionary { get; } = new Dictionary<GameObject, NpcBase>();

        /// <summary>
        /// Gets the created <see cref="Exiled.API.Features.Player"/> to represent the npc.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Gets the attached ReferenceHub component.
        /// </summary>
        public ReferenceHub ReferenceHub { get; }

        /// <summary>
        /// Gets or sets the npc's position.
        /// </summary>
        public Vector3 Position
        {
            get => Player.Position;
            set => Player.ReferenceHub.playerMovementSync.OverridePosition(value, 0f, true);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the npc can receive action.
        /// </summary>
        public bool IsActionLocked { get; set; }

        /// <summary>
        /// Gets the attached <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        protected GameObject GameObject { get; }

        /// <summary>
        /// Destroys the fake player.
        /// </summary>
        public virtual void Destroy()
        {
            Dictionary.Remove(GameObject);
            PlayerManager.RemovePlayer(GameObject, CustomNetworkManager.slots);
            NetworkServer.UnSpawn(GameObject);
            Object.Destroy(GameObject);
            Player = null;
        }
    }
}