using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

// audio manager class adapted from tutorial https://www.youtube.com/watch?v=6OT43pvUyfY Brackeys
public class AudioManager : MonoBehaviour
{
    public Sound[] soundList;
    private void Awake()
    {
        GameEvents.PlayAudio += PlayAudioClip;
        foreach(Sound s in soundList)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.volume = s.volume;
        }
    }

    public void PlayAudioClip(object sender, PlayAudioArgs e)
    {
        
        Sound s = Array.Find(soundList, Sound => Sound.name == e.name);
        if(s == null)
        {
            Debug.Log("Sound (" + e.name + ") not found");
            return;
        }
        s.source.Play();
    }
}
