/*
 * 2018 Stompy Blondie Games
 * Licensed under MIT. See accompanying LICENSE file for details.
 */
using UnityEngine;

namespace StompyBlondie.Extensions
{
    /**
     * Extension class for Unity Transform objects
     */
    public static class ExtensionTransform
    {
        /*
         * Searches the child heirarchy of the given transform for the name given.
         */
        public static Transform FindRecursive(this Transform parent, string name)
        {
            if(parent.name == name)
                return parent;
            foreach(Transform child in parent)
            {
                var findRecurse = child.FindRecursive(name);
                if(findRecurse != null)
                    return findRecurse;
            }
            return null;
        }

    }
}