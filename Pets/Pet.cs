// -----------------------------------------------------------------------
// <copyright file="Pet.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using NPCs;
    using NPCs.Cores.Navigation;
    using UnityEngine;

    /// <summary>
    /// Represents an in-game pet.
    /// </summary>
    public class Pet : Npc
    {
        private readonly MovementCore movementCore;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pet"/> class.
        /// </summary>
        /// <param name="owner">The owner of the pet.</param>
        /// <param name="petPreferences">The preferences of the owner.</param>
        public Pet(Player owner, PetPreferences petPreferences)
            : base(petPreferences.Role, petPreferences.Name, petPreferences.Scale)
        {
            Owner = owner;
            PetPreferences = petPreferences;
            IsGodModeEnabled = true;
            if (PetPreferences.HeldItem != ItemType.None)
                CurrentItem = Item.Create(PetPreferences.HeldItem);

            movementCore = new MovementCore(this)
            {
                FollowTarget = owner.GameObject,
            };
        }

        /// <summary>
        /// Gets all existing instances of pets.
        /// </summary>
        public static List<Pet> Instances { get; } = new();

        /// <summary>
        /// Gets the owner of the pet.
        /// </summary>
        public Player Owner { get; }

        /// <summary>
        /// Gets the owner's preferences for the pet.
        /// </summary>
        public PetPreferences PetPreferences { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the pet is currently visible.
        /// </summary>
        /// <remarks>This also affects the player's pet preferences.</remarks>
        public new bool IsSpawned
        {
            get => base.IsSpawned;
            set
            {
                PetPreferences.IsShown = value;
                base.IsSpawned = value;
            }
        }

        /// <summary>
        /// Gets or sets the pet's held item.
        /// </summary>
        public new Item CurrentItem
        {
            get => base.CurrentItem;
            set
            {
                base.CurrentItem = value;
                PetPreferences.HeldItem = value.Type;
            }
        }

        /// <summary>
        /// Gets or sets the display name of the pet.
        /// </summary>
        public new string Name
        {
            get => base.Name;
            set
            {
                base.Name = value;
                PetPreferences.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the display role of the pet.
        /// </summary>
        public new RoleType Role
        {
            get => base.Role;
            set
            {
                base.Role = value;
                PetPreferences.Role = value;
            }
        }

        /// <summary>
        /// Gets or sets the scale of the pet.
        /// </summary>
        public new Vector3 Scale
        {
            get => base.Scale;
            set
            {
                base.Scale = value;
                PetPreferences.Scale = value;
            }
        }

        /// <summary>
        /// Gets the given owner's pet or creates one if it does not exist.
        /// </summary>
        /// <param name="owner">The owner of the pet.</param>
        /// <returns>The gotten or created pet.</returns>
        public static Pet GetOrCreate(Player owner) => Get(owner) ?? Create(owner);

        /// <summary>
        /// Returns a value indicating whether the player has an active pet.
        /// </summary>
        /// <param name="owner">The player to check.</param>
        /// <returns>The pet, or null if none is found.</returns>
        public static Pet Get(Player owner) => Instances.FirstOrDefault(pet => pet.Owner == owner);

        /// <summary>
        /// Creates a pet for a player.
        /// </summary>
        /// <param name="owner">The owner of the pet.</param>
        /// <returns>The created pet.</returns>
        public static Pet Create(Player owner)
        {
            PetPreferences preferences = PetPreferences.Get(owner) ?? new PetPreferences(owner.UserId);
            Pet pet = new Pet(owner, preferences);
            Instances.Add(pet);
            return pet;
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            movementCore.Kill();
            Instances.Remove(this);
            base.Destroy();
        }
    }
}