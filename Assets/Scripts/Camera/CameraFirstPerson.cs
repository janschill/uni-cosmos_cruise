using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ------- CamFirstPerson -------
 * The FirstPerson-Cam
 *
 */

public class CameraFirstPerson : MonoBehaviour
{
	private GameObject player;
	private Vector3 distance;
	private Vector3 currentdistance;
	public float speed;

	private void Start ()
	{
		player = GameObject.Find ("Player");

		transform.position = player.transform.position;
		transform.rotation = Quaternion.Euler (5, 0, 0);
		distance = transform.position - player.transform.position + new Vector3 (0, .25f, -1);
		currentdistance = distance;

		Debug.Log ("Playerobject: " + player.name);
	}

	private void LateUpdate ()
	{
		transform.position = player.transform.position + currentdistance;

		float move = Input.GetAxis ("Horizontal") * speed * Time.deltaTime;

		if (!Mathf.Approximately (move, 0f))
		{
			transform.RotateAround (player.transform.position, Vector3.up, move);
			currentdistance = transform.position - player.transform.position;
		}
	}
}
