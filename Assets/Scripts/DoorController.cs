using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    bool toggle;
    public bool keyCheck;
    bool playerNear;
    public Animator doorAnim;

    private void Awake()
    {
        keyCheck = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerNear = false;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerNear && gameObject.name == "normalDoor")
        {
            openClose();
        }

        if (Input.GetKeyDown(KeyCode.E) && playerNear && keyCheck == true)
        {
            openClose();
        }

        if (Input.GetKeyDown(KeyCode.E) && playerNear && keyCheck == false && gameObject.name != "normalDoor")
        {
            Debug.Log("Door is Locked, Find a Key");
            Debug.Log(gameObject.tag);
        }
    }

    public void openClose()
    {
        toggle = !toggle;

        if (toggle == false)
        {
            Debug.Log("door closes");
            doorAnim.SetTrigger("close");
        }

        if (toggle == true)
        {
            Debug.Log("door opens");
            doorAnim.SetTrigger("open");
        }
    }
}
