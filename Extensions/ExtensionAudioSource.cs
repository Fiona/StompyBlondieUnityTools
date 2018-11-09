/*
 * 2018 Stompy Blondie Games
 * Licensed under MIT. See accompanying LICENSE file for details.
 */
using System.Collections;
using UnityEngine;
using StompyBlondie.Utils;

namespace StompyBlondie.Extensions
{
    public static class ExtensionAudioSource
    {
        public static void FadeOut(this AudioSource source, float fadeOutTime = 1f)
        {
            ExtensionMonoBehaviour.GetInstance().StartCoroutine(DoFadeOut(source, fadeOutTime));
        }

        private static IEnumerator DoFadeOut(AudioSource source, float fadeOutTime)
        {
            yield return LerpHelper.QuickTween(
                (v) => source.volume = v, source.volume, 0f, fadeOutTime
                );
            source.Stop();
        }
    }
}