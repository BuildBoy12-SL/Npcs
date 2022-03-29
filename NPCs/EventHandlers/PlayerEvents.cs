// -----------------------------------------------------------------------
// <copyright file="PlayerEvents.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.EventHandlers
{
    using Exiled.Events.EventArgs;
    using PlayerHandlers = Exiled.Events.Handlers.Player;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers.Player"/>.
    /// </summary>
    public class PlayerEvents
    {
        /// <summary>
        /// Subscribes to all required events.
        /// </summary>
        public void Subscribe()
        {
            PlayerHandlers.ReceivingEffect += OnReceivingEffect;
        }

        /// <summary>
        /// Unsubscribes from all required events.
        /// </summary>
        public void Unsubscribe()
        {
            PlayerHandlers.ReceivingEffect -= OnReceivingEffect;
        }

        private void OnReceivingEffect(ReceivingEffectEventArgs ev)
        {
            if (NpcBase.Dictionary.ContainsKey(ev.Player.GameObject))
                ev.IsAllowed = false;
        }
    }
}