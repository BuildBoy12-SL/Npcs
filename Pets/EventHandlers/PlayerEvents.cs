// -----------------------------------------------------------------------
// <copyright file="PlayerEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets.EventHandlers
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Exiled.Permissions.Extensions;
    using Pets.API;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers.Player"/>.
    /// </summary>
    public class PlayerEvents
    {
        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnChangingRole(ChangingRoleEventArgs)"/>
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!ev.Player.CheckPermission("pets.pet") || !Round.IsStarted)
                return;

            Pet pet = Pet.GetOrCreate(ev.Player);
            if (ev.NewRole == RoleType.Spectator || ev.NewRole == RoleType.None || ev.NewRole == RoleType.Scp079)
                pet.Npc.Despawn();
            else if (pet.Preferences.IsShown)
                pet.Npc.Spawn();
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnDestroying(DestroyingEventArgs)"/>
        public void OnDestroying(DestroyingEventArgs ev)
        {
            ev.Player.GetPet()?.Destroy();
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnSpawningRagdoll(SpawningRagdollEventArgs)"/>
        public void OnSpawningRagdoll(SpawningRagdollEventArgs ev)
        {
            if (ev.Owner.IsPet(out _))
                ev.IsAllowed = false;
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnShot(ShotEventArgs)"/>
        public void OnShot(ShotEventArgs ev)
        {
            if (ev.Target.IsPet(out _))
                ev.CanHurt = false;
        }
    }
}