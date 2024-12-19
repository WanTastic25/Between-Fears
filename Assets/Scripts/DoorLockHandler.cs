using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockHandler : MonoBehaviour
{
    public playerInteractions playerInteractions;
    public GameObject door1;
    public GameObject door2;
    public DoorController doorController;

    private void Start()
    {
        door1 = GameObject.Find("Door1");
        door2 = GameObject.Find("Door2");
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
    }
}
