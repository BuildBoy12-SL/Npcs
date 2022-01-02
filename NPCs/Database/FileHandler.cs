// -----------------------------------------------------------------------
// <copyright file="FileHandler.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Database
{
    using System;
    using System.Globalization;
    using System.IO;
    using Exiled.API.Features;
    using NPCs.Database.Models;

    /// <summary>
    /// Handles I/O operations.
    /// </summary>
    public static class FileHandler
    {
        private static readonly string NpcConfigsPath = Path.Combine(Paths.Configs, "NPCs");
        private static readonly string PetPreferencesPath = Path.Combine(NpcConfigsPath, "PetPreferences");

        static FileHandler()
        {
            if (!Directory.Exists(NpcConfigsPath))
                Directory.CreateDirectory(NpcConfigsPath);

            if (!Directory.Exists(PetPreferencesPath))
                Directory.CreateDirectory(PetPreferencesPath);
        }

        /// <summary>
        /// Gets a file based on a  config collection.
        /// </summary>
        /// <param name="collection">The collection the file lies in.</param>
        /// <returns>The path to the specified file.</returns>
        public static string GetFile(string collection)
        {
            if (string.IsNullOrWhiteSpace(collection))
                throw new ArgumentNullException(nameof(collection));

            string folderPath = GetCollectionFolder(collection);
            if (string.IsNullOrWhiteSpace(collection))
                throw new DirectoryNotFoundException($"Unable to find a configured collection with the name of {collection}");

            return Path.Combine(folderPath, DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Gets a file based on a user and the config collection.
        /// </summary>
        /// <param name="user">The user whos file to find.</param>
        /// <param name="collection">The collection the file lies in.</param>
        /// <returns>The path to the specified file.</returns>
        public static string GetFile(User user, string collection)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(collection))
                throw new ArgumentNullException(nameof(collection));

            string folderPath = GetCollectionFolder(collection);
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new DirectoryNotFoundException($"Unable to find a configured collection with the name of {collection}");

            return Path.Combine(folderPath, user.UserId);
        }

        private static string GetCollectionFolder(string collection)
        {
            if (string.Equals(collection, nameof(PetPreferences), StringComparison.OrdinalIgnoreCase))
                return PetPreferencesPath;

            return string.Empty;
        }
    }
}