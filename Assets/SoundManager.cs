using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource source;
    [SerializeField]
    AudioClip step;
    [SerializeField]
    AudioClip wetStep;
    [SerializeField]
    AudioClip swoosh;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void playStep()
    {
        if (!PlayerStateManager.Instance.OnWater)
            source.PlayOneShot(step);
        else
            source.PlayOneShot(wetStep);
    }

    void playSwoosh()
    {
        source.PlayOneShot(swoosh);
    }
}
