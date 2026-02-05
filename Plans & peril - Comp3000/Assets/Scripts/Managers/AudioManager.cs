using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource BackgroundMusic;
    public AudioSource AttackSound;
    public AudioSource BuffSound;
    public AudioSource DebuffSound;
    public AudioSource DeathSound;
    public AudioSource UIClickSound;

    public List<AudioClip> hitSoundVariations;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void PlayBackgroundMusic(AudioClip clip)
    {
        BackgroundMusic.PlayOneShot(clip);
    }
    public void StopBackgroundMusic()
    {
       BackgroundMusic.Stop(); 
    }
    public void PlayAttackSound()
    {
        AttackSound.clip = hitSoundVariations[Random.RandomRange(0,hitSoundVariations.Count)];
        AttackSound.Play();
    }
    public void PlayBuffSound()
    {
        BuffSound.Play();
    }
    public void PlayDebuffSound()
    {
        DebuffSound.Play();
    }
    public void PlayDeathSound()
    {
        DeathSound.Play();
    }
    public void PlayUIClickSound()
    {
        UIClickSound.Play();
    }
}
