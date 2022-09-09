// -----------------------------------------------------------------------
// <copyright file="MovementCore.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Cores.Navigation
{
    using System.Collections.Generic;
    using MEC;
    using NPCs.Enums;
    using UnityEngine;

    /// <summary>
    /// Handles the movements of a fake player.
    /// </summary>
    public class MovementCore
    {
        private const float SneakSpeed = 1.8f;
        private readonly Npc npc;
        private readonly CoroutineHandle coroutineHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementCore"/> class.
        /// </summary>
        /// <param name="npc">The npc to control.</param>
        public MovementCore(Npc npc)
        {
            this.npc = npc;
            Movement = PlayerMovementState.Sprinting;
            coroutineHandle = Timing.RunCoroutine(MovementCoroutine(), Segment.FixedUpdate);
        }

        /// <summary>
        /// Gets or sets the object to follow.
        /// </summary>
        public GameObject FollowTarget { get; set; }

        /// <summary>
        /// Gets or sets the movement type of the pet.
        /// </summary>
        public PlayerMovementState Movement
        {
            get => npc.ReferenceHub.animationController.MoveState;
            set => npc.ReferenceHub.animationController.MoveState = value;
        }

        /// <summary>
        /// Gets or sets the direction the pet is moving.
        /// </summary>
        public MovementDirection Direction { get; set; } = MovementDirection.Forward;

        /// <summary>
        /// Gets or sets a value indicating whether the movement controller is paused.
        /// </summary>
        public bool IsPaused { get; set; }

        private float WalkSpeed => CharacterClassManager._staticClasses[(int)npc.Role].walkSpeed;

        private float RunSpeed => CharacterClassManager._staticClasses[(int)npc.Role].runSpeed;

        /// <summary>
        /// Kills movement control.
        /// </summary>
        public void Kill() => Timing.KillCoroutines(coroutineHandle);

        private void Follow()
        {
            Vector3 moveDirection = FollowTarget.transform.position - npc.Position;

            Quaternion rot = Quaternion.LookRotation(moveDirection.normalized);
            npc.Rotation = new Vector2(rot.eulerAngles.x, rot.eulerAngles.y);

            switch (moveDirection.magnitude)
            {
                case < 3:
                    return;
                case > 10:
                    npc.Position = FollowTarget.transform.position;
                    return;
                default:
                    Move();
                    break;
            }
        }

        private void Move()
        {
            float speed = Movement switch
            {
                PlayerMovementState.Sneaking => SneakSpeed,
                PlayerMovementState.Sprinting => RunSpeed,
                PlayerMovementState.Walking => WalkSpeed,
                _ => 0f
            };

            Vector3 newPosition = npc.Position;
            switch (Direction)
            {
                case MovementDirection.Forward:
                    newPosition += npc.CameraTransform.forward / 10 * speed;
                    break;
                case MovementDirection.Backwards:
                    newPosition -= npc.CameraTransform.forward / 10 * speed;
                    break;
                case MovementDirection.Right:
                    newPosition += Quaternion.AngleAxis(90, Vector3.up) * npc.CameraTransform.forward / 10 * speed;
                    break;
                case MovementDirection.Left:
                    newPosition -= Quaternion.AngleAxis(90, Vector3.up) * npc.CameraTransform.forward / 10 * speed;
                    break;
            }

            if (npc.Position != newPosition && !Physics.Linecast(npc.Position, newPosition, FallDamage.StaticGroundMask))
                npc.Position = newPosition;
        }

        private IEnumerator<float> MovementCoroutine()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(0.1f);
                if (IsPaused || !npc.IsSpawned)
                    continue;

                if (FollowTarget != null)
                    Follow();
                else
                    Move();
            }
        }
    }
}