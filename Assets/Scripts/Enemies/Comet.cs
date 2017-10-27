using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Gr")
        {
            collider.gameObject.SetActive(false);
            Debug.Log("cometcollsion");
        }
    }
}
