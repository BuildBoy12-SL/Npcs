// -----------------------------------------------------------------------
// <copyright file="Serializer.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Database
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using NPCs.Database.Models;

    /// <summary>
    /// Handles serialization.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Serializes the given content into a file in the given collection.
        /// </summary>
        /// <param name="content">The content to serialize.</param>
        /// <param name="collection">The type of config.</param>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        public static void Serialize<T>(T content, string collection)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            string serialized = JsonConvert.SerializeObject(content);
            string file = FileHandler.GetFile(collection);
            File.WriteAllText(file, serialized);
        }

        /// <summary>
        /// Serializes the given content into a file in the given collection.
        /// </summary>
        /// <param name="content">The content to serialize.</param>
        /// <param name="user">The owner of the config.</param>
        /// <param name="collection">The type of config.</param>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        public static void Serialize<T>(T content, User user, string collection)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            string serialized = JsonConvert.SerializeObject(content);
            string file = FileHandler.GetFile(user, collection);
            File.WriteAllText(file, serialized);
        }

        /// <summary>
        /// Deserializes the contents of a Json string into the specified type.
        /// </summary>
        /// <param name="content">The content to deserialize.</param>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <returns>The deserialized object.</returns>
        public static T Deserialize<T>(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content));

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}