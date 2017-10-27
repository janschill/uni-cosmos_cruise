using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -------Blocker ------- 
 * This Blocker blocks the Player from cruising down the middle to the end.
 * The Blocker doesn't have a Rigidbody, so it can't just be moved by the player.
 * The Player has to find the platform, which Destroys the Blocker GameObject.
 * 
 * Here the Blocker just pushes the Player, when colliding in to the opposite 
 * direction with a given force.
 * 
 */
public class BlockerController : MonoBehaviour
{
	//private void OnTriggerEnter(Collider collider)
	//{
	//    if(collider.gameObject.tag.Contains("Player"))
	//    {
	//        Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
	//        Vector3 pos = transform.position - collider.gameObject.transform.position;
	//        rb.AddForce((-pos) * 1000);
	//    }
	//}
}
