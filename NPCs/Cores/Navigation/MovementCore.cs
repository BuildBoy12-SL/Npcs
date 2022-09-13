// -----------------------------------------------------------------------
// <copyright file="MovementCore.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Cores.Navigation
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using MEC;
    using NPCs.API;
    using NPCs.Enums;
    using UnityEngine;

    /// <summary>
    /// Handles the movements of a fake player.
    /// </summary>
    public class MovementCore : Core
    {
        private const float SneakSpeed = 1.8f;
        private readonly CoroutineHandle coroutineHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementCore"/> class.
        /// </summary>
        /// <param name="npc">The Npc to control.</param>
        public MovementCore(Npc npc)
            : base(npc)
        {
            MovementState = PlayerMovementState.Walking;
            coroutineHandle = Timing.RunCoroutine(MovementCoroutine(), Segment.FixedUpdate);
        }

        /// <summary>
        /// Gets or sets the object to follow.
        /// </summary>
        public GameObject CurrentTarget { get; set; }

        /// <summary>
        /// Gets or sets the movement type of the pet.
        /// </summary>
        public PlayerMovementState MovementState
        {
            get => Npc.ReferenceHub.animationController.MoveState;
            set => Npc.ReferenceHub.animationController.MoveState = value;
        }

        /// <summary>
        /// Gets or sets the direction the pet is moving.
        /// </summary>
        public MovementDirection Direction { get; set; } = MovementDirection.Forward;

        /// <summary>
        /// Gets or sets a value indicating whether the movement controller is paused.
        /// </summary>
        public bool IsPaused { get; set; }

        private float WalkSpeed => CharacterClassManager._staticClasses[(int)Npc.Role.Type].walkSpeed;

        private float RunSpeed => CharacterClassManager._staticClasses[(int)Npc.Role.Type].runSpeed;

        /// <summary>
        /// Kills movement control.
        /// </summary>
        public void Kill() => Timing.KillCoroutines(coroutineHandle);

        private void Follow()
        {
            Vector3 moveDirection = CurrentTarget.transform.position - Npc.Position;

            Quaternion rot = Quaternion.LookRotation(moveDirection.normalized);
            Npc.Rotation = new Vector2(rot.eulerAngles.x, rot.eulerAngles.y);

            if (Player.Get(CurrentTarget) is Player player)
                MovementState = player.MoveState;

            switch (moveDirection.magnitude)
            {
                case < 3:
                    return;
                case > 10:
                    Npc.Position = CurrentTarget.transform.position;
                    return;
                default:
                    Move();
                    break;
            }
        }

        private void Move()
        {
            float speed = MovementState switch
            {
                PlayerMovementState.Sneaking => SneakSpeed,
                PlayerMovementState.Sprinting => RunSpeed,
                PlayerMovementState.Walking => WalkSpeed,
                _ => 0f
            };

            Vector3 newPosition = Npc.Position;
            switch (Direction)
            {
                case MovementDirection.Forward:
                    newPosition += Npc.CameraTransform.forward / 10 * speed;
                    break;
                case MovementDirection.Backwards:
                    newPosition -= Npc.CameraTransform.forward / 10 * speed;
                    break;
                case MovementDirection.Right:
                    newPosition += Quaternion.AngleAxis(90, Vector3.up) * Npc.CameraTransform.forward / 10 * speed;
                    break;
                case MovementDirection.Left:
                    newPosition -= Quaternion.AngleAxis(90, Vector3.up) * Npc.CameraTransform.forward / 10 * speed;
                    break;
            }

            if (Npc.Position != newPosition && !Physics.Linecast(Npc.Position, newPosition, FallDamage.StaticGroundMask))
                Npc.Position = newPosition;
        }

        private IEnumerator<float> MovementCoroutine()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(0.1f);
                if (IsPaused || !Npc.IsSpawned)
                    continue;

                if (CurrentTarget != null)
                    Follow();
                else
                    Move();
            }
        }
    }
}