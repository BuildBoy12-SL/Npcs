// -----------------------------------------------------------------------
// <copyright file="ReflectionExtensions.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API.Extensions
{
    using System;
    using System.Linq;

    /// <summary>
    /// Miscellaneous <see cref="System.Reflection"/> extensions.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Checks if a <see cref="Type"/> inherits or implements a given <see cref="Type"/>.
        /// </summary>
        /// <param name="child">The <see cref="Type"/> to check for inheritance or implementations.</param>
        /// <param name="parent">The <see cref="Type"/> to check the child for.</param>
        /// <returns>Whether the child inherits or implements the given parent type.</returns>
        public static bool InheritsOrImplements(this Type child, Type parent)
        {
            parent = ResolveGenericTypeDefinition(parent);

            Type currentChild = child.IsGenericType
                ? child.GetGenericTypeDefinition()
                : child;

            while (currentChild != typeof(object))
            {
                if (parent == currentChild || HasAnyInterfaces(parent, currentChild))
                    return true;

                currentChild = currentChild.BaseType is { IsGenericType: true }
                    ? currentChild.BaseType.GetGenericTypeDefinition()
                    : currentChild.BaseType;

                if (currentChild == null)
                    return false;
            }

            return false;
        }

        private static bool HasAnyInterfaces(Type parent, Type child)
        {
            return child.GetInterfaces().Any(type =>
            {
                Type currentInterface = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                return currentInterface == parent;
            });
        }

        private static Type ResolveGenericTypeDefinition(Type parent)
        {
            bool shouldUseGenericType = !(parent.IsGenericType && parent.GetGenericTypeDefinition() != parent);
            if (parent.IsGenericType && shouldUseGenericType)
                parent = parent.GetGenericTypeDefinition();
            return parent;
        }
    }
}