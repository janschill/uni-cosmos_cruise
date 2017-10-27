using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ------- MainMenu Sphere ------- 
 * Here I set the interaction with a single sphere from the main menu
 * you can hover the sphere, which scales it and
 * pressing will rotate the sphere indicating, that is selected
 * as the texture to be played with
 * 
 */
public class PlayerMenuColorController : MonoBehaviour
{
	private bool mouseover;
	private bool clicked;
	public float speed;

	private void Update ()
	{
		/* scaling the sphere on hovering */
		if (mouseover)
			transform.localScale = new Vector3 (200, 200, 200);
		else
			transform.localScale = new Vector3 (100, 100, 100);

		/* rotating the sphere on clicked */
		if (clicked)
			transform.RotateAround (transform.position, Vector3.up, Time.deltaTime * speed);
	}

	/* set the bools for Update() */
	private void OnMouseDown ()
	{
		if (clicked)
			clicked = false;
		else if (!clicked)
			clicked = true;


		/* call the method in PlayerMenuSpawnerController to save the last clicked element*/
		PlayerMenuSpawnerController.SetPlayerPref (System.Int32.Parse (transform.gameObject.name));
		Debug.Log (System.Int32.Parse (transform.gameObject.name));
	}

	private void OnMouseOver ()
	{
		mouseover = true;
	}

	private void OnMouseExit ()
	{
		mouseover = false;
	}
}
