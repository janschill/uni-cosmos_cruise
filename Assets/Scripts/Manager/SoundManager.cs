using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance = null;

    public AudioSource backgroundmusic;
    public AudioSource soundeffects;

    public AudioClip[] clips;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        RandomBackgroundMusic();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        soundeffects.clip = clip;

        soundeffects.Play();
    }

    public void ToggleMute()
    {
        Debug.Log("ToggleMute");

        if (backgroundmusic.mute == false)
            backgroundmusic.mute = true;
        
        if (backgroundmusic.mute == true)
            backgroundmusic.mute = false;
    }

    private void RandomBackgroundMusic()
    {
        //AudioClip clip = backgroundmusic.GetComponent<AudioClip>();
        backgroundmusic = GetComponent<AudioSource>();

        int random = Random.Range(0, clips.Length);

		backgroundmusic.clip = clips[random];

        backgroundmusic.Play();
		
        //clip = clips[random];
    }
}
