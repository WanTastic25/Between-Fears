using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FuseHandler : MonoBehaviour
{
    public GameObject fuseObj;
    public Animator fuseAnimator;

    public Light[] lights;
    public AudioSource[] sounds;

    public bool playerNear;
    public bool fuseOn;
    public bool otherFuseOn;

    public playerInteractions playerInteract;

    private void Start()
    {
        lights = FindObjectsOfType<Light>();
        sounds = FindObjectsOfType<AudioSource>();

        playerNear = false;
        fuseOn = true;
        otherFuseOn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerNear = true;
            playerInteract.interactNotiHolder.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerNear = false;
        }
    }

    private void Update()
    {
        if (gameObject.name != "elevatorFuse" && playerNear == true && Input.GetKeyDown(KeyCode.E))
        {
            fuseAnimator.SetTrigger("pushed");

            var fuseSound = FindObjectOfType<audioManager>().sounds.FirstOrDefault(s => s.name == "fuse");
            if (fuseSound != null && !fuseSound.source.isPlaying)
            {
                FindObjectOfType<audioManager>().Play("fuse");
            }

            fuseOn = !fuseOn;
        }

        lightControl();

        if (gameObject.name == "elevatorFuse" && playerNear == true && Input.GetKeyDown(KeyCode.E))
        {
            var fuseSound = FindObjectOfType<audioManager>().sounds.FirstOrDefault(s => s.name == "fuse");
            if (fuseSound != null && !fuseSound.source.isPlaying)
            {
                FindObjectOfType<audioManager>().Play("elevatorOn");
                FindObjectOfType<audioManager>().Play("fuse");
            }

            fuseAnimator.SetTrigger("pushed");
            otherFuseOn = !otherFuseOn;
        }
    }

    public void lightControl()
    {
        if (fuseOn == false)
        {
            foreach (Light light in lights)
            {
                if (light == true)
                {
                    if (light.CompareTag("CommonLight"))
                    {
                        light.enabled = false; // Disable each light with the tag "CommonLight"
                    }
                }
            }

            foreach (AudioSource sound in sounds)
            {
                if (sound.CompareTag("CommonLight"))
                {
                    sound.enabled = false; // Disable each sound with the tag "CommonLight"
                }
            }
        }

        if (fuseOn == true)
        {
            foreach (Light light in lights)
            {
                if(light == true)
                {
                    light.enabled = true; // enable each light
                }
            }

            foreach (AudioSource sound in sounds)
            {
                if (sound.CompareTag("CommonLight"))
                {
                    sound.enabled = true; // Disable each sound with the tag "CommonLight"
                }
            }
        }
    }
}
