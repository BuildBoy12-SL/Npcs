// -----------------------------------------------------------------------
// <copyright file="UserCache.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Database
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Features;
    using NPCs.Database.Models;

    /// <summary>
    /// Handles the caching of players and their respective users for compatability with the database.
    /// </summary>
    public static class UserCache
    {
        private static Dictionary<Player, User> Cache { get; } = new Dictionary<Player, User>();

        /// <summary>
        /// Adds a player to the cache.
        /// </summary>
        /// <param name="player">The player to add.</param>
        public static void Add(Player player)
        {
            User user = new User(player.Nickname, player.UserId);
            Cache.Add(player, user);
        }

        /// <summary>
        /// Removes a player from the cache.
        /// </summary>
        /// <param name="player">The player to remove.</param>
        public static void Remove(Player player)
        {
            Cache.Remove(player);
        }

        /// <summary>
        /// Gets a user container based off of a player.
        /// </summary>
        /// <param name="player">The player to find the user for.</param>
        /// <returns>The user container or null if none are found.</returns>
        public static User Get(Player player)
        {
            return Cache.Values.FirstOrDefault(user => user.UserId == player.UserId);
        }
    }
}