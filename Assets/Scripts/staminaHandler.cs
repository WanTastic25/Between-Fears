using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class staminaHandler : MonoBehaviour
{
    public float maxStamina;
    public float currentStamina;
    public float restoreRate;
    public bool exhaustion;
    public Image staminaBar;
    public PlayerController player;

    private void Start()
    {
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        staminaBar.fillAmount = currentStamina / maxStamina;

        if (player.direction.magnitude >= 0.1f) // if player is not standing still
        {
            if (Input.GetKey(KeyCode.LeftShift) && !player.crouched) //If shift is pressed and player not crouching, stamina drains
            {
                currentStamina -= 10 * Time.deltaTime;
            }
            else // Player is walking
            {
                if (currentStamina < maxStamina)
                {
                    currentStamina += 2 * Time.deltaTime;
                }
            }
        }
        else if (player.direction.magnitude == 0) // if player is standing still
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += 3.5f * Time.deltaTime;
            }
        }

        if (currentStamina <= 0) // if current stamina is 0 or less
        {
            //player stops
            StartCoroutine(exhausted());
        }
    }

    IEnumerator exhausted()
    {
        FindObjectOfType<audioManager>().Play("tired");
        player.playerAnim.SetInteger("trigger", 5);
        player.moveSpeed = 0;
        player.tired = true;
        player.direction = Vector3.zero;

        yield return new WaitForSeconds(2.5f);

        float regenRate = 10f; // Rate at which stamina regenerates per second

        while (currentStamina < 40)
        {
            currentStamina += regenRate * Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        player.playerAnim.SetInteger("trigger", 0);

        yield return new WaitForSeconds(1f);

        FindObjectOfType<audioManager>().Stop("tired");
        player.tired = false;
        player.moveSpeed = player.normalSpeed;
    }
}
