// -----------------------------------------------------------------------
// <copyright file="PetPreferences.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Database.Models
{
    /// <summary>
    /// Represents a user's preferences in their pets.
    /// </summary>
    public class PetPreferences
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PetPreferences"/> class.
        /// </summary>
        public PetPreferences()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PetPreferences"/> class.
        /// </summary>
        /// <param name="user"><inheritdoc cref="User"/></param>
        /// <param name="isShown"><inheritdoc cref="IsShown"/></param>
        /// <param name="name"><inheritdoc cref="Name"/></param>
        /// <param name="role"><inheritdoc cref="Role"/></param>
        /// <param name="heldItem"><inheritdoc cref="HeldItem"/></param>
        public PetPreferences(User user, bool isShown, string name, int role, int heldItem)
        {
            User = user;
            IsShown = isShown;
            Name = name;
            Role = role;
            HeldItem = heldItem;
        }

        /// <summary>
        /// Gets or sets the owner of the pet.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the pet is visible.
        /// </summary>
        public bool IsShown { get; set; }

        /// <summary>
        /// Gets or sets the name given to the pet.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the pet's role.
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// Gets or sets the pet's held item.
        /// </summary>
        public int HeldItem { get; set; }
    }
}