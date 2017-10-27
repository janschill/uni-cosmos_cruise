using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometLauncher : MonoBehaviour
{
	public GameObject comet;
	private Vector3 position;
	private GameObject instance;

	private void Start ()
	{
		position = transform.position = RandomVector ();
		StartCoroutine (ShootComet ());
	}

	private IEnumerator ShootComet ()
	{
		int i = 0;
		while (i < GameManager.instance.numOfComets)
		{
			yield return new WaitForSeconds (3);
			GameObject instance = Instantiate (comet, position, Quaternion.identity) as GameObject;
			Rigidbody rb = instance.GetComponent<Rigidbody> ();
			rb.AddForce (Vector3.down * 100);
			position = RandomVector ();
			i++;
		}
	}

	private Vector3 RandomVector ()
	{
		Vector3 vec = new Vector3 (RandV (), 15, RandV ());
		return vec;
	}

	private float RandV ()
	{
		return Random.Range (1, GameManager.instance.gridcontroller.gridsize);
	}
}
