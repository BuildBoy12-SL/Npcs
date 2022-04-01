// -----------------------------------------------------------------------
// <copyright file="Pet.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using NPCs;
    using NPCs.Cores;
    using Pets.API;
    using UnityEngine;

    /// <summary>
    /// Represents an in-game pet.
    /// </summary>
    public class Pet
    {
        private readonly MovementHandler movementHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pet"/> class.
        /// </summary>
        /// <param name="owner">The owner of the pet.</param>
        public Pet(Player owner)
        {
            Owner = owner;
            Preferences = PetPreferences.Get(owner);
            if (Preferences == null)
            {
                string defaultName = Plugin.Instance.Config.DefaultName.Replace("{Name}", owner.DisplayNickname ?? owner.Nickname);
                Preferences = new PetPreferences(owner.UserId, true, defaultName, RoleType.ClassD.ToId(), -1, Plugin.Instance.Config.Size);
            }

            Npc = new Npc(Identifiers.RoleIdToType(Preferences.Role), Preferences.Name, Plugin.Instance.Config.Size);
            movementHandler = new MovementHandler(Npc, Owner);
            if (Preferences.IsShown)
                IsShown = true;
        }

        /// <summary>
        /// Gets all existing instances of pets.
        /// </summary>
        public static List<Pet> Instances { get; } = new List<Pet>();

        /// <summary>
        /// Gets the owner of the pet.
        /// </summary>
        public Player Owner { get; }

        /// <summary>
        /// Gets the npc controlling the pet.
        /// </summary>
        public Npc Npc { get; }

        /// <summary>
        /// Gets the owner's preferences for the pet.
        /// </summary>
        public PetPreferences Preferences { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the pet is currently visible.
        /// </summary>
        /// <remarks>This also affects the player's pet preferences.</remarks>
        public bool IsShown
        {
            get => Preferences.IsShown;
            set
            {
                Preferences.IsShown = value;
                if (value)
                {
                    Npc.Spawn();
                    movementHandler.Play();
                    return;
                }

                movementHandler.Pause();
                Npc.Despawn();
            }
        }

        /// <summary>
        /// Gets or sets the pet's held item.
        /// </summary>
        public ItemType HeldItem
        {
            get => Npc.HeldItem;
            set
            {
                Item item = Item.Create(value);
                if (item is Firearm firearm)
                    firearm.ClearAttachments();

                Npc.ReferenceHub.inventory.NetworkCurItem = new InventorySystem.Items.ItemIdentifier(value, item.Serial);
                Preferences.HeldItem = value.ToId();
            }
        }

        /// <summary>
        /// Gets or sets the display name of the pet.
        /// </summary>
        public string Name
        {
            get => Npc.Name;
            set
            {
                Npc.Name = value;
                Preferences.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the display role of the pet.
        /// </summary>
        public RoleType Role
        {
            get => Npc.Role;
            set
            {
                Npc.Role = value;
                Preferences.Role = value.ToId();
            }
        }

        /// <summary>
        /// Gets or sets the scale of the pet.
        /// </summary>
        public Vector3 Scale
        {
            get => Npc.Player.Scale;
            set
            {
                Npc.Player.GameObject.transform.localScale = value;
                Preferences.Scale = value;
            }
        }

        /// <summary>
        /// Gets the given owner's pet or creates one if it does not exist.
        /// </summary>
        /// <param name="owner">The owner of the pet.</param>
        /// <returns>The gotten or created pet.</returns>
        public static Pet GetOrCreate(Player owner)
        {
            Pet pet = owner.GetPet();
            if (pet != null)
                return pet;

            pet = new Pet(owner);
            Instances.Add(pet);
            return pet;
        }

        /// <summary>
        /// Destroys the pet and all of its logic.
        /// </summary>
        public void Destroy()
        {
            movementHandler.Kill();
            Npc.Destroy();
            Instances.Remove(this);
        }
    }
}