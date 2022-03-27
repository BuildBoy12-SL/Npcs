// -----------------------------------------------------------------------
// <copyright file="NavigationCore.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Cores.NavigationCore
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using MEC;
    using NPCs.Enums;
    using NPCs.Pathing;
    using UnityEngine;

    /// <summary>
    /// Handles the core navigation logic.
    /// </summary>
    public class NavigationCore
    {
        private readonly Queue<Vector3> followTargetCache = new Queue<Vector3>();
        private readonly NpcBase npcBase;
        private readonly MovementHandler movementHandler;
        private readonly CoroutineHandle coroutineHandle;
        private float movementSpeed;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationCore"/> class.
        /// </summary>
        /// <param name="npcBase">The npc to control.</param>
        public NavigationCore(NpcBase npcBase)
        {
            this.npcBase = npcBase;
            movementHandler = new MovementHandler(npcBase, this);
            coroutineHandle = Timing.RunCoroutine(NavCoroutine());
        }

        /// <summary>
        /// Gets or sets the player to follow.
        /// </summary>
        public Player FollowTarget { get; set; }

        /// <summary>
        /// Gets or sets the maximum distance before the lost behavior will activate.
        /// </summary>
        public float MaximumFollowDistance { get; set; }

        /// <summary>
        /// Gets or sets the behavior that will occur when the maximum distance from the npcs target is hit.
        /// </summary>
        public TargetLostBehavior TargetLostBehavior { get; set; } = TargetLostBehavior.Teleport;

        /// <summary>
        /// Gets or sets a value indicating whether movement should be paused.
        /// </summary>
        public bool IsActionLocked { get; set; }

        /// <summary>
        /// Gets the npc's sprinting speed.
        /// </summary>
        public float SprintSpeed => CharacterClassManager._staticClasses[(int)npcBase.Player.Role.Type].runSpeed;

        /// <summary>
        /// Gets the npc's walking speed.
        /// </summary>
        public float WalkSpeed => CharacterClassManager._staticClasses[(int)npcBase.Player.Role.Type].walkSpeed;

        /// <summary>
        /// Gets a value indicating whether the npc should sprint when they can.
        /// </summary>
        public bool IsSprinting => true;

        /// <summary>
        /// Gets or sets the direction the npc is moving.
        /// </summary>
        public MovementDirection MovementDirection { get; set; }

        /// <summary>
        /// Gets or sets the npc's movement speed.
        /// </summary>
        public float MovementSpeed
        {
            get
            {
                if (movementSpeed > 0)
                    return movementSpeed;

                if (IsSprinting)
                    return SprintSpeed;

                return WalkSpeed;
            }
            set => movementSpeed = value;
        }

        /// <summary>
        /// Kills the navigation coroutine.
        /// </summary>
        public void Kill() => Timing.KillCoroutines(coroutineHandle);

        /// <summary>
        /// Process a <see cref="Node"/>.
        /// </summary>
        /// <param name="targetNode">The node to process.</param>
        /// <param name="currentNode">The current node.</param>
        /// <param name="previous">The previous index.</param>
        /// <param name="visitedNodes">The nodes that have been visited.</param>
        public void ProcessNode(Node targetNode, Node currentNode, int previous, ref Dictionary<Node, int> visitedNodes)
        {
            if (Map.IsLczDecontaminated && currentNode.Position.y < 200f && currentNode.Position.y > -200f)
            {
                visitedNodes.Add(currentNode, int.MinValue);
                return;
            }

            visitedNodes.Add(currentNode, previous + 1);
            if (currentNode == targetNode)
                return;

            foreach (Node node in currentNode.LinkedNodes)
            {
                if (visitedNodes.ContainsKey(node))
                    continue;

                ProcessNode(targetNode, node, previous + 1, ref visitedNodes);
            }
        }

        private IEnumerator<float> NavCoroutine()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(0.1f);

                if (FollowTarget != null)
                {
                    MoveToTarget();
                    continue;
                }

                followTargetCache.Clear();
            }
        }

        private void MoveToTarget()
        {
            if (FollowTarget.IsDead)
                return;

            Vector3 moveDirection = FollowTarget.Position - npcBase.Position;
            Quaternion rot = Quaternion.LookRotation(moveDirection.normalized);
            Vector2 rotation = new Vector2(rot.eulerAngles.x, rot.eulerAngles.y);
            npcBase.Player.Rotations = rotation;
            npcBase.Player.CameraTransform.rotation = rot;

            float distance = moveDirection.magnitude;
            if (distance > MaximumFollowDistance)
            {
                RunTargetLostBehavior();
                followTargetCache.Clear();
                return;
            }
        }

        private void RunTargetLostBehavior()
        {
            if (TargetLostBehavior == TargetLostBehavior.Teleport)
            {
                npcBase.Position = FollowTarget.Position;
                return;
            }

            StopNavigation();
            if (TargetLostBehavior == TargetLostBehavior.Search)
            {
            }
        }

        private void StopNavigation()
        {
            FollowTarget = null;
            Kill();
        }
    }
}