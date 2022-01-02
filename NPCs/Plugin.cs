// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs
{
    using System;
    using Exiled.API.Features;
    using HarmonyLib;
    using NPCs.Database;
    using NPCs.EventHandlers;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private Harmony harmony;

        private PlayerEvents playerEvents;
        private ServerEvents serverEvents;

        /// <summary>
        /// Gets the only existing instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <summary>
        /// Gets an instance of the <see cref="Database.Data"/> class.
        /// </summary>
        public Data Data { get; private set; }

        /// <inheritdoc />
        public override string Author { get; } = "Build";

        /// <inheritdoc />
        public override string Name { get; } = "NPCs";

        /// <inheritdoc/>
        public override string Prefix { get; } = "NPCs";

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(4, 2, 2);

        /// <inheritdoc />
        public override Version Version { get; } = new Version(1, 0, 0);

        /// <inheritdoc />
        public override void OnEnabled()
        {
            Instance = this;

            harmony = new Harmony($"npcs.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();

            Data = new Data();

            playerEvents = new PlayerEvents(this);
            serverEvents = new ServerEvents(this);

            playerEvents.Subscribe();
            serverEvents.Subscribe();

            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            playerEvents.Unsubscribe();
            serverEvents.Unsubscribe();

            playerEvents = null;
            serverEvents = null;

            Data = null;

            harmony.UnpatchAll(harmony.Id);
            harmony = null;

            Instance = null;

            base.OnDisabled();
        }
    }
}