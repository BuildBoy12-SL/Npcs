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
            if (player is null || player == Server.Host)
            {
                response = "Console has no need for menial things such as pets.";
                return false;
            }

            PetPreferences preferences = PetPreferences.Get(player.UserId) ?? new PetPreferences(player.UserId);
            if (arguments.Count == 0)
            {
                response = $"Your pet is currently holding {FormatItemName(preferences.HeldItem)}";
                return true;
            }

            if (!Enum.TryParse(arguments.At(0), true, out ItemType itemType) || BlacklistedItems.Contains(itemType))
            {
                response = "Please specify a valid item.";
                return false;
            }

            if (Pet.Get(player) is Pet pet)
                pet.CurrentItem = itemType == ItemType.None ? null : Exiled.API.Features.Items.Item.Create(itemType);
            else
                preferences.HeldItem = itemType;

            response = $"Set the pet's held item to {FormatItemName(itemType)}";
            return true;
        }

        private static string FormatItemName(ItemType itemType)
        {
            if (itemType is ItemType.None)
                return "nothing";

            return "a(n) " + itemType;
        }
    }
}