// -----------------------------------------------------------------------
// <copyright file="RoomData.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Pathing
{
    using System;
    using System.Collections.Generic;
    using NPCs.API;

    /// <summary>
    /// A container for required data about a room.
    /// </summary>
    [Serializable]
    public class RoomData
    {
        /// <summary>
        /// Gets or sets the saved relative position in the room.
        /// </summary>
        public SerializableVector3 Relative { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the room.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets a collection of items in the room.
        /// </summary>
        public List<int> Items { get; set; }
    }
}