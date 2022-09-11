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
        public override Version RequiredExiledVersion { get; } = new(5, 3, 0);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Instance = this;
            harmony = new Harmony($"pets.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();

            playerEvents = new PlayerEvents(this);
            Exiled.Events.Handlers.Player.ChangingRole += playerEvents.OnChangingRole;
            Exiled.Events.Handlers.Player.Destroying += playerEvents.OnDestroying;
            Exiled.Events.Handlers.Player.EnteringPocketDimension += playerEvents.OnEnteringPocketDimension;
            Exiled.Events.Handlers.Player.Handcuffing += playerEvents.OnHandcuffing;
            Exiled.Events.Handlers.Player.Spawned += playerEvents.OnSpawned;
            Exiled.Events.Handlers.Player.TriggeringTesla += playerEvents.OnTriggeringTesla;

            serverEvents = new ServerEvents();
            Exiled.Events.Handlers.Server.ReloadedConfigs += ReloadCommands;
            Exiled.Events.Handlers.Server.RestartingRound += serverEvents.OnRestartingRound;
            Exiled.Events.Handlers.Server.RoundEnded += serverEvents.OnRoundEnded;
            Exiled.Events.Handlers.Server.WaitingForPlayers += serverEvents.OnWaitingForPlayers;

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.ReloadedConfigs -= ReloadCommands;
            Exiled.Events.Handlers.Server.RestartingRound -= serverEvents.OnRestartingRound;
            Exiled.Events.Handlers.Server.RoundEnded -= serverEvents.OnRoundEnded;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= serverEvents.OnWaitingForPlayers;
            serverEvents = null;

            Exiled.Events.Handlers.Player.ChangingRole -= playerEvents.OnChangingRole;
            Exiled.Events.Handlers.Player.Destroying -= playerEvents.OnDestroying;
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= playerEvents.OnEnteringPocketDimension;
            Exiled.Events.Handlers.Player.Handcuffing -= playerEvents.OnHandcuffing;
            Exiled.Events.Handlers.Player.Spawned -= playerEvents.OnSpawned;
            Exiled.Events.Handlers.Player.TriggeringTesla -= playerEvents.OnTriggeringTesla;
            playerEvents = null;

            harmony.UnpatchAll(harmony.Id);
            harmony = null;
            Instance = null;
            base.OnDisabled();
        }

        private void ReloadCommands()
        {
            OnUnregisteringCommands();
            OnRegisteringCommands();
        }
    }
}