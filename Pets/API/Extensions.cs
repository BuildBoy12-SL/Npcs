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
        /// <summary>
        /// Returns a value indicating whether the player has an active pet.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <returns>The pet or null if none is found.</returns>
        public static Pet GetPet(this Player player)
        {
            foreach (Pet pet in Pet.Instances)
            {
                if (pet.Owner == player)
                    return pet;
            }

            return null;
        }

        /// <summary>
        /// Returns a value indicating whether the player is a pet.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <param name="pet">The pet instance or null if the player is not a pet.</param>
        /// <returns>Whether the checked player is a pet.</returns>
        public static bool IsPet(this Player player, out Pet pet)
        {
            foreach (Pet p in Pet.Instances)
            {
                if (p.Npc == player)
                {
                    pet = p;
                    return true;
                }
            }

            pet = null;
            return false;
        }
    }
}