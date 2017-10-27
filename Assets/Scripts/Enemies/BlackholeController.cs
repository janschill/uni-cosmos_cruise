using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ------- Attractor/Black Hole ------- 
 * here I am using a gravitational attraction to mimic a black hole in space
 * at the moment (10.06.17) it's programmed to only attract the player and
 * destroy it when he comes to close.
 * 
 * to make this work I used the physics engine within in the rigidbodies
 * and some physics formula: F = Gm1m2/r^2 * a (a = unit vector)
 * 
 * in the Attract() method we calculate the force in which the bigger object 
 * (mass) in this case (no matter what) the attractor. this vector we have to
 * apply to the to be pulled object (player)
 * 
 * I used the help of the book Nature of Coding by Daniel Shiffman
 * to aquire the fundamentals to code the Attract()-method.
 * 
 */

public class BlackholeController : MonoBehaviour
{
	public float amplitude;
	public float speed;

	public float g = 1.5f;

	public AudioClip blackholesound;

	private Vector3 attposition;
	private Vector3 playerposition;
	private GameObject player;
	private Rigidbody rb, playerrb;
	private Vector3 defaultpos;
	private Vector3 temp;
	private bool delay = true;
	private float dist;

	void Start ()
	{
		player = GameObject.Find ("Player");
		playerrb = player.GetComponent<Rigidbody> ();
		rb = GetComponent<Rigidbody> ();

		temp = transform.position;
		defaultpos.y = temp.y;

		Invoke ("SetDelay", 3);
	}

	void Update ()
	{
		IncreaseMass ();

		/* create a hovering effect */
		temp.y = defaultpos.y + amplitude * Mathf.Sin (speed * Time.time);

		attposition = transform.position = temp;

		if (player && playerrb)
		{
			playerposition = player.transform.position;
			float distance = (playerposition - transform.position).magnitude;

			if (distance < (4 + dist) && !delay)
			{
				Vector3 force = Attract ();
				playerrb.AddForce (force);
			}
		}
	}

	Vector3 Attract ()
	{
		Vector3 force = attposition - player.transform.position;
		float distance = force.magnitude;
		distance = Mathf.Clamp (distance, 5f, 10f);

		force.Normalize ();
		float strength = (g * rb.mass * playerrb.mass) / (distance * distance);
		force = force * strength;

		return force;
	}

	private void IncreaseMass ()
	{
		transform.localScale += new Vector3 (0.0005f, 0.0005f, 0.0005f);
		rb.mass += 0.008f;
		dist += 0.0008f;
	}

	private void SetDelay ()
	{
		delay = false;
	}

	private void OnTriggerEnter (Collider collider)
	{
		if (collider.gameObject.tag.Contains ("Player") && player)
		{
			Destroy (player);
			GameManager.instance.GameOver ();
			SoundManager.instance.PlaySoundEffect (blackholesound);
		}
	}
}
