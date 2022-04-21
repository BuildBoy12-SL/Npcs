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
    using NPCs.Patches.Compatibility;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private Harmony harmony;

        /// <inheritdoc />
        public override string Author => "Build";

        /// <inheritdoc />
        public override string Name => "NPCs";

        /// <inheritdoc />
        public override string Prefix => "npcs";

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

            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            harmony?.UnpatchAll(harmony.Id);
            harmony = null;

            base.OnDisabled();
        }

        private void PatchCompatability()
        {
            foreach (Type type in Assembly.GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(ICompatibilityClass)))
                    ((ICompatibilityClass)Activator.CreateInstance(type)).Patch(harmony);
            }
        }
    }
}