using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockHandler : MonoBehaviour
{
    public playerInteractions playerInteractions;

    public GameObject door1;
    public GameObject door2;
    public GameObject door3;
    public GameObject door4;
    public DoorController doorController;

    private void Start()
    {
        door1 = GameObject.Find("SecurityHomeDoor");
        door2 = GameObject.Find("MaintenanceDoor1");
        door4 = GameObject.Find("MaintenanceDoor2");
        door3 = GameObject.Find("PlayerDoor");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInteractions.key1 == true)
        {
            doorController = door1.GetComponentInChildren<DoorController>();
            doorController.keyCheck = true;
        }

        if (playerInteractions.key2 == true)
        {
            doorController = door2.GetComponentInChildren<DoorController>();
            doorController.keyCheck = true;
        }

        if (playerInteractions.key4 == true)
        {
            doorController = door4.GetComponentInChildren<DoorController>();
            doorController.keyCheck = true;
        }

        if (playerInteractions.key3 == true)
        {
            doorController = door3.GetComponentInChildren<DoorController>();
            doorController.keyCheck = true;
        }
    }
}
