using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DashSound : MonoBehaviour
{

    [SerializeField] private AudioSource dashSound;

    public float cooldownTime = 1f;
    private bool onCooldown;

    private IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (!onCooldown))
        {
            dashSound.Play();

            StartCoroutine(Cooldown());
        }
    }


    

}
