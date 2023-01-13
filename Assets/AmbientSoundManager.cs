using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void playSound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
