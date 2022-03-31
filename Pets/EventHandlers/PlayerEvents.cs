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

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnSpawningRagdoll(SpawningRagdollEventArgs)"/>
        public void OnSpawningRagdoll(SpawningRagdollEventArgs ev)
        {
            foreach (Pet pet in Pet.Instances)
            {
                if (ev.Owner == pet.Npc.Player)
                {
                    ev.IsAllowed = false;
                    return;
                }
            }
        }
    }
}