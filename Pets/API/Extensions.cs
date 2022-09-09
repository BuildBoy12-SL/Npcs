// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets.API
{
    using Exiled.API.Features;

    /// <summary>
    /// Various extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <inheritdoc cref="Pet.Get(Player)"/>
        public static Pet GetPet(this Player owner) => Pet.Get(owner);
    }
}