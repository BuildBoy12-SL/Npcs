// -----------------------------------------------------------------------
// <copyright file="Data.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Database
{
    using NPCs.Database.TransferTypes;

    /// <summary>
    /// Represents the core class to interact with the database.
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Data"/> class.
        /// </summary>
        public Data()
        {
            Get = new Get();
            Send = new Send();
        }

        /// <summary>
        /// Gets an instance of the class used to obtain info from the websocket server.
        /// </summary>
        public Get Get { get; }

        /// <summary>
        /// Gets an instance of the class used to send info to the websocket server.
        /// </summary>
        public Send Send { get; }
    }
}