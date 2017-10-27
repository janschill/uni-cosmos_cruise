using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PowerController : MonoBehaviour
{
	public enum Powers
	{
		jump,
		shield}

	;
	// for different powerups (collectables)
	private Image[] buttonsJump;
	private Image[] buttonsShield;

	private void Awake ()
	{
		buttonsJump = new Image[GameManager.MAXPOWER];
		buttonsShield = new Image[GameManager.MAXPOWER];
	}

	public void SetPowers (Powers pow)
	{
		for (int i = 0; i < GameManager.MAXPOWER; i++)
		{
			if (pow == Powers.jump)
			{
				if (i < GameManager.numOfJumps)
					buttonsJump [i].fillCenter = true;
				else
					buttonsJump [i].fillCenter = false;
			}
			if (pow == Powers.shield)
			{
				if (i < GameManager.numOfShields)
					buttonsShield [i].fillCenter = true;
				else
					buttonsShield [i].fillCenter = false;
			}
		}
	}

	public void IncreasePower (Powers pow)
	{
		if (pow == Powers.jump && GameManager.numOfJumps < 7)
			GameManager.numOfJumps++;
		if (pow == Powers.shield && GameManager.numOfShields < 7)
			GameManager.numOfShields++;

		SetPowers (pow);
	}

	public void DecreasePower (Powers pow)
	{
		if (pow == Powers.jump && GameManager.numOfJumps > 0)
			GameManager.numOfJumps--;
		if (pow == Powers.shield && GameManager.numOfShields > 0)
			GameManager.numOfShields--;

		SetPowers (pow);
	}

	public void GetReferences ()
	{
		for (int i = 0; i < 7; i++)
		{
			buttonsJump [i] = GameObject.Find ("ButtonJump" + i).GetComponent<Image> ();
			buttonsShield [i] = GameObject.Find ("ButtonShield" + i).GetComponent<Image> ();
		}
	}
}
