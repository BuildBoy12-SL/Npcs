// -----------------------------------------------------------------------
// <copyright file="Identifiers.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Handles cross-version compatability of varying entities.
    /// </summary>
    public static class Identifiers
    {
        private static readonly Dictionary<string, int> ItemIDs = new()
        {
            { "None", -1 },
            { "KeycardJanitor", 0 },
            { "KeycardScientist", 1 },
            { "KeycardResearchCoordinator", 2 },
            { "KeycardZoneManager", 3 },
            { "KeycardGuard", 4 },
            { "KeycardNTFOfficer", 5 },
            { "KeycardContainmentEngineer", 6 },
            { "KeycardNTFLieutenant", 7 },
            { "KeycardNTFCommander", 8 },
            { "KeycardFacilityManager", 9 },
            { "KeycardChaosInsurgency", 10 },
            { "KeycardO5", 11 },
            { "Radio", 12 },
            { "GunCOM15", 13 },
            { "Medkit", 14 },
            { "Flashlight", 15 },
            { "MicroHID", 16 },
            { "SCP500", 17 },
            { "SCP207", 18 },
            { "Ammo12gauge", 19 },
            { "GunE11SR", 20 },
            { "GunCrossvec", 21 },
            { "Ammo556x45", 22 },
            { "GunFSP9", 23 },
            { "GunLogicer", 24 },
            { "GrenadeHE", 25 },
            { "GrenadeFlash", 26 },
            { "Ammo44cal", 27 },
            { "Ammo762x39", 28 },
            { "Ammo9x19", 29 },
            { "GunCOM18", 30 },
            { "SCP018", 31 },
            { "SCP268", 32 },
            { "Adrenaline", 33 },
            { "Painkillers", 34 },
            { "Coin", 35 },
            { "ArmorLight", 36 },
            { "ArmorCombat", 37 },
            { "ArmorHeavy", 38 },
            { "GunRevolver", 39 },
            { "GunAK", 40 },
            { "GunShotgun", 41 },
            { "SCP330", 42 },
            { "MutantHands", 43 },
            { "SCP2176", 44 },
            { "SCP244a", 45 },
            { "SCP244b", 46 },
            { "Coal", 47 },
            { "ParticleDisruptor", 48 },
            { "SCP1853", 49 },
        };

        private static readonly Dictionary<int, string> ItemIDsReverse = ItemIDs.ToDictionary(pair => pair.Value, pair => pair.Key);

        private static readonly Dictionary<string, int> RoleIDs = new()
        {
            { "None", -1 },
            { "Scp173", 0 },
            { "ClassD", 1 },
            { "Spectator", 2 },
            { "Scp106", 3 },
            { "NtfSpecialist", 4 },
            { "Scp049", 5 },
            { "Scientist", 6 },
            { "Scp079", 7 },
            { "ChaosConscript", 8 },
            { "Scp096", 9 },
            { "Scp0492", 10 },
            { "NtfSergeant", 11 },
            { "NtfCaptain", 12 },
            { "NtfPrivate", 13 },
            { "Tutorial", 14 },
            { "FacilityGuard", 15 },
            { "Scp93953", 16 },
            { "Scp93989", 17 },
            { "ChaosRifleman", 18 },
            { "ChaosRepressor", 19 },
            { "ChaosMarauder", 20 },
        };

        private static readonly Dictionary<int, string> RoleIDsReverse = RoleIDs.ToDictionary(pair => pair.Value, pair => pair.Key);

        /// <summary>
        /// Gets an item id from an <see cref="ItemType"/>.
        /// </summary>
        /// <param name="item">The item to get the id of.</param>
        /// <returns>The found id or -1 if none are found to match.</returns>
        public static int ToId(this ItemType item)
        {
            if (ItemIDs.TryGetValue(item.ToString(), out int id))
                return id;
            return -1;
        }

        /// <summary>
        /// Gets an <see cref="ItemType"/> from an id.
        /// </summary>
        /// <param name="id">The id to get the type of.</param>
        /// <returns>The found <see cref="ItemType"/> or <see cref="ItemType.None"/> if none are found to match.</returns>
        public static ItemType ItemIdToType(int id)
        {
            if (ItemIDsReverse.TryGetValue(id, out string typeStr) && Enum.TryParse(typeStr, out ItemType type))
                return type;
            return ItemType.None;
        }

        /// <summary>
        /// Gets an item id from an <see cref="RoleType"/>.
        /// </summary>
        /// <param name="role">The role to get the id of.</param>
        /// <returns>The found id or -1 if none are found to match.</returns>
        public static int ToId(this RoleType role)
        {
            if (RoleIDs.TryGetValue(role.ToString(), out int id))
                return id;
            return -1;
        }

        /// <summary>
        /// Gets an <see cref="RoleType"/> from an id.
        /// </summary>
        /// <param name="id">The id to get the type of.</param>
        /// <returns>The found <see cref="RoleType"/> or <see cref="RoleType.None"/> if none are found to match.</returns>
        public static RoleType RoleIdToType(int id)
        {
            if (RoleIDsReverse.TryGetValue(id, out string typeStr) && Enum.TryParse(typeStr, out RoleType type))
                return type;
            return RoleType.None;
        }
    }
}