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
    using MEC;
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
            if (ev.Player is Pet)
                ev.IsAllowed = false;
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnChangingRole(ChangingRoleEventArgs)"/>
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole is RoleType.Spectator or RoleType.Scp079 && ev.Player.GetPet() is Pet pet)
                pet.Destroy();
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnSpawned(ReferenceHub)"/>
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (!Round.IsStarted || !ev.Player.CheckPermission("pets.pet") || ev.Player.Role.Type == RoleType.Tutorial)
                return;

            Timing.CallDelayed(1f, () =>
            {
                Pet pet = Pet.GetOrCreate(ev.Player);
                if (pet.PetPreferences.IsShown)
                    pet.IsSpawned = true;
            });
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnTriggeringTesla(TriggeringTeslaEventArgs)"/>
        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (ev.Player is not Pet)
                return;

            ev.IsTriggerable = false;
            ev.IsInIdleRange = false;
        }
    }
}