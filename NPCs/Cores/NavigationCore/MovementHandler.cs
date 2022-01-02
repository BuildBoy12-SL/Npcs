// -----------------------------------------------------------------------
// <copyright file="MovementHandler.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Cores.NavigationCore
{
    using System.Collections.Generic;
    using MEC;
    using NPCs.Enums;
    using UnityEngine;

    /// <summary>
    /// Handles basic movement logic.
    /// </summary>
    public class MovementHandler
    {
        private readonly List<CoroutineHandle> movementCoroutines = new List<CoroutineHandle>();
        private readonly NpcBase npcBase;
        private readonly NavigationCore navigationCore;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementHandler"/> class.
        /// </summary>
        /// <param name="npcBase">The npc to move.</param>
        /// <param name="navigationCore">The navigation core to interact with.</param>
        public MovementHandler(NpcBase npcBase, NavigationCore navigationCore)
        {
            this.npcBase = npcBase;
            this.navigationCore = navigationCore;
        }

        /// <summary>
        /// Kills all current movement.
        /// </summary>
        public void Clear()
        {
            foreach (CoroutineHandle coroutine in movementCoroutines)
                Timing.KillCoroutines(coroutine);

            movementCoroutines.Clear();
        }

        /// <summary>
        /// Guides the npc towards the specified location.
        /// </summary>
        /// <param name="position">The position to go to.</param>
        /// <returns>The estimated time of arrival.</returns>
        public float GoTo(Vector3 position)
        {
            npcBase.IsActionLocked = true;

            foreach (CoroutineHandle coroutine in movementCoroutines)
                Timing.KillCoroutines(coroutine);
            movementCoroutines.Clear();

            Vector3 heading = position - npcBase.Position;
            heading.y = 0;
            Quaternion rotation = Quaternion.LookRotation(heading.normalized);
            float distance = heading.magnitude;
            npcBase.Player.Rotations = new Vector2(rotation.eulerAngles.x, rotation.eulerAngles.y);

            float eta = 0.1f * (distance / (npcBase.Player.CameraTransform.forward / 10 * navigationCore.MovementSpeed).magnitude);
            Move(MovementDirection.Forward);
            movementCoroutines.Add(Timing.CallDelayed(eta, () =>
            {
                Move(MovementDirection.None);
                if (Vector3.Distance(npcBase.Position, position) >= 2f)
                    npcBase.Position = position;

                npcBase.IsActionLocked = false;
            }));

            return eta;
        }

        /// <summary>
        /// Forces the npc to move in the specified direction.
        /// </summary>
        /// <param name="movementDirection">The direction to move.</param>
        public void Move(MovementDirection movementDirection)
        {
            navigationCore.MovementDirection = movementDirection;
            if (navigationCore.IsSprinting)
                npcBase.ReferenceHub.animationController.MoveState = PlayerMovementState.Sprinting;
            else
                npcBase.ReferenceHub.animationController.MoveState = PlayerMovementState.Walking;

            float speed = navigationCore.MovementSpeed;
            switch (movementDirection)
            {
                case MovementDirection.Forward:
                    var pos = npcBase.Position + (npcBase.Player.CameraTransform.forward / 10 * speed);

                    if (!Physics.Linecast(npcBase.Position, pos, npcBase.Player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                        npcBase.Position = pos;
                    break;

                case MovementDirection.Backwards:
                    pos = npcBase.Position - (npcBase.Player.CameraTransform.forward / 10 * speed);

                    if (!Physics.Linecast(npcBase.Position, pos, npcBase.Player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                        npcBase.Position = pos;
                    break;

                case MovementDirection.Right:
                    pos = npcBase.Position + (Quaternion.AngleAxis(90, Vector3.up) * npcBase.Player.CameraTransform.forward / 10 * speed);

                    if (!Physics.Linecast(npcBase.Position, pos, npcBase.Player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                        npcBase.Position = pos;
                    break;

                case MovementDirection.Left:
                    pos = npcBase.Position - (Quaternion.AngleAxis(90, Vector3.up) * npcBase.Player.CameraTransform.forward / 10 * speed);

                    if (!Physics.Linecast(npcBase.Position, pos, npcBase.Player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                        npcBase.Position = pos;
                    break;
            }
        }
    }
}