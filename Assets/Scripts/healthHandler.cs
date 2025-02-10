using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthHandler : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public bool injured;
    public Image healthBar;
    public PlayerController player;
    public GameObject spawnPoint;
    public Animator playerAnims;

    void Start()
    {
        playerAnims = player.GetComponent<Animator>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth > 100f)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth <= 0f)
        {
            playerDie();
        }
    }

    public void damagedByEnemy()
    {
        float enemyDamage = 10f;
        //Method to make player lose health
        currentHealth -= enemyDamage;        
    }

    public void gainHealth()
    {
        FindObjectOfType<audioManager>().Play("medKitPickUp");
        currentHealth += 5f;
    }

    //Method to kill player
    public void playerDie()
    {
        player.transform.position = spawnPoint.transform.position;
        currentHealth = maxHealth;
    }
}
