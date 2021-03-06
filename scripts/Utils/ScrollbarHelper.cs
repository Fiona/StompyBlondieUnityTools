/*
 * 2018 Stompy Blondie Games
 * Licensed under MIT. See accompanying LICENSE file for details.
 */
using UnityEngine;
using UnityEngine.UI;

namespace StompyBlondie.Utils
{
    /**
     * Static class that adds functionality to Scrollbar objects. Like matching the size of the scrollbar
     * to the content and scrolling to particular objects.
     */
    public static class ScrollbarHelper
    {
        /**
         * Sets the display and content size of the passed scrollbar to match the content.
         */
        public static void SetupScrollbar(Scrollbar scrollbar, GameObject content, RectTransform contentWindow)
        {
            if(content.transform.childCount == 0)
            {
                scrollbar.gameObject.SetActive(false);
                return;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());

            // Get content and scroll window heights
            var heightOfScrollWindow = contentWindow.rect.height;
            var heightOfContent = content.GetComponent<RectTransform>().sizeDelta.y;

            // No need for scroll bar if content inside scroll
            if(heightOfContent < heightOfScrollWindow)
            {
                scrollbar.gameObject.SetActive(false);
                return;
            }

            // Set size of scroll bar handle
            var scrollbarOverflow = heightOfContent - heightOfScrollWindow;
            scrollbar.size = 1f - (scrollbarOverflow / heightOfScrollWindow);
            if(scrollbar.size < .1f)
                scrollbar.size = .1f;

            // Set callback for setting value of scroll bar
            scrollbar.onValueChanged.AddListener((val) =>
            {
                var _heightOfScrollWindow = contentWindow.rect.height;
                var _heightOfContent = content.GetComponent<RectTransform>().sizeDelta.y;
                var overflow = System.Math.Abs(_heightOfContent - _heightOfScrollWindow);
                content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, overflow * val);
            });
        }

        /**
         * Smoothly slides the scrollbar to the specified value between 0f and 1f.
         */
        public static void SetScrollbarTo(Scrollbar scrollbar, float value, float duration = .2f)
        {
            scrollbar.StartCoroutine(
                LerpHelper.QuickTween(
                    (float val) => { scrollbar.value = val; },
                    scrollbar.value,
                    value,
                    duration,
                    lerpType:LerpHelper.Type.Exponential
                )
            );
        }

        /**
         * Given a scroll bar, it's content and an element inside the content. This will smoothly scroll
         * to the element in question.
         */
        public static void ScrollbarAutoFocusToElement(Scrollbar scrollbar, GameObject content,
            RectTransform contentWindow, RectTransform element)
        {
            if(!scrollbar.gameObject.activeSelf)
                return;

            var heightOfScrollWindow = contentWindow.rect.height;
            var heightOfContent = content.GetComponent<RectTransform>().sizeDelta.y;
            var halfScrollHeight = (heightOfScrollWindow / 2);

            var elementY = Mathf.Abs(element.anchoredPosition.y);
            if(elementY < halfScrollHeight)
                SetScrollbarTo(scrollbar, 0f);
            else if(elementY > heightOfContent - halfScrollHeight)
            {
                SetScrollbarTo(scrollbar, 1f);
            }
            else
            {
                SetScrollbarTo(scrollbar, elementY / heightOfContent);
            }
        }

    }
}