using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    private List<AudioSource> audioSourceList = new List<AudioSource>();

    [Header("요리제작")]
    [SerializeField] AudioClip cookingSound;

    [Header("요리섭취")]
    [SerializeField] AudioClip cookingEatSound;
    // Start is called before the first frame update

    public void PlayCookingEat()
    {
        PlaySound(cookingEatSound);
    }

    public void PlayCooking()
    {
        PlaySound(cookingSound);
    }

    public void PlaySound(AudioClip audioClip)
    {
        AudioSource audioSource = GetAudioSource(audioClip);
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public AudioSource GetAudioSource(AudioClip audioClip)
    {
        foreach(AudioSource audioSource in audioSourceList)
        {
            if (audioSource.clip == audioClip) return audioSource;
        }

        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSourceList.Add(newAudioSource);
        return newAudioSource;
    }
}
