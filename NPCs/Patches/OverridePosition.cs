// -----------------------------------------------------------------------
// <copyright file="OverridePosition.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace NPCs.Patches
{
#pragma warning disable SA1313
    using HarmonyLib;
    using UnityEngine;

    /// <summary>
    /// Patches <see cref="PlayerMovementSync.OverridePosition"/> to prevent calls to <see cref="PlayerMovementSync.TargetSetRotation"/> when the player is a npc.
    /// </summary>
    [HarmonyPatch(typeof(PlayerMovementSync), nameof(PlayerMovementSync.OverridePosition))]
    internal static class OverridePosition
    {
        private static bool Prefix(PlayerMovementSync __instance, Vector3 pos, float rot, bool forceGround = false)
        {
            if (__instance == null)
                return true;

            if (!NpcBase.Dictionary.ContainsKey(__instance.gameObject))
                return true;

            if (forceGround && Physics.Raycast(pos, Vector3.down, out var hitInfo, 100f, __instance.CollidableSurfaces))
            {
                pos = hitInfo.point + (Vector3.up * 1.3f);
                pos = new Vector3(pos.x, pos.y - ((1f - __instance._hub.transform.localScale.y) * 1.3f), pos.z);
            }

            __instance.ForcePosition(pos);
            return false;
        }
    }
}