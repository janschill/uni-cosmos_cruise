using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -------CameraController ------- 
 * The CameraController is attached to the GameManger GameObject,
 * where it has a listener in the Update(). When 1 or 2 are getting pressed
 * SetCamera() is called from the GameManager script. And here it just switches 
 * between cam-1 and cam-2, both already being placed in the scene.
 * 
 * Because the grid of the level is growing by one row/col each time the player
 * passes a level, the camera has to adapt. I decided to move the camera back, 
 * higher and increase the orthographic size, which gives us more room to play with.
 * 
 * The only thing outsourced is the player/camera interaction with the close-up cam
 * (cam2), because it involves movement of the player. (PlayerController/Update())
 * 
 */
public class CameraController : MonoBehaviour
{
    public GameObject cameraMain;
    public GameObject cameraFirstPerson;

    public void GetReferences()
    {
        cameraMain = GameObject.Find("CameraMain");
        cameraFirstPerson = GameObject.Find("CameraFirstPerson");
        cameraFirstPerson.SetActive(false);
    }

    public void SetCameraPosition()
    {
        /* main camera orthographic hanging statically above the level */
        Camera.main.transform.position = new Vector3(GameManager.instance.gridcontroller.gridsize / 2f, 5, -6 + GameManager.difficulty);
        Camera.main.transform.rotation = Quaternion.Euler(30f, 0, 0);
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 7 + GameManager.difficulty;
    }

    public void SetCamera(int i)
    {
        if (i == 1)
        {
            cameraFirstPerson.SetActive(false);
            cameraMain.SetActive(true);
            Debug.Log("Cam-1");
        }

        if(i == 2)
        {
            cameraFirstPerson.SetActive(true);
            cameraMain.SetActive(false);
            Debug.Log("Cam-2");
        }
    }
}
