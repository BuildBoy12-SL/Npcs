// -----------------------------------------------------------------------
// <copyright file="User.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Database.Models
{
    /// <summary>
    /// A class used to represent an in-game player.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="username"><inheritdoc cref="Username"/></param>
        /// <param name="userId"><inheritdoc cref="UserId"/></param>
        public User(string username, string userId)
        {
            Username = username;
            UserId = userId;
        }

        /// <summary>
        /// Gets or sets the last known username of the player.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets the unique userid of the player.
        /// </summary>
        public string UserId { get; }
    }
}