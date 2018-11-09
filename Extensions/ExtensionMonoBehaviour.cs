/*
 * 2018 Stompy Blondie Games
 * Licensed under MIT. See accompanying LICENSE file for details.
 */
using UnityEngine;

namespace StompyBlondie.Extensions
{
    public class ExtensionMonoBehaviour: MonoBehaviour
    {
        private static ExtensionMonoBehaviour instance;

        public static ExtensionMonoBehaviour GetInstance()
        {
            if(instance == null)
            {
                var obj = new GameObject("Extension MonoBehaviour");
                instance = obj.AddComponent<ExtensionMonoBehaviour>();
            }
            return instance;
        }
    }
}