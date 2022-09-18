// -----------------------------------------------------------------------
// <copyright file="NavigationNodeBase.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API.Navigation.Nodes
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using NPCs.API.Extensions;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <inheritdoc />
    public abstract class NavigationNodeBase : MonoBehaviour
    {
        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        public string Name => !gameObject || string.IsNullOrEmpty(gameObject.name) ? "NavigationNode" : $"NavigationNode_{gameObject.name}";

        /// <summary>
        /// Generates node mappings using the types found in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to find the node types from.</param>
        public static void GenerateFromAssembly(Assembly assembly)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.InheritsOrImplements(typeof(NavigationNode<>)) || type.IsAbstract)
                    continue;

                Type genericParameter = type.BaseType!.GetGenericArguments()[0];
                foreach (Object obj in FindObjectsOfType(genericParameter))
                    ((MonoBehaviour)obj).gameObject.AddComponent(type);
            }

            stopwatch.Stop();
            Exiled.API.Features.Log.Debug($"Generated navigation nodes from {assembly.GetName().Name} in {stopwatch.ElapsedMilliseconds}ms.", Plugin.Instance.Config.Debug);
        }

        /// <summary>
        /// Generates the default node mappings.
        /// </summary>
        internal static void GenerateMap() => GenerateFromAssembly(typeof(NavigationNodeBase).Assembly);
    }
}