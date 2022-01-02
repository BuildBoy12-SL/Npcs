// -----------------------------------------------------------------------
// <copyright file="Node.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Pathing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Interactables.Interobjects.DoorUtils;
    using UnityEngine;

    /// <summary>
    /// Represents a node an npc can path to.
    /// </summary>
    public class Node : MonoBehaviour
    {
        /// <summary>
        /// Gets a collection of all node instances.
        /// </summary>
        public static Dictionary<string, Node> Dictionary { get; } = new Dictionary<string, Node>();

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the position of the node.
        /// </summary>
        public Vector3 Position => gameObject.transform.position;

        /// <summary>
        /// Gets or sets the name of the room the node is in.
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// Gets or sets serializable data about the room.
        /// </summary>
        public RoomData RoomData { get; set; }

        /// <summary>
        /// Gets a collection of linked nodes.
        /// </summary>
        public HashSet<Node> LinkedNodes { get; } = new HashSet<Node>();

        /// <summary>
        /// Gets or sets the attached door.
        /// </summary>
        public DoorVariant AttachedDoor { get; set; }

        /// <summary>
        /// Gets or sets a nullable kvp to represent an attached elevator.
        /// </summary>
        public KeyValuePair<Lift.Elevator, Lift>? AttachedElevator { get; set; }

        /// <summary>
        /// Gets a collection of items that can spawn in the room.
        /// </summary>
        public HashSet<int> PossibleItems { get; } = new HashSet<int>();

        /// <summary>
        /// Creates a node.
        /// </summary>
        /// <param name="position">The position of the node.</param>
        /// <param name="name">The name of the node.</param>
        /// <param name="room">The name of the room the node resides in.</param>
        /// <returns>The created node.</returns>
        public static Node Create(Vector3 position, string name = "DefaultNode", string room = "")
        {
            if (Dictionary.ContainsKey(name))
                return null;

            GameObject newObject = new GameObject();
            Node node = newObject.AddComponent<Node>();
            node.Name = name;
            node.Room = room;
            Room r = Map.Rooms.Where(rm => rm.Name.RemoveBracketsOnEndOfName().Equals(room, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (r != null)
            {
                node.RoomData = new RoomData
                {
                    Relative = position - r.Position,
                    Rotation = r.Transform.localRotation.eulerAngles.y,
                    Items = node.PossibleItems.ToList(),
                };
            }

            newObject.transform.position = position;
            Dictionary.Add(node.Name, node);
            return node;
        }

        /// <summary>
        /// Creates a node.
        /// </summary>
        /// <param name="roomData">Data of the room to put the node.</param>
        /// <param name="name">The name of the node.</param>
        /// <param name="room">The name of the room the node resides in.</param>
        /// <returns>The created node.</returns>
        public static Node Create(RoomData roomData, string name = "DefaultNode", string room = "")
        {
            if (Dictionary.ContainsKey(name))
                return null;

            GameObject newObject = new GameObject();
            Node node = newObject.AddComponent<Node>();
            node.Name = name;
            node.Room = room;
            node.RoomData = roomData;
            if (roomData.Items != null)
            {
                foreach (int item in roomData.Items)
                {
                    node.PossibleItems.Add(item);
                }
            }

            Room r = Map.Rooms.Where(rm => rm.Name.RemoveBracketsOnEndOfName().Equals(room, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (r != null)
            {
                newObject.transform.position = r.Position + (Quaternion.Euler(0, r.Transform.localRotation.eulerAngles.y - roomData.Rotation, 0) * roomData.Relative);
            }

            Dictionary.Add(node.Name, node);
            return node;
        }

        /// <summary>
        /// Gets a node from a room.
        /// </summary>
        /// <param name="room">The room to get the node from.</param>
        /// <returns>The found node or null if none were found.</returns>
        public static Node Get(Room room) => Get($"AUTO_Room_{room.Name}".Replace(' ', '_'));

        /// <summary>
        /// Gets a node from a name.
        /// </summary>
        /// <param name="name">The name of the node to get.</param>
        /// <returns>The found node or null if none were found.</returns>
        public static Node Get(string name)
        {
            Dictionary.TryGetValue(name, out Node node);
            return node;
        }

        /// <summary>
        /// Clears and destroys all nodes.
        /// </summary>
        public static void Clear()
        {
            List<Node> nodes = Dictionary.Values.ToList();
            foreach (Node node in nodes)
            {
                Destroy(node);
            }

            Dictionary.Clear();
        }

        private void OnDestroy()
        {
            Dictionary.Remove(Name);
        }
    }
}