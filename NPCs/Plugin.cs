﻿// -----------------------------------------------------------------------
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
    using NPCs.API.Navigation.Nodes;
    using NPCs.Patches.Manual;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private Harmony harmony;

        /// <summary>
        /// Gets an instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <inheritdoc />
        public override string Author => "Build";

        /// <inheritdoc />
        public override string Name => "NPCs";

        /// <inheritdoc />
        public override string Prefix => "npcs";

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new(5, 3, 0);

        /// <inheritdoc />
        public override Version Version { get; } = new(1, 0, 0);

        /// <inheritdoc />
        public override void OnEnabled()
        {
            Instance = this;

            harmony = new Harmony($"npcs.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();
            RunManualPatches();

            Exiled.Events.Handlers.Map.Generated += NavigationNodeBase.GenerateMap;

            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Map.Generated -= NavigationNodeBase.GenerateMap;

            harmony?.UnpatchAll(harmony.Id);
            harmony = null;

            Instance = null;
            base.OnDisabled();
        }

        private void RunManualPatches()
        {
            foreach (Type type in Assembly.GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(IManualPatch)))
                    ((IManualPatch)Activator.CreateInstance(type)).Patch(harmony);
            }
        }
    }
}