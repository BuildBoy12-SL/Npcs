// -----------------------------------------------------------------------
// <copyright file="SerializableVector3.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.API
{
    using System;
    using UnityEngine;

    /// <summary>
    /// A serializable middleman for the <see cref="Vector3"/> class.
    /// </summary>
    [Serializable]
    public class SerializableVector3
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableVector3"/> class.
        /// </summary>
        public SerializableVector3()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableVector3"/> class.
        /// </summary>
        /// <param name="x"><inheritdoc cref="X"/></param>
        /// <param name="y"><inheritdoc cref="Y"/></param>
        /// <param name="z"><inheritdoc cref="Z"/></param>
        public SerializableVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Gets or sets the value of the x axis.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the value of the y axis.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the value of the z axis.
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// Converts a <see cref="SerializableVector3"/> to a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="serializableVector3">The <see cref="SerializableVector3"/> to convert.</param>
        /// <returns>The generated <see cref="Vector3"/>.</returns>
        public static implicit operator Vector3(SerializableVector3 serializableVector3)
        {
            return new Vector3(serializableVector3.X, serializableVector3.Y, serializableVector3.Z);
        }

        /// <summary>
        /// Converts a <see cref="Vector3"/> to a <see cref="SerializableVector3"/>.
        /// </summary>
        /// <param name="vector3">The <see cref="Vector3"/> to convert.</param>
        /// <returns>The generated <see cref="SerializableVector3"/>.</returns>
        public static implicit operator SerializableVector3(Vector3 vector3)
        {
            return new SerializableVector3(vector3.x, vector3.y, vector3.z);
        }

        /// <inheritdoc />
        public override string ToString() => $"({X}, {Y}, {Z})";
    }
}