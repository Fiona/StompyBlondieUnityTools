/*
 * 2018 Stompy Blondie Games
 * Licensed under MIT. See accompanying LICENSE file for details.
 */
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StompyBlondie.Behaviours
{
    /*
     * Super simple monobehaviour that has a public member isOver that you can poll to determine
     * if the mouse is currently hovering over it.
     */
    public class IsMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool isOver = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            isOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isOver = false;
        }
    }
}