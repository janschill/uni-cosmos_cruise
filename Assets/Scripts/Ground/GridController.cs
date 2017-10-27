using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/* ------- Level builder -------
 *
 * two different Spawn methods for those objects that have to be placed over
 * normal ground elements and those that can be placesd anywhere
 */
public class GridController : MonoBehaviour
{
	public const int COLUMNS = 10;
	public const int ROWS = 10;

	private Vector3 startposition;
	private Vector3 endposition;

	[System.NonSerialized]
	public int gridsize = 11;

	public GameObject[] grounds;
	public GameObject start;
	public GameObject end;
	public GameObject wormhole;
	public GameObject directionchanger;
	public GameObject empty;
	public GameObject gameoverbox;
	public GameObject[] platform;
	public GameObject blocker;
	public GameObject player;
	public GameObject[] enemies;
	public GameObject[] collectables;

	private Transform gridholder;

	private List<Vector3> gridpositions = new List<Vector3> ();
	private List<Vector3> removedgridpositions = new List<Vector3> ();

	/* ***************** Gets called in GameManager *****************
	 * - increases gridsize every next level
	 * - sets start- and endposition
	 * - spawns all the enemies, collectables, player and so on
     */
	public void BuildLevel ()
	{
		gridsize = COLUMNS + GameManager.level;

		startposition = new Vector3 (1, 0f, gridsize - 1);
		endposition = new Vector3 (gridsize - 1, 0f, 1);

		InitializeList ();

		SpawnGridAtRandom (wormhole, GameManager.difficulty * 3, GameManager.difficulty * 5);
		SpawnGridAtRandom (directionchanger, GameManager.difficulty, GameManager.difficulty);
		SpawnGridAtRandom (empty, GameManager.level, GameManager.level * 4);
		SpawnObjectAtRandom (collectables, 2, 2 + GameManager.difficulty, 1);
		SpawnObjectAtRandom (enemies, GameManager.difficulty, GameManager.difficulty + 5, 2);
		SpawnObjectAtRandom (platform, 1, 1, 0.5f);
		GameObject instance = Instantiate (blocker, new Vector3 (endposition.x, 1, endposition.z), Quaternion.identity) as GameObject;
		instance.name = "Blocker";

		SpawnGrid ();
		SpawnPlayer ();
		SpawnGameOverBox ();
	}

	/* ***************** Some code from Rogoulike Tutorial (Unity.com) *****************
  * saves vectors with possible position in a list
  */
	private void InitializeList ()
	{
		gridpositions.Clear ();
		removedgridpositions.Clear ();

		for (int z = 1; z < gridsize; z++)
		{
			for (int x = 1; x < gridsize; x++)
			{
				if (z == startposition.z && x == startposition.x || z == endposition.z && x == endposition.x)
					;
				else
					gridpositions.Add (new Vector3 (x, 0f, z));
			}
		}
	}

	/* ***************** Instantiates grid prefabs *****************
  * instantiates ground, start and end at the correct position
  */
	private void SpawnGrid ()
	{
		gridholder = new GameObject ("Gridholder").transform;
		gridholder.tag = "Grid";

		int random = Random.Range (0, grounds.Length);

		for (int z = 1; z < gridsize; z++)
		{
			for (int x = 1; x < gridsize; x++)
			{
				GameObject toinstantiate = grounds [random];

				Vector3 position = new Vector3 (x, 0, z);

				for (int i = 0; i < removedgridpositions.Count; i++)
				{
					if (removedgridpositions [i] == position)
						toinstantiate = empty;
				}

				if (position == startposition)
					toinstantiate = start;
				else if (position == endposition)
					toinstantiate = end;

				GameObject instance = Instantiate (toinstantiate, position, Quaternion.identity) as GameObject;

				instance.transform.SetParent (gridholder);

			}
		}
	}

	/* ***************** random *****************
  * two lists, just to keep track of the ones that get removed
  * to spawn specific objects there
  */
	private Vector3 RandomPosition ()
	{
		int randomindex = Random.Range (0, gridpositions.Count);

		Vector3 randomposition = gridpositions [randomindex];

		removedgridpositions.Add (randomposition);

		gridpositions.RemoveAt (randomindex);

		return randomposition;
	}

	private void SpawnGridAtRandom (GameObject prefab, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++)
		{
			Vector3 randomposition = RandomPosition ();

			Instantiate (prefab, randomposition, Quaternion.identity);
		}
	}

	private void SpawnObjectAtRandom (GameObject[] prefab, int minimum, int maximum, float y)
	{
		int objectcount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectcount; i++)
		{
			int randomindex = Random.Range (0, gridpositions.Count);
			Vector3 randomposition = gridpositions [randomindex];

			randomposition.y = y; // attractor has to be height 2, otherwise he would glitch into ground

			GameObject randomenemy = prefab [Random.Range (0, prefab.Length)];

			Instantiate (randomenemy, randomposition, Quaternion.identity);
		}
	}

	private void SpawnPlayer ()
	{
		Vector3 spawnVector = startposition;

		spawnVector.y = 2f;

		GameObject instance = Instantiate (player, spawnVector, Quaternion.identity) as GameObject;
		instance.name = "Player";
	}

	private void SpawnGameOverBox ()
	{
		Vector3 pos = new Vector3 (gridsize / 2f, 1, gridsize / 2);

		GameObject instance = Instantiate (gameoverbox, pos, Quaternion.identity);

		BoxCollider bc = instance.GetComponent<BoxCollider> () as BoxCollider;

		bc.size = new Vector3 (15 + GameManager.level, 5, 15 + GameManager.level);
	}

	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}
}
