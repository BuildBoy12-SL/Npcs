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
        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnDestroying(DestroyingEventArgs)"/>
        public void OnDestroying(DestroyingEventArgs ev)
        {
            ev.Player.GetPet()?.Destroy();
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnEnteringPocketDimension(EnteringPocketDimensionEventArgs)"/>
        public void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            if (ev.Player.IsPet(out _))
                ev.IsAllowed = false;
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnSpawned(ReferenceHub)"/>
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (!Round.IsStarted || !ev.Player.CheckPermission("pets.pet"))
                return;

            Pet pet = Pet.GetOrCreate(ev.Player);
            if (ev.Player.IsDead || ev.Player.Role == RoleType.Scp079)
                pet.Npc.Despawn();
            else if (pet.Preferences.IsShown)
                pet.Npc.Spawn();
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnTriggeringTesla(TriggeringTeslaEventArgs)"/>
        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (ev.Player.IsPet(out _))
            {
                ev.IsTriggerable = false;
                ev.IsInIdleRange = false;
            }
        }
    }
}