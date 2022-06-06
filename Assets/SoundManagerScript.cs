using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip playerRunning, fireSound, hitSound, deathSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        playerRunning = Resources.Load<AudioClip> ("playerRunning");
        fireSound = Resources.Load<AudioClip> ("fire");
        hitSound = Resources.Load<AudioClip> ("hit");
        deathSound = Resources.Load<AudioClip> ("death");

        audioSrc = GetComponent<AudioSource> ();

    }

    // Update is called once per frame
    void Update()
    {


        static void PlaySound (string clip)
        {
            switch (clip)
            {
                case "playerRunning":
                    audioSrc.PlayOneShot(playerRunning);
                    break;
                case "fire":
                    audioSrc.PlayOneShot(fireSound);
                    break;
                case "hit":
                    audioSrc.PlayOneShot(hitSound);
                    break;
                case "death":
                    audioSrc.PlayOneShot(deathSound);
                    break;
            }
        }




    }
}
