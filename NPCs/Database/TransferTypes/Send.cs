// -----------------------------------------------------------------------
// <copyright file="Send.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Database.TransferTypes
{
    using NPCs.Database.Models;

    /// <summary>
    /// A class used to forward info to the database.
    /// </summary>
    public class Send
    {
        /// <summary>
        /// Sends a user's pet preferences to the database.
        /// </summary>
        /// <param name="petPreferences">The preferences to send.</param>
        public void PetPreferences(PetPreferences petPreferences)
        {
            Serializer.Serialize(petPreferences, petPreferences.User, nameof(PetPreferences));
        }
    }
}