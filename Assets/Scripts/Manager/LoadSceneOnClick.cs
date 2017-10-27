using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* ------- SceneLoader ------- 
 * This script just loads scenes by index.
 * Mostly used for buttons (Start,Back etc.)
 */
public class LoadSceneOnClick : MonoBehaviour
{
    public void LoadByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void DestroyGMInstance()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(SoundManager.instance.gameObject);
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }
}
