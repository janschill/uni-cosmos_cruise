using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
	public float speed;

	private Rigidbody rb;
	private bool delay = true;
	private Vector3 playerpos;
	private GameObject player;

	void Start ()
	{
		Invoke ("SetDelay", 3);
		player = GameObject.Find ("Player");
		rb = GetComponent<Rigidbody> ();
	}

	void Update ()
	{
		if (player)
		{
			playerpos = player.transform.position;

			Vector3 force = playerpos - transform.position;
			float distance = force.magnitude;

			if (distance < 5f && !delay)
				rb.AddForce (Force (force));
		}
	}

	public Vector3 Force (Vector3 force)
	{
		force.Normalize ();
		force = force * speed;
		return force;
	}

	private void SetDelay ()
	{
		delay = false;
	}
}
