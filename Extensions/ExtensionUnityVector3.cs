/*
 * 2018 Stompy Blondie Games
 * Licensed under MIT. See accompanying LICENSE file for details.
 */
using UnityEngine;
using StompyBlondie.Common.Types;

namespace StompyBlondie.Extensions
{
    public static class ExtensionUnityVector3
    {
        public static Pos ToPos(this Vector3 input)
        {
            return new Pos(x:input.x, y:input.z, layer:input.y);
        }

        public static Vector3 ToVector3(this Pos input)
        {
            return new Vector3(input.X, input.Layer, input.Y);
        }
    }
}