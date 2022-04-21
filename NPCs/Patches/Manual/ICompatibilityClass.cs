// -----------------------------------------------------------------------
// <copyright file="ICompatibilityClass.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Patches.Manual
{
    using HarmonyLib;

    /// <summary>
    /// Defines the contract for a class that handles compatibility through patches with another plugin.
    /// </summary>
    public interface ICompatibilityClass
    {
        /// <summary>
        /// Patches all the required methods.
        /// </summary>
        /// <param name="harmony">An instance of the <see cref="Harmony"/> class.</param>
        void Patch(Harmony harmony);
    }
}