// -----------------------------------------------------------------------
// <copyright file="PetCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets.Commands
{
    using System;
    using System.Reflection;
    using System.Text;
    using CommandSystem;
    using NorthwoodLib.Pools;

    /// <inheritdoc />
    public class PetCommand : ParentCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PetCommand"/> class.
        /// </summary>
        public PetCommand() => LoadGeneratedCommands();

        /// <inheritdoc />
        public override string Command => "pet";

        /// <inheritdoc />
        public override string[] Aliases { get; } = Array.Empty<string>();

        /// <inheritdoc />
        public override string Description => "Handles pet related commands.";

        /// <inheritdoc/>
        public sealed override void LoadGeneratedCommands()
        {
            foreach (PropertyInfo property in Plugin.Instance.Config.GetType().GetProperties())
            {
                if (property.GetValue(Plugin.Instance.Config) is ICommand command)
                    RegisterCommand(command);
            }
        }

        /// <inheritdoc/>
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