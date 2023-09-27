using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSound, sfxSound, enemySound;
    public AudioSource musicSource, sfxSource, enemySource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("MainMenuMusic");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSound, x => x.name == name);

        if(s == null)
        {
            Debug.Log("Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSound, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlayEnemySound(string name)
    {
        Sound s = Array.Find(enemySound, x => x.name == name);

        if(s == null)
        {
            Debug.Log("Not Found");
        }
        else
        {
            enemySource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusicButton()
    {
        musicSource.mute = !musicSource.mute;
    }

}
