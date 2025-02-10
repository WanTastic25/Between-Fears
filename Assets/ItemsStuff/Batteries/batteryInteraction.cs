using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class batteryInteraction : MonoBehaviour
{
    public UnityEvent batteryEvents;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            KinematicHandler KinematicHandler = player.GetComponent<KinematicHandler>();
            if (KinematicHandler != null)
            {
                batteryEvents.AddListener(KinematicHandler.gainBattery);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<audioManager>().Play("batteryPickUp");
            batteryEvents.Invoke();
            Destroy(this.gameObject);
        }
    }
}
