using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorLauncher : MonoBehaviour
{
    public GameObject meteor;
    private GameObject[] instance;
    private Vector3 launcherposition;
    public float speed = 7.0f;

    private void Start()
    {
        launcherposition = transform.position = new Vector3(11, 1, 10);

        instance = new GameObject[GameManager.instance.numOfComets];

        for (int i = 0; i < instance.Length; i++)
        {
            instance[i] = Instantiate(meteor, RandomPosition(), Quaternion.identity) as GameObject;
        }
    }

    private void Update()
    {
        StartCoroutine(ShootMeteor());
    }

    private Vector3 RandomPosition()
    {
        float rand = Random.Range(1, GameManager.instance.gridcontroller.gridsize+1);
        Vector3 vec = new Vector3(GameManager.instance.gridcontroller.gridsize, 1, rand);
        return vec;
    }

    private IEnumerator ShootMeteor()
    {
        int i = 0;
        while (i < GameManager.instance.numOfComets)
        {
            yield return new WaitForSeconds(2);
            instance[i].transform.Translate(Vector3.left * Time.deltaTime * speed);
            i++;
        }
    }
}
