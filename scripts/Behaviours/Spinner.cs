/*
 * 2018 Stompy Blondie Games
 * Licensed under MIT. See accompanying LICENSE file for details.
 */
using StompyBlondie.Common.Types;
using UnityEngine;

namespace StompyBlondie.Behaviours
{
    /**
     * Like the Bobber object but spins in a specified direction.
     */
    public class Spinner: MonoBehaviour
    {
        public RotationalDirection direction;
        public float speed = 50f;

        public void Update()
        {
            if(direction == RotationalDirection.AntiClockwise)
                transform.Rotate(Vector3.forward * Time.deltaTime * speed);
            else
                transform.Rotate(-(Vector3.forward * Time.deltaTime * speed));
        }
    }
}