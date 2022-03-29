// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets
{
    using System;
    using Exiled.API.Features;
    using HarmonyLib;
    using Pets.EventHandlers;

    /// <inheritdoc />
    public class Plugin : Plugin<Config>
    {
        private Harmony harmony;
        private PlayerEvents playerEvents;
        private ServerEvents serverEvents;

        /// <summary>
        /// Gets the only existing instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <inheritdoc/>
        public override string Author => "Build";

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new Version(5, 0, 0);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Instance = this;
            harmony = new Harmony($"pets.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();

            playerEvents = new PlayerEvents();
            Exiled.Events.Handlers.Player.ChangingRole += playerEvents.OnChangingRole;
            serverEvents = new ServerEvents();
            Exiled.Events.Handlers.Server.RoundEnded += serverEvents.OnRoundEnded;
            Exiled.Events.Handlers.Server.WaitingForPlayers += serverEvents.OnWaitingForPlayers;

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= serverEvents.OnRoundEnded;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= serverEvents.OnWaitingForPlayers;
            serverEvents = null;
            harmony?.UnpatchAll(harmony.Id);
            harmony = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}