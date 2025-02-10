using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MedPackInteractions : MonoBehaviour
{
    public UnityEvent MedPackEvents;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            healthHandler healthHandler = player.GetComponent<healthHandler>();
            if (healthHandler != null)
            {
                MedPackEvents.AddListener(healthHandler.gainHealth);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MedPackEvents.Invoke();
            Destroy(this.gameObject);
        }
    }
}
