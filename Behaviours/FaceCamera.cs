/*
 * 2018 Stompy Blondie Games
 * Licensed under MIT. See accompanying LICENSE file for details.
 */
using UnityEngine;

namespace StompyBlondie.Behaviours
{
	/*
	 * Behaviour that constantly "looks" at the camera. Useful
	 * for billboarded objects.
	 */
	public class FaceCamera: MonoBehaviour
	{
		void Awake()
		{
			Update();
		}
		
		void Update()
		{
			transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
				Camera.main.transform.rotation * Vector3.up);
		}
	}
}

