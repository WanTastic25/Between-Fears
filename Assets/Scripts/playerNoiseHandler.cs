using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNoiseHandler : MonoBehaviour
{
    public PlayerController playerControl;
    public SphereCollider noiseColliderTrigger;

    private void Start()
    {
        //playerControl = GetComponent<PlayerController>();
        //noiseColliderTrigger = GetComponent<SphereCollider>();

        noiseColliderTrigger.radius = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControl.playerRb.velocity != Vector3.zero)
        {
            noiseColliderTrigger.radius = 10f;
        }

        if (playerControl.playerRb.velocity != Vector3.zero && playerControl.crouched == true)
        {
            noiseColliderTrigger.radius = 0.5f;
        }

        if (playerControl.playerRb.velocity != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            noiseColliderTrigger.radius = 15f;
        }

        if (playerControl.playerRb.velocity == Vector3.zero)
        {
            noiseColliderTrigger.radius = 0.25f;
        }
    }
}
