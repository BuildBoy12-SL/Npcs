// -----------------------------------------------------------------------
// <copyright file="List.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Commands
{
    using System;
    using CommandSystem;
    using Exiled.Permissions.Extensions;

    /// <inheritdoc />
    public class List : ICommand
    {
        /// <inheritdoc />
        public string Command => "list";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "l" };

        /// <inheritdoc />
        public string Description => "Lists all NPCs.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("npc.list"))
            {
                response = "You do not have permission to run this command.";
                return false;
            }

            response = Environment.NewLine + string.Join(Environment.NewLine, Npc.List);
            return true;
        }
    }
}