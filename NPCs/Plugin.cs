// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs
{
    using System;
    using System.Reflection;
    using Exiled.API.Features;
    using Exiled.Loader;
    using HarmonyLib;
    using NPCs.EventHandlers;
    using NPCs.Patches.Compatibility;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private Harmony harmony;
        private PlayerEvents playerEvents;

        /// <inheritdoc />
        public override string Author => "Build";

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new(5, 0, 0);

        /// <inheritdoc />
        public override Version Version { get; } = new(1, 0, 0);

        /// <inheritdoc />
        public override void OnEnabled()
        {
            harmony = new Harmony($"npcs.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();
            PatchCompatability();

            playerEvents = new PlayerEvents();
            playerEvents.Subscribe();

            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            playerEvents?.Unsubscribe();
            playerEvents = null;

            harmony?.UnpatchAll(harmony.Id);
            harmony = null;

            base.OnDisabled();
        }

        private void PatchCompatability()
        {
            DiscordIntegration.Patch(harmony);
            EndConditions.Patch(harmony);
        }
    }
}