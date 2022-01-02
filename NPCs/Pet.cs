// -----------------------------------------------------------------------
// <copyright file="Pet.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using NPCs.API;
    using NPCs.Cores;
    using NPCs.Database;
    using NPCs.Database.Models;

    /// <summary>
    /// A class to represent a pet in game.
    /// </summary>
    public class Pet
    {
        private MovementHandler movementHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pet"/> class.
        /// </summary>
        /// <param name="owner">The owner of the pet.</param>
        private Pet(Player owner)
        {
            Owner = owner;
            User ownerUser = UserCache.Get(Owner);
            Preferences = Data.Get?.PetPreferences(ownerUser);
            if (Preferences == null)
            {
                string defaultName = Plugin.Instance.Config.Pets.DefaultName.Replace("{Name}", owner.DisplayNickname ?? owner.Nickname);
                Preferences = new PetPreferences(ownerUser, true, defaultName, RoleType.ClassD.ToId(), -1);
                Data.Send?.PetPreferences(Preferences);
            }

            Npc = new Npc(Identifiers.RoleIdToType(Preferences.Role), Preferences.Name, Plugin.Instance.Config.Pets.Size);
            movementHandler = new MovementHandler(Npc, Owner);
            /* Npc.Navigation.FollowTarget = owner; */
            Npc.Player.IsGodModeEnabled = true;
            if (Preferences.IsShown)
                Npc.Spawn();
        }

        /// <summary>
        /// Gets all existing instances of pets.
        /// </summary>
        public static List<Pet> Instances { get; } = new List<Pet>();

        /// <summary>
        /// Gets the NPC instance.
        /// </summary>
        public Npc Npc { get; private set; }

        /// <summary>
        /// Gets the owner of the pet.
        /// </summary>
        public Player Owner { get; private set; }

        /// <summary>
        /// Gets the owner's preferences for the pet.
        /// </summary>
        public PetPreferences Preferences { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the pet is currently visible.
        /// </summary>
        public bool IsShown
        {
            get => Preferences.IsShown;
            private set
            {
                Preferences.IsShown = value;
                Data.Send?.PetPreferences(Preferences);
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
                Npc.HeldItem = value;
                Preferences.HeldItem = value.ToId();
                Data.Send?.PetPreferences(Preferences);
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
                Data.Send?.PetPreferences(Preferences);
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
                Data.Send?.PetPreferences(Preferences);
            }
        }

        private static Data Data => Plugin.Instance.Data;

        /// <summary>
        /// Creates a pet for the given owner.
        /// </summary>
        /// <param name="owner">The owner of the pet.</param>
        /// <returns>The created pet.</returns>
        public static Pet Create(Player owner)
        {
            Pet pet = owner.GetPet();
            if (pet != null)
                return pet;

            pet = new Pet(owner);
            Instances.Add(pet);
            return pet;
        }

        /// <summary>
        /// Destroys all logic required by the pet.
        /// </summary>
        public void Destroy()
        {
            movementHandler.Kill();
            movementHandler = null;
            Owner = null;
            Preferences = null;
            Npc.Destroy();
            Npc = null;
            Instances.Remove(this);
        }

        /// <summary>
        /// Hides the pet without overriding the preference.
        /// </summary>
        public void ForceHide()
        {
            Npc.Despawn();
        }

        /// <summary>
        /// Hides the pet.
        /// </summary>
        public void Hide()
        {
            movementHandler.Pause();
            Npc.Despawn();
            IsShown = false;
        }

        /// <summary>
        /// Shows the pet.
        /// </summary>
        public void Show()
        {
            Npc.Spawn();
            movementHandler.Play();
            IsShown = true;
        }
    }
}