// -----------------------------------------------------------------------
// <copyright file="SpawnPreset.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Commands.SpawnPreset
{
    using System;
    using System.Text;
    using CommandSystem;
    using NorthwoodLib.Pools;

    /// <summary>
    /// A command to spawn a preset npc.
    /// </summary>
    public class SpawnPreset : ParentCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnPreset"/> class.
        /// </summary>
        public SpawnPreset() => LoadGeneratedCommands();

        /// <inheritdoc/>
        public override string Command { get; } = "spawnpreset";

        /// <inheritdoc/>
        public override string[] Aliases { get; } = { "sp" };

        /// <inheritdoc/>
        public override string Description { get; } = "Spawns a preset npc.";

        /// <inheritdoc />
        public sealed override void LoadGeneratedCommands()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("Please enter a valid subcommand! Available:");
            foreach (ICommand command in AllCommands)
            {
                stringBuilder.AppendLine(command.Aliases != null && command.Aliases.Length > 0
                    ? $"{command.Command} | Aliases: {string.Join(", ", command.Aliases)}"
                    : command.Command);
            }

            response = StringBuilderPool.Shared.ToStringReturn(stringBuilder).TrimEnd();
            return false;
        }
    }
}