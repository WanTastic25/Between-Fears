using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameEndLogic : MonoBehaviour
{
    // Call this method to reset the map
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Reload the currently active scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
