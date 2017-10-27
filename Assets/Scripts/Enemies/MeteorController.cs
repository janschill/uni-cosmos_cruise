using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
	GameObject player;

	private void Start ()
	{
		player = GameObject.Find ("Player");
	}

	private void Update ()
	{
		if (gameObject)
		{
			Vector3 diff = player.transform.position - transform.position;

			float distance = diff.magnitude;

			if (distance > 15)
				Destroy (gameObject);
		}
	}
}
