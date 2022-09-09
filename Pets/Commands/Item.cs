// -----------------------------------------------------------------------
// <copyright file="Item.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using Pets.API;

    /// <summary>
    /// Sets the pet's held item.
    /// </summary>
    public class Item : ICommand
    {
        private static readonly ItemType[] BlacklistedItems =
        {
            ItemType.Ammo9x19,
            ItemType.Ammo12gauge,
            ItemType.Ammo44cal,
            ItemType.Ammo556x45,
            ItemType.Ammo762x39,
            ItemType.ArmorCombat,
            ItemType.ArmorHeavy,
            ItemType.ArmorLight,
        };

        /// <inheritdoc />
        public string Command => "item";

        /// <inheritdoc />
        public string[] Aliases { get; } = { "i" };

        /// <inheritdoc />
        public string Description => "Sets the pet's held item";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("pets.item"))
            {
                response = "Insufficient permission. Required permission: pets.item";
                return false;
            }

            Player player = Player.Get(sender);
            if (player is null)
            {
                response = "Console has no need for menial things such as pets.";
                return false;
            }

            Pet pet = player.GetPet();
            if (pet is null)
            {
                response = "Your pet is not spawned in.";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = $"Your pet is currently holding a {pet.CurrentItem.Type}";
                return true;
            }

            if (!Enum.TryParse(arguments.At(0), true, out ItemType itemType) || BlacklistedItems.Contains(itemType))
            {
                response = "Please specify a valid item.";
                return false;
            }

            pet.CurrentItem = itemType == ItemType.None ? null : Exiled.API.Features.Items.Item.Create(itemType);
            response = $"Set the pet's held item to a(n) {itemType}";
            return true;
        }
    }
}