// -----------------------------------------------------------------------
// <copyright file="MapEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Pets.EventHandlers
{
    using Exiled.Events.EventArgs;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers.Player"/>.
    /// </summary>
    public class MapEvents
    {
        /// <inheritdoc cref="Exiled.Events.Handlers.Map.OnPlacingBlood(PlacingBloodEventArgs)"/>
        public void OnPlacingBlood(PlacingBloodEventArgs ev)
        {
            foreach (Pet pet in Pet.Instances)
            {
                if (ev.Player == pet.Npc.Player)
                {
                    ev.IsAllowed = false;
                    return;
                }
            }
        }
    }
}