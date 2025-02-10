using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storyteller : MonoBehaviour
{
    public GameObject enemy;
    public GameObject storyPoint1;

    public FuseHandler fuseHandler;

    // Start is called before the first frame update
    void Start()
    {
        enemy.SetActive(false);

        GameObject normalFuseObject = GameObject.Find("Fuse");
        if (normalFuseObject != null)
        {
            fuseHandler = normalFuseObject.GetComponent<FuseHandler>();
        }
        else
        {
            Debug.LogWarning("normalFuse not found");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the storyPoint1
        if (other.gameObject.CompareTag("Player"))
        {
            // Turn off lights
            fuseHandler.fuseOn = false;
            FindObjectOfType<audioManager>().Play("lightsOff");

            this.gameObject.SetActive(false);

            // Spawn or activate the enemy
            if (enemy != null)
            {
                enemy.SetActive(true);
            }

            Debug.Log("StoryPoint1 activated: Lights off, enemy spawned!");
        }
    }
}
