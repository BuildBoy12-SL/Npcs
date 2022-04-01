// -----------------------------------------------------------------------
// <copyright file="PetPreferences.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets
{
    using System.Collections.Generic;
    using System.IO;
    using Exiled.API.Features;
    using Exiled.Loader;
    using Pets.API;

    /// <summary>
    /// Represents a user's preferences in their pets.
    /// </summary>
    public class PetPreferences
    {
        private static Dictionary<string, PetPreferences> preferences;

        /// <summary>
        /// Initializes a new instance of the <see cref="PetPreferences"/> class.
        /// </summary>
        public PetPreferences()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PetPreferences"/> class.
        /// </summary>
        /// <param name="userId">The owner of the preferences.</param>
        /// <param name="isShown"><inheritdoc cref="IsShown"/></param>
        /// <param name="name"><inheritdoc cref="Name"/></param>
        /// <param name="role"><inheritdoc cref="Role"/></param>
        /// <param name="heldItem"><inheritdoc cref="HeldItem"/></param>
        /// <param name="scale"><inheritdoc cref="Scale"/></param>
        public PetPreferences(string userId, bool isShown, string name, int role, int heldItem, SerializableVector3 scale)
        {
            IsShown = isShown;
            Name = name;
            Role = role;
            HeldItem = heldItem;
            Scale = scale;
            preferences?.Add(userId, this);
        }

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

        /// <summary>
        /// Gets or sets the pet's scale.
        /// </summary>
        public SerializableVector3 Scale { get; set; }

        /// <summary>
        /// Gets a players preferences.
        /// </summary>
        /// <param name="player">The player to lookup the preferences for.</param>
        /// <returns>The found preferences, or <see langword="null"/> if none are found.</returns>
        public static PetPreferences Get(Player player) => Get(player.UserId);

        /// <summary>
        /// Gets a players preferences based off of their user id.
        /// </summary>
        /// <param name="userId">The user id to lookup the preferences for.</param>
        /// <returns>The found preferences, or <see langword="null"/> if none are found.</returns>
        public static PetPreferences Get(string userId)
        {
            if (preferences == null)
                return null;

            preferences.TryGetValue(userId, out PetPreferences petPreferences);
            return petPreferences;
        }

        /// <summary>
        /// Writes all pet preferences to file.
        /// </summary>
        public static void Save()
        {
            if (!Directory.Exists(Plugin.Instance.Config.FolderPath))
                Directory.CreateDirectory(Plugin.Instance.Config.FolderPath);

            File.WriteAllText(Path.Combine(Plugin.Instance.Config.FolderPath, Plugin.Instance.Config.File), Loader.Serializer.Serialize(preferences ?? new Dictionary<string, PetPreferences>()));
        }

        /// <summary>
        /// Loads all pet preferences into memory.
        /// </summary>
        public static void Load()
        {
            string filePath = Path.Combine(Plugin.Instance.Config.FolderPath, Plugin.Instance.Config.File);
            preferences = !File.Exists(filePath)
                ? new Dictionary<string, PetPreferences>()
                : Loader.Deserializer.Deserialize<Dictionary<string, PetPreferences>>(File.ReadAllText(filePath));
        }
    }
}