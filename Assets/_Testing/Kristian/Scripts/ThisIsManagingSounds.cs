using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisIsManagingSounds : MonoBehaviour
{

    public float soundToPlay = -1.0f;
    public AudioClip[] audioClip;
    AudioSource audio;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (soundToPlay > -1.0f)
        {
            if (!audio.isPlaying)
            PlaySound((int)soundToPlay, 1);
            soundToPlay = -1.0f;
        }

    }

    void PlaySound(int clip, float volumeScale)
    {
        audio.PlayOneShot(audioClip[clip], volumeScale);
    }
}
