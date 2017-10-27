using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuSpawnerController : MonoBehaviour
{
    public GameObject playermenu;

    public static bool clicked;

    List<Vector3> list = new List<Vector3>();
    List<Vector3> listoption = new List<Vector3>();

    private const int ROWS = 3;
    private const int COLS = 5;
    private Transform holder;
    private Renderer rend;
    public Material[] materials;

    private void Awake()
    {
        rend = playermenu.GetComponent<Renderer>();
        InitializeList();
        InitializeListOption();
        SpawnPlayers();
    }

    private void InitializeList()
    {
        for (int i = 1; i <= COLS; i++)
        {
            for (int j = 1; j <= ROWS; j++)
            {
                list.Add(new Vector3(200f * i, 800 - (j * 200), 200));
            }
        }
    }

    public static void SetPlayerPref(int i)
    {
        Debug.Log(i);
        PlayerPrefs.SetInt("Material", i);
    }

    private void SpawnPlayers()
    {
        holder = new GameObject("holder").transform;
        int i = 0;

        foreach (Vector3 vec in list)
        {
			rend.material = materials[i];
            GameObject instance = Instantiate(playermenu, vec, Quaternion.identity) as GameObject;
            instance.name = "" + i;
			i++;
            instance.transform.SetParent(holder);
        }
    }

    public void PlayerMover(int i)
    {
        if (i == 1)
        {
            InitializeListOption();
            foreach (Transform trans in holder)
            {
                trans.transform.position = Vector3.MoveTowards(trans.transform.position, RandomPosition(listoption), 10000);
            }
        }

        if (i == 2)
        {
            InitializeList();
            foreach (Transform trans in holder)
            {
                trans.transform.position = Vector3.MoveTowards(trans.transform.position, RandomPosition(list), 10000);
            }
        }
    }

    private void InitializeListOption()
    {
        int x = 0;

        for (int i = 1; i <= 4; i++)
        {
            for (int y = 150; y <= 600; y += 150)
            {
                if (i == 1)
                    x = -80;
                if (i == 2)
                    x = 100;
                if (i == 3)
                    x = 1150;
                if (i == 4)
                    x = 1330;

                listoption.Add(new Vector3(x, y, 200));
            }
        }
    }

    private Vector3 RandomPosition(List<Vector3> list)
    {
        int randomindex = Random.Range(0, list.Count);

        Vector3 randomPosition = list[randomindex];

        list.RemoveAt(randomindex);

        return randomPosition;
    }
}
