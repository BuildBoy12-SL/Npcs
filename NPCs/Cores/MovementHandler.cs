// -----------------------------------------------------------------------
// <copyright file="MovementHandler.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Cores
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using MEC;
    using NPCs.Enums;
    using UnityEngine;

    /// <summary>
    /// Handles the movements of a fake player.
    /// </summary>
    public class MovementHandler
    {
        private readonly Npc npc;
        private readonly Player toFollow;
        private readonly CoroutineHandle coroutineHandle;
        private bool isPaused = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementHandler"/> class.
        /// </summary>
        /// <param name="npc">The npc to control.</param>
        /// <param name="toFollow">The player to follow.</param>
        public MovementHandler(Npc npc, Player toFollow)
        {
            this.npc = npc;
            this.toFollow = toFollow;
            coroutineHandle = Timing.RunCoroutine(MovementCoroutine(), Segment.FixedUpdate);
        }

        /// <summary>
        /// Gets or sets the movement type of the pet.
        /// </summary>
        public PlayerMovementState Movement
        {
            get => npc.Player.ReferenceHub.animationController.MoveState;
            set => npc.Player.ReferenceHub.animationController.UserCode_CmdChangeSpeedState((byte)value);
        }

        /// <summary>
        /// Gets or sets the direction the pet is moving.
        /// </summary>
        public MovementDirection Direction { get; set; }

        private float RunSpeed => CharacterClassManager._staticClasses[(int)toFollow.Role.Type].runSpeed;

        private float WalkSpeed => CharacterClassManager._staticClasses[(int)toFollow.Role.Type].walkSpeed;

        /// <summary>
        /// Unpauses movement control.
        /// </summary>
        public void Play() => isPaused = false;

        /// <summary>
        /// Pauses movement control.
        /// </summary>
        public void Pause() => isPaused = true;

        /// <summary>
        /// Kills movement control.
        /// </summary>
        public void Kill() => Timing.KillCoroutines(coroutineHandle);

        private IEnumerator<float> MovementCoroutine()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(0.1f);
                if (isPaused)
                    continue;

                if (npc.Player is null)
                    continue;

                Vector3 moveDirection = toFollow.Position - npc.Player.Position;

                Quaternion rot = Quaternion.LookRotation(moveDirection.normalized);
                Vector2 rotation = new Vector2(rot.eulerAngles.x, rot.eulerAngles.y);
                npc.Player.Rotations = rotation;
                npc.Player.CameraTransform.rotation = rot;

                if (moveDirection.magnitude < 3)
                    continue;

                if (moveDirection.magnitude > 10)
                {
                    npc.Position = toFollow.Position;
                    continue;
                }

                Movement = toFollow.MoveState == PlayerMovementState.Sneaking ? PlayerMovementState.Sneaking : PlayerMovementState.Sprinting;
                Direction = MovementDirection.Forward;

                float speed = 0f;
                switch (Movement)
                {
                    case PlayerMovementState.Sneaking:
                        speed = 1.8f;
                        break;

                    case PlayerMovementState.Sprinting:
                        speed = RunSpeed;
                        break;

                    case PlayerMovementState.Walking:
                        speed = WalkSpeed;
                        break;
                }

                bool wall = false;
                switch (Direction)
                {
                    case MovementDirection.Forward:
                        var pos = npc.Position + (npc.Player.CameraTransform.forward / 10 * speed);

                        if (!Physics.Linecast(npc.Position, pos, npc.Player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                            npc.Position = pos;
                        else
                            wall = true;
                        break;

                    case MovementDirection.Backwards:
                        pos = npc.Position - (npc.Player.CameraTransform.forward / 10 * speed);

                        if (!Physics.Linecast(npc.Position, pos, npc.Player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                            npc.Position = pos;
                        else
                            wall = true;
                        break;

                    case MovementDirection.Right:
                        pos = npc.Position + (Quaternion.AngleAxis(90, Vector3.up) * npc.Player.CameraTransform.forward / 10 * speed);

                        if (!Physics.Linecast(npc.Position, pos, npc.Player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                            npc.Position = pos;
                        else
                            wall = true;
                        break;

                    case MovementDirection.Left:
                        pos = npc.Position - (Quaternion.AngleAxis(90, Vector3.up) * npc.Player.CameraTransform.forward / 10 * speed);

                        if (!Physics.Linecast(npc.Position, pos, npc.Player.ReferenceHub.playerMovementSync.CollidableSurfaces))
                            npc.Position = pos;
                        else
                            wall = true;
                        break;
                }

                if (wall)
                {
                    Direction = MovementDirection.None;
                }
            }
        }
    }
}