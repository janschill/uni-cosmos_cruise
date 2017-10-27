using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
	private GameObject blocker;

	private void Start ()
	{
		blocker = GameObject.Find ("Blocker");
	}

	private void OnTriggerEnter (Collider collider)
	{
		if (collider.gameObject.tag.Contains ("Player"))
		{
			Destroy (blocker);
		}
	}
}
