// -----------------------------------------------------------------------
// <copyright file="Get.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Database.TransferTypes
{
    using System.IO;
    using NPCs.Database.Models;

    /// <summary>
    /// A class used to obtain info from the database.
    /// </summary>
    public class Get
    {
        /// <summary>
        /// Receives a user's preferences from the database.
        /// </summary>
        /// <param name="owner">The owner of the pet.</param>
        /// <returns>The user's preferences or null if none are found.</returns>
        public PetPreferences PetPreferences(User owner)
        {
            string file = FileHandler.GetFile(owner, nameof(PetPreferences));
            if (!File.Exists(file))
                return null;

            return Serializer.Deserialize<PetPreferences>(File.ReadAllText(file));
        }
    }
}