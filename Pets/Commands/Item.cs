// -----------------------------------------------------------------------
// <copyright file="Item.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets.Commands
{
    using System;
    using System.ComponentModel;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using Pets.API;

    /// <summary>
    /// Sets the pet's held item.
    /// </summary>
    public class Item : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "item";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "i" };

        /// <inheritdoc />
        public string Description { get; set; } = "Sets the pet's held item";

        /// <summary>
        /// Gets or sets the permission required to use this command.
        /// </summary>
        [Description("The permission required to use this command.")]
        public string RequiredPermission { get; set; } = "pets.item";

        /// <summary>
        /// Gets or sets the response to provide to the user that lacks the required permission.
        /// </summary>
        [Description("The response to provide to the user that lacks the required permission.")]
        public string RequiredPermissionResponse { get; set; } = "Insufficient permission. Required permission: pets.item";

        /// <summary>
        /// Gets or sets the items that cannot be equipped to a pet.
        /// </summary>
        [Description("The items that cannot be equipped to a pet.")]
        public ItemType[] BlacklistedItems { get; set; } =
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

        /// <summary>
        /// Gets or sets the response to provide to the user when no arguments are provided.
        /// </summary>
        [Description("The response to provide to the user when no arguments are provided.")]
        public string NoArgumentsResponse { get; set; } = "Your pet is currently holding {0}";

        /// <summary>
        /// Gets or sets the literal translation of the word 'nothing'.
        /// </summary>
        [Description("The literal translation of the word 'nothing'.")]
        public string Nothing { get; set; } = "nothing";

        /// <summary>
        /// Gets or sets the way item translations should be formatted.
        /// </summary>
        public string FormattedItem { get; set; } = "a(n) {0}";

        /// <summary>
        /// Gets or sets the response to provide to the user when an invalid item is specified.
        /// </summary>
        [Description("The response to provide to the user when an invalid item is specified.")]
        public string InvalidItemResponse { get; set; } = "Please specify a valid item.";

        /// <summary>
        /// Gets or sets the response to provide when the pet's item is successfully changed.
        /// </summary>
        [Description("The response to provide when the pet's item is successfully changed.")]
        public string SuccessResponse { get; set; } = "Set the pet's held item to {0}";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(RequiredPermission))
            {
                response = RequiredPermissionResponse;
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
                response = string.Format(NoArgumentsResponse, FormatItemName(preferences.HeldItem));
                return true;
            }

            if (!Enum.TryParse(arguments.At(0), true, out ItemType itemType) || BlacklistedItems.Contains(itemType))
            {
                response = InvalidItemResponse;
                return false;
            }

            if (Pet.Get(player) is Pet pet)
                pet.CurrentItem = itemType == ItemType.None ? null : Exiled.API.Features.Items.Item.Create(itemType);
            else
                preferences.HeldItem = itemType;

            response = string.Format(SuccessResponse, FormatItemName(itemType));
            return true;
        }

        private string FormatItemName(ItemType itemType) => itemType is ItemType.None ? Nothing : string.Format(FormattedItem, itemType);
    }
}