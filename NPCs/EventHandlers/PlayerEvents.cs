// -----------------------------------------------------------------------
// <copyright file="PlayerEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.EventHandlers
{
    using Exiled.Events.EventArgs;
    using Exiled.Permissions.Extensions;
    using NPCs.API;
    using NPCs.Database;
    using PlayerHandlers = Exiled.Events.Handlers.Player;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers.Player"/>.
    /// </summary>
    public class PlayerEvents
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerEvents"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public PlayerEvents(Plugin plugin) => this.plugin = plugin;

        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            PlayerHandlers.ChangingRole += OnChangingRole;
            PlayerHandlers.Destroying += OnDestroying;
            PlayerHandlers.Verified += OnVerified;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            PlayerHandlers.ChangingRole -= OnChangingRole;
            PlayerHandlers.Destroying -= OnDestroying;
            PlayerHandlers.Verified -= OnVerified;
        }

        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!ev.Player.CheckPermission("rr.pets"))
                return;

            Pet pet = ev.Player.GetPet() ?? Pet.Create(ev.Player);
            if (ev.NewRole == RoleType.Spectator || ev.NewRole == RoleType.None || ev.NewRole == RoleType.Scp079)
                pet.ForceHide();
            else if (pet.Preferences.IsShown)
                pet.Show();
        }

        private void OnDestroying(DestroyingEventArgs ev)
        {
            UserCache.Remove(ev.Player);
        }

        private void OnVerified(VerifiedEventArgs ev)
        {
            UserCache.Add(ev.Player);
        }
    }
}