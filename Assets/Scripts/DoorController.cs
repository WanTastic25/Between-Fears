using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    bool toggle;
    public bool keyCheck;
    bool playerNear;
    public Animator doorAnim;
    public Animator elevAnim;

    public keypadInteraction keypadInteraction;
    public dialogueTrigger dialogueTrigger;
    public GameObject keypadInteractionObj;

    public UnityEvent OnDoorOpened;
    public UnityEvent OnDoorReached;

    public FuseHandler fuseHandler;

    private void Awake()
    {
        keypadInteractionObj = GameObject.Find("KeypadLogic");
        keypadInteraction = keypadInteractionObj.GetComponent<keypadInteraction>();
        keyCheck = false;
        //Get the dialogue trigger
        dialogueTrigger = gameObject.GetComponent<dialogueTrigger>();
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

        if (playerNear && gameObject.name == "ElevatorDoor" && fuseHandler.otherFuseOn == true)
        {
            dialogueTrigger.enabled = false;
            if (Input.GetKeyDown(KeyCode.E))
            {
                openCloseElevator();
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && playerNear && keyCheck == true)
        {
            openClose();
        }

        if (playerNear && keypadInteraction.keypadUnlock == true && gameObject.name == "SecurityDoor")
        {
            dialogueTrigger.enabled = false;
        }

        if (playerNear && keyCheck == true && gameObject.name == "secHomeDoor")
        {
            dialogueTrigger.enabled = false;
        }

        if (playerNear && keyCheck == true && gameObject.name == "MaintenanceDoor")
        {
            dialogueTrigger.enabled = false;
        }

        if (playerNear && keyCheck == true && gameObject.name == "MaintenanceDoor2")
        {
            dialogueTrigger.enabled = false;
        }

        if (playerNear && keyCheck == true && gameObject.name == "HomeDoor")
        {
            dialogueTrigger.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && playerNear && keypadInteraction.keypadUnlock == true && gameObject.name == "SecurityDoor")
        {
            openClose();
        }

        OnDoorReached?.Invoke();
    }

    public void openClose()
    {
        toggle = !toggle;
        openDoor();

        if (toggle == false)
        {
            FindObjectOfType<audioManager>().Play("doorClose");
            doorAnim.SetTrigger("close");
        }

        if (toggle == true)
        {
            FindObjectOfType<audioManager>().Play("doorOpen");
            doorAnim.SetTrigger("open");
        }
    }

    public void openDoor()
    {
        OnDoorOpened?.Invoke();
    }

    public void openCloseElevator()
    {
        toggle = !toggle;

        if (toggle == false)
        {
            FindObjectOfType<audioManager>().Play("elevatorClose");
            elevAnim.SetTrigger("close");
        }

        if (toggle == true)
        {
            FindObjectOfType<audioManager>().Play("elevatorOpen");
            elevAnim.SetTrigger("open");
        }
    }
}
