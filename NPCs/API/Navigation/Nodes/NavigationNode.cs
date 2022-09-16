// -----------------------------------------------------------------------
// <copyright file="NavigationNode.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API.Navigation.Nodes
{
    using UnityEngine;

    /// <summary>
    /// Represents a node that contains identifying features that npcs can use to navigate the map.
    /// </summary>
    /// <typeparam name="T">The type of the object it is attached to.</typeparam>
    public abstract class NavigationNode<T> : NavigationNodeBase
        where T : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets the object the node is attached to.
        /// </summary>
        public abstract T AttachedObject { get; protected set; }

        /// <summary>
        /// Sets the <see cref="AttachedObject"/> using the type of <typeparamref name="T"/> to get the component from the <see cref="GameObject"/> this component is attached to.
        /// </summary>
        protected virtual void Awake() => AttachedObject = GetComponent<T>();
    }
}