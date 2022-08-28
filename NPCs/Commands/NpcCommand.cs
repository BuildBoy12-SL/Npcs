// -----------------------------------------------------------------------
// <copyright file="NpcCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Commands
{
    using System;
    using System.Text;
    using CommandSystem;
    using NorthwoodLib.Pools;

    /// <inheritdoc />
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class NpcCommand : ParentCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NpcCommand"/> class.
        /// </summary>
        public NpcCommand() => LoadGeneratedCommands();

        /// <inheritdoc />
        public override string Command => "npc";

        /// <inheritdoc />
        public override string[] Aliases { get; } = Array.Empty<string>();

        /// <inheritdoc />
        public override string Description => "The parent command for the NPC plugin.";

        /// <inheritdoc />
        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new Destroy());
            RegisterCommand(new List());
            RegisterCommand(new Spawn());
        }

        /// <inheritdoc />
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("Please enter a valid subcommand! Available:");
            foreach (ICommand command in AllCommands)
            {
                stringBuilder.AppendLine(command.Aliases is { Length: > 0 }
                    ? $"{command.Command} | Aliases: {string.Join(", ", command.Aliases)}"
                    : command.Command);
            }

            response = StringBuilderPool.Shared.ToStringReturn(stringBuilder).TrimEnd();
            return false;
        }
    }
}